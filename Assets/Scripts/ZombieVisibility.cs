using UnityEngine;

public class ZombieVisibility : MonoBehaviour
{
    public float visionLength = 8f;
    public float visionAngle = 70f;

    private Transform player;
    private SpriteRenderer[] zombieRenderers;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        zombieRenderers =
            GetComponentsInChildren<SpriteRenderer>();

        SetZombieVisible(false);
    }

    void Update()
    {
        if (player == null ||
            !player.gameObject.activeInHierarchy)
        {
            SetZombieVisible(false);
            return;
        }

        Vector2 directionToZombie =
            transform.position - player.position;

        float distance =
            directionToZombie.magnitude;

        // Check how far the zombie is from the player's facing direction
        float angle =
            Vector2.Angle(
                player.up,
                directionToZombie
            );

        // Zombie must be within both the vision range and angle
        bool insideVisionCone =
            distance <= visionLength &&
            angle <= visionAngle / 2f;

        SetZombieVisible(insideVisionCone);
    }

    void SetZombieVisible(bool visible)
    {
        if (zombieRenderers == null)
            return;

        // Show or hide every sprite attached to the zombie
        foreach (
            SpriteRenderer renderer
            in zombieRenderers
        )
        {
            renderer.enabled = visible;
        }
    }
}
