using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public PlayerShooting playerShooting;

    void Update()
    {
        // Prevent errors if either reference is missing
        if (playerShooting == null || ammoText == null)
            return;

        ammoText.text = playerShooting.AmmoDisplay;
    }
}
