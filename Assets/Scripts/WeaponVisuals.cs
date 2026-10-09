using UnityEngine;

public class WeaponVisuals : MonoBehaviour
{
    [Header("Weapon Player Objects")]
    public GameObject pistolPlayer;
    public GameObject assaultRiflePlayer;
    public GameObject shotgunPlayer;

    // Animator for each weapon
    private Animator pistolAnimator;
    private Animator assaultRifleAnimator;
    private Animator shotgunAnimator;

    void Start()
    {
        // Automatically find each Animator, including inactive objects
        if (pistolPlayer != null)
        {
            pistolAnimator =
                pistolPlayer.GetComponentInChildren<Animator>(true);
        }

        if (assaultRiflePlayer != null)
        {
            assaultRifleAnimator =
                assaultRiflePlayer.GetComponentInChildren<Animator>(true);
        }

        if (shotgunPlayer != null)
        {
            shotgunAnimator =
                shotgunPlayer.GetComponentInChildren<Animator>(true);
        }

        // Start with the pistol equipped
        SetWeapon(WeaponType.Pistol);
    }

    // Activates only the selected weapon's visual
    public void SetWeapon(WeaponType weapon)
    {
        if (pistolPlayer != null)
        {
            pistolPlayer.SetActive(
                weapon == WeaponType.Pistol
            );
        }

        if (assaultRiflePlayer != null)
        {
            assaultRiflePlayer.SetActive(
                weapon == WeaponType.AssaultRifle
            );
        }

        if (shotgunPlayer != null)
        {
            shotgunPlayer.SetActive(
                weapon == WeaponType.Shotgun
            );
        }
    }

    // Plays the reload animation for the currently equipped weapon
    public void PlayReloadAnimation(WeaponType weapon)
    {
        Animator selectedAnimator = null;

        switch (weapon)
        {
            case WeaponType.Pistol:
                selectedAnimator = pistolAnimator;
                break;

            case WeaponType.AssaultRifle:
                selectedAnimator = assaultRifleAnimator;
                break;

            case WeaponType.Shotgun:
                selectedAnimator = shotgunAnimator;
                break;
        }

        if (selectedAnimator != null)
        {
            // This must match the Animator trigger name
            selectedAnimator.SetTrigger("Reload");
        }
    }
}