using UnityEngine;
using TMPro;

public class GameStatusUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText;

    public PlayerHealth playerHealth;
    public WaveManager waveManager;

    void Update()
    {
        if (playerHealth != null)
        {
            healthText.text =
                "HEALTH: " +
                playerHealth.CurrentHealth +
                " / " +
                playerHealth.MaxHealth;
        }

        if (waveManager != null)
        {
            waveText.text =
                "WAVE: " +
                waveManager.CurrentWave;
        }
    }
}