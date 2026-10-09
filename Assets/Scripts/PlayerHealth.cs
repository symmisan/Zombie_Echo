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
        currentHealth =
            Mathf.Min(currentHealth + amount, maxHealth);
    }
}