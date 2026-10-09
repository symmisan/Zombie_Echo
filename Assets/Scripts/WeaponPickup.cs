using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon")]
    public WeaponType weaponType = WeaponType.Shotgun;

    [Header("Pickup Lifetime")]
    public float lifetime = 7f;

    void Start()
    {
        // Destroy the pickup after the lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerShooting shooting =
            other.GetComponentInParent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.CollectWeapon(weaponType);

            // Change the player's visible weapon
            WeaponVisuals visuals =
                other.GetComponentInParent<WeaponVisuals>();

            if (visuals != null)
            {
                visuals.SetWeapon(weaponType);
            }

            // Remove the pickup after it is collected
            Destroy(gameObject);
        }
    }

    // Attempts to drop a weapon when a zombie dies
    public static void TryDropFromZombie(
        Vector3 dropPosition,
        GameObject assaultRiflePrefab,
        GameObject shotgunPrefab,
        float dropChance
    )
    {
        dropChance = Mathf.Clamp01(dropChance);

        // Decide whether this zombie drops a weapon
        if (Random.value > dropChance)
            return;

        // Randomly choose the assault rifle or shotgun
        GameObject pickupPrefab =
            Random.value < 0.5f
                ? assaultRiflePrefab
                : shotgunPrefab;

        // If the selected prefab is missing, use the other one
        if (pickupPrefab == null)
        {
            pickupPrefab =
                assaultRiflePrefab != null
                    ? assaultRiflePrefab
                    : shotgunPrefab;
        }

        // Spawn the selected pickup
        if (pickupPrefab != null)
        {
            Instantiate(
                pickupPrefab,
                dropPosition,
                Quaternion.identity
            );
        }
    }
}