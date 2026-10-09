
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 24;

    // NEW: Pickup disappears after 7 seconds
    public float lifetime = 7f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooting shooting =
                other.GetComponent<PlayerShooting>();

            if (shooting != null)
            {
                shooting.AddAmmo(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
