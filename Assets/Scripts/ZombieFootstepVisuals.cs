using System.Collections.Generic;
using UnityEngine;

public class ZombieFootstepVisuals : MonoBehaviour
{
    public GameObject footstepPrefab;

    public float stepInterval = 0.55f;
    public float sideOffset = 0.1f;

    public float visionLength = 8f;
    public float visionAngle = 70f;

    public int maximumFootsteps = 4;

    private Transform player;
    private float stepTimer;
    private Vector2 previousPosition;
    private bool leftFoot;

    private List<GameObject> activeFootsteps =
        new List<GameObject>();

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        previousPosition =
            transform.position;

        stepTimer =
            Random.Range(0f, stepInterval);
    }

    void Update()
    {
        if (player == null ||
            footstepPrefab == null)
        {
            return;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            Vector2 currentPosition =
                transform.position;

            Vector2 movementDirection =
                currentPosition -
                previousPosition;

            if (movementDirection.sqrMagnitude > 0.001f)
            {
                if (IsOutsideVisionCone())
                {
                    CreateFootstep(
                        currentPosition,
                        movementDirection.normalized
                    );
                }

                previousPosition =
                    currentPosition;

                leftFoot =
                    !leftFoot;
            }

            stepTimer =
                stepInterval *
                Random.Range(0.85f, 1.15f);
        }
    }

    bool IsOutsideVisionCone()
    {
        Vector2 directionToZombie =
            transform.position -
            player.position;

        float distance =
            directionToZombie.magnitude;

        float angle =
            Vector2.Angle(
                player.up,
                directionToZombie
            );

        bool insideVisionCone =
            distance <= visionLength &&
            angle <= visionAngle / 2f;

        return !insideVisionCone;
    }

    void CreateFootstep(
        Vector2 position,
        Vector2 movementDirection
    )
    {
        activeFootsteps.RemoveAll(
            footprint => footprint == null
        );

        if (activeFootsteps.Count >= maximumFootsteps)
        {
            Destroy(activeFootsteps[0]);
            activeFootsteps.RemoveAt(0);
        }

        Vector2 sideDirection =
            new Vector2(
                -movementDirection.y,
                movementDirection.x
            );

        float footSide =
            leftFoot ? -sideOffset : sideOffset;

        Vector2 footprintPosition =
            position +
            sideDirection * footSide;

        GameObject footprint =
            Instantiate(
                footstepPrefab,
                footprintPosition,
                Quaternion.identity
            );

        float rotationAngle =
            Mathf.Atan2(
                movementDirection.y,
                movementDirection.x
            ) * Mathf.Rad2Deg - 90f;

        footprint.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                rotationAngle
            );

        activeFootsteps.Add(footprint);
    }
}