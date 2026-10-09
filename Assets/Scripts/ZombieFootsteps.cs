using UnityEngine;

public class ZombieFootsteps : MonoBehaviour
{
    public float lifetime = 1f;

    private SpriteRenderer spriteRenderer;
    private Color startingColor;
    private float timer;

    void Start()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        startingColor =
            spriteRenderer.color;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float transparency =
            1f - timer / lifetime;

        Color newColor =
            startingColor;

        newColor.a =
            startingColor.a * transparency;

        spriteRenderer.color =
            newColor;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}