using UnityEngine;

// All weapons available in the game
public enum WeaponType
{
    Pistol,
    AssaultRifle,
    Shotgun
}

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Sounds")]
    public AudioClip pistolSound;
    public AudioClip assaultRifleSound;
    public AudioClip shotgunSound;

    [Range(0f, 1f)]
    public float gunshotVolume = 0.2f;

    private AudioSource gunAudioSource;

    [Header("Weapon Visuals")]
    public WeaponVisuals weaponVisuals;

    [Header("Wall Collision")]
    public LayerMask wallLayers;

    private WeaponType currentWeapon;

    private float shootingRange;
    private float fireCooldown;
    private int damage;

    private float standingSpreadAngle = 10f;
    private float movingSpreadAngle = 35f;

    private int magazineSize;
    private int currentAmmo;
    private int reserveAmmo;
    private int maxReserveAmmo;
    private float reloadTime;

    private float nextFireTime;
    private float reloadTimer;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;

    public int ReserveAmmo =>
        currentWeapon == WeaponType.Pistol
            ? 999999
            : reserveAmmo;

    public string AmmoDisplay =>
        currentWeapon == WeaponType.Pistol
            ? currentAmmo + "/∞"
            : currentAmmo + "/" + reserveAmmo;

    public float CurrentShootingRange => shootingRange;

    public float CurrentStandingConeAngle =>
        standingSpreadAngle;

    public float CurrentMovingConeAngle =>
        movingSpreadAngle;

    public string CurrentWeaponName =>
        currentWeapon.ToString();

    void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();

        if (gunAudioSource == null)
        {
            gunAudioSource =
                gameObject.AddComponent<AudioSource>();
        }

        gunAudioSource.playOnAwake = false;
        gunAudioSource.loop = false;
        gunAudioSource.spatialBlend = 0f;
    }

    void Start()
    {
        if (weaponVisuals == null)
        {
            weaponVisuals =
                GetComponentInChildren<WeaponVisuals>(true);
        }

        EquipWeapon(WeaponType.Pistol);
    }

    void Update()
    {
        // Automatically switch to pistol when
        // rifle or shotgun runs out of all ammo.
        if (currentWeapon != WeaponType.Pistol &&
            currentAmmo <= 0 &&
            reserveAmmo <= 0)
        {
            EquipWeapon(WeaponType.Pistol);
        }

        // Handle reload timer.
        if (isReloading)
        {
            reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0f)
            {
                FinishReload();
            }

            return;
        }

        // Manual reload.
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }

        // Automatic reload.
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        // Automatically fire at zombies.
        if (Time.time >= nextFireTime)
        {
            ZombieHealth target =
                FindZombieInsideShootingCone();

            if (target != null)
            {
                ShootAt(target);

                PlayGunshotSound();

                currentAmmo--;

                nextFireTime =
                    Time.time + fireCooldown;

                if (currentAmmo <= 0)
                {
                    if (currentWeapon != WeaponType.Pistol &&
                        reserveAmmo <= 0)
                    {
                        EquipWeapon(WeaponType.Pistol);
                    }
                    else
                    {
                        StartReload();
                    }
                }
            }
        }
    }

    void PlayGunshotSound()
    {
        AudioClip selectedSound = null;

        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                selectedSound = pistolSound;
                break;

            case WeaponType.AssaultRifle:
                selectedSound = assaultRifleSound;
                break;

            case WeaponType.Shotgun:
                selectedSound = shotgunSound;
                break;
        }

        if (selectedSound != null &&
            gunAudioSource != null)
        {
            gunAudioSource.PlayOneShot(
                selectedSound,
                gunshotVolume
            );
        }
    }

    // Equip weapons collected around the arena.
    public void CollectWeapon(WeaponType weapon)
    {
        EquipWeapon(weapon);
    }

    void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;

        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                shootingRange = 6f;
                fireCooldown = 0.35f;
                damage = 2;

                magazineSize = 12;
                reserveAmmo = 999999;
                maxReserveAmmo = 999999;
                reloadTime = 1.2f;
                break;

            case WeaponType.AssaultRifle:
                shootingRange = 8f;
                fireCooldown = 0.12f;
                damage = 2;

                magazineSize = 30;
                reserveAmmo = 150;
                maxReserveAmmo = 150;
                reloadTime = 1.5f;
                break;

            case WeaponType.Shotgun:
                shootingRange = 4f;
                fireCooldown = 0.8f;
                damage = 6;

                magazineSize = 6;
                reserveAmmo = 24;
                maxReserveAmmo = 24;
                reloadTime = 1.8f;
                break;
        }

        currentAmmo = magazineSize;

        if (weaponVisuals != null)
        {
            weaponVisuals.SetWeapon(currentWeapon);
        }

        isReloading = false;
        reloadTimer = 0f;
        nextFireTime = Time.time;
    }

    // Check if a wall blocks the shot.
    bool IsWallBlocking(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;

        if (distance <= 0f)
            return false;

        RaycastHit2D wallHit =
            Physics2D.Raycast(
                start,
                direction.normalized,
                distance,
                wallLayers
            );

        return wallHit.collider != null;
    }

    // Find the closest zombie inside the cone.
    ZombieHealth FindZombieInsideShootingCone()
    {
        ZombieHealth closestZombie = null;
        float closestDistance = Mathf.Infinity;

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        bool isMoving =
            Mathf.Abs(horizontal) > 0.1f ||
            Mathf.Abs(vertical) > 0.1f;

        float shootingConeAngle = isMoving
            ? movingSpreadAngle
            : standingSpreadAngle;

        ZombieHealth[] zombies =
            FindObjectsByType<ZombieHealth>();

        foreach (ZombieHealth zombie in zombies)
        {
            if (zombie == null)
                continue;

            Vector2 directionToZombie =
                zombie.transform.position -
                transform.position;

            float distance =
                directionToZombie.magnitude;

            float angle =
                Vector2.Angle(
                    transform.up,
                    directionToZombie
                );

            bool insideShootingCone =
                distance <= shootingRange * 0.7f &&
                angle <= shootingConeAngle / 2f;

            if (!insideShootingCone)
                continue;

            // Ignore zombies behind walls.
            if (IsWallBlocking(
                transform.position,
                zombie.transform.position))
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closestZombie = zombie;
                closestDistance = distance;
            }
        }

        return closestZombie;
    }

    // Shoot at the selected zombie.
    void ShootAt(ZombieHealth target)
    {
        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        bool isMoving =
            Mathf.Abs(horizontal) > 0.1f ||
            Mathf.Abs(vertical) > 0.1f;

        float currentSpread = isMoving
            ? movingSpreadAngle
            : standingSpreadAngle;

        Vector2 direction =
            target.transform.position -
            transform.position;

        direction.Normalize();

        float randomAngle =
            Random.Range(
                -currentSpread / 2f,
                currentSpread / 2f
            );

        direction =
            Quaternion.Euler(
                0f,
                0f,
                randomAngle
            ) * direction;

        Vector2 startPoint =
            (Vector2)transform.position +
            direction * 0.4f;

        // Check if a wall is hit first.
        RaycastHit2D wallHit =
            Physics2D.Raycast(
                startPoint,
                direction,
                shootingRange,
                wallLayers
            );

        float maximumBulletDistance = shootingRange;

        if (wallHit.collider != null)
        {
            maximumBulletDistance = wallHit.distance;
        }

        // Detect zombies along the bullet path.
        RaycastHit2D[] hits =
            Physics2D.RaycastAll(
                startPoint,
                direction,
                maximumBulletDistance
            );

        ZombieHealth closestZombie = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit2D hit in hits)
        {
            // Ignore pickups.
            if (hit.collider.GetComponentInParent<HealthPickup>() != null ||
                hit.collider.GetComponentInParent<AmmoPickup>() != null ||
                hit.collider.GetComponentInParent<WeaponPickup>() != null)
            {
                continue;
            }

            // Stop at a wall.
            if ((wallLayers.value &
                (1 << hit.collider.gameObject.layer)) != 0)
            {
                continue;
            }

            ZombieHealth zombie =
                hit.collider.GetComponentInParent<ZombieHealth>();

            if (zombie != null &&
                hit.distance < closestDistance)
            {
                closestZombie = zombie;
                closestDistance = hit.distance;
            }
        }

        if (closestZombie != null)
        {
            closestZombie.TakeDamage(damage);
        }
    }

    void StartReload()
    {
        if (isReloading ||
            currentAmmo >= magazineSize)
        {
            return;
        }

        if (currentWeapon != WeaponType.Pistol &&
            reserveAmmo <= 0)
        {
            if (currentAmmo <= 0)
            {
                EquipWeapon(WeaponType.Pistol);
            }

            return;
        }

        isReloading = true;
        reloadTimer = reloadTime;

        if (weaponVisuals != null)
        {
            weaponVisuals.PlayReloadAnimation(currentWeapon);
        }
    }

    void FinishReload()
    {
        if (currentWeapon == WeaponType.Pistol)
        {
            currentAmmo = magazineSize;
        }
        else
        {
            int ammoNeeded =
                magazineSize - currentAmmo;

            int ammoToLoad =
                Mathf.Min(ammoNeeded, reserveAmmo);

            currentAmmo += ammoToLoad;
            reserveAmmo -= ammoToLoad;
        }

        isReloading = false;
        reloadTimer = 0f;
    }

    public void AddAmmo(int amount)
    {
        if (currentWeapon == WeaponType.Pistol)
        {
            return;
        }

        reserveAmmo =
            Mathf.Min(
                reserveAmmo + amount,
                maxReserveAmmo
            );
    }
}
