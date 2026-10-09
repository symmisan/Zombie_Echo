using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Prevent health from dropping below zero
        currentHealth =
            Mathf.Max(currentHealth - damage, 0);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over");
            gameObject.SetActive(false);
        }
    }

    public void Heal(int amount)
    {
        // Prevent healing beyond the maximum health
        currentHealth =
            Mathf.Min(currentHealth + amount, maxHealth);
    }
}
