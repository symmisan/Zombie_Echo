using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    // Health pickup disappears after 7 seconds
    public float lifetime = 7f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only allow the player to collect health
        if (other.CompareTag("Player"))
        {
            PlayerHealth health =
                other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
