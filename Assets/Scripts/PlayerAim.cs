using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Camera playerCamera;

    void Update()
    {
        Vector3 mousePosition =
            playerCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = 0f;

        Vector2 direction =
            mousePosition - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.up = direction;
        }
    }
}