
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;

    [Header("Attack Settings")]
    public float attackDistance = 1f;
    public float attackCooldown = 1f;

    [Header("Facing Settings")]
    public Transform visual;
    public float facingOffset = 0f;

    [Header("Contact Damage")]
    public int contactDamage = 1;
    public float damageCooldown = 1f;

    private Transform player;
    private PlayerHealth playerHealth;

    private Rigidbody2D rb;
    private Animator animator;

    private float nextDamageTime;
    private float nextAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponentInChildren<Animator>();

        if (visual == null && animator != null)
        {
            visual = animator.transform;
        }

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;

            playerHealth =
                playerObject.GetComponent<PlayerHealth>();
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        Vector2 direction =
            player.position - transform.position;

        float distance = direction.magnitude;

        // Always face the player.
        if (visual != null && direction != Vector2.zero)
        {
            float angle =
                Mathf.Atan2(
                    direction.y,
                    direction.x
                ) * Mathf.Rad2Deg;

            angle += facingOffset;

            Quaternion targetRotation =
                Quaternion.Euler(0f, 0f, angle);

            visual.rotation =
                Quaternion.RotateTowards(
                    visual.rotation,
                    targetRotation,
                    720f * Time.fixedDeltaTime
                );
        }

        // Attack when close enough.
        if (distance <= attackDistance)
        {
            // Stop moving while attacking.
            rb.linearVelocity = Vector2.zero;

            // Trigger attack animation.
            if (Time.time >= nextAttackTime)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }

                nextAttackTime =
                    Time.time + attackCooldown;
            }

            // NEW: Damage player within attack range.
            if (Time.time >= nextDamageTime)
            {
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);

                    nextDamageTime =
                        Time.time + damageCooldown;
                }
            }

            return;
        }

        // Move toward the player.
        Vector2 nextPosition =
            Vector2.MoveTowards(
                rb.position,
                player.position,
                moveSpeed * Time.fixedDeltaTime
            );

        rb.MovePosition(nextPosition);
    }
}
