using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [Header("Health")]
    public int health = 6;

    [Header("Health and Ammo Drops")]
    public GameObject healthPackPrefab;
    public GameObject ammoPackPrefab;

    public float healthDropChance = 0.15f;
    public float ammoDropChance = 0.20f;

    [Header("Weapon Drops")]
    public GameObject assaultRiflePickupPrefab;
    public GameObject shotgunPickupPrefab;

    public float weaponDropChance = 0.15f;

    private bool isDead;

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        if (health <= 0)
        {
            isDead = true;

            // Try to drop one item when the zombie dies
            TryDropItem();

            Destroy(gameObject);
        }
    }

    void TryDropItem()
    {
        // One random roll determines which item drops
        float dropRoll = Random.value;

        // Health pack drop
        if (dropRoll < healthDropChance &&
            healthPackPrefab != null)
        {
            Instantiate(
                healthPackPrefab,
                transform.position,
                Quaternion.identity
            );

            return;
        }

        // Ammo pack drop
        if (dropRoll <
            healthDropChance + ammoDropChance &&
            ammoPackPrefab != null)
        {
            Instantiate(
                ammoPackPrefab,
                transform.position,
                Quaternion.identity
            );

            return;
        }

        // Weapon drop
        float weaponDropStart =
            healthDropChance + ammoDropChance;

        if (dropRoll <
            weaponDropStart + weaponDropChance)
        {
            WeaponPickup.TryDropFromZombie(
                transform.position,
                assaultRiflePickupPrefab,
                shotgunPickupPrefab,
                1f
            );
        }
    }
}