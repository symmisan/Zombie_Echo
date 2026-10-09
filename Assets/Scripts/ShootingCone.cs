
using UnityEngine;

public class ShootingCone : MonoBehaviour
{
    [Header("Shooting Cone Settings")]
    [Range(3, 180)]
    public int segments = 80;

    [Header("Wall Detection")]
    public LayerMask wallLayers;

    [Header("Shooting Cone Color")]
    public Color coneColor =
        new Color(1f, 0.15f, 0.1f, 0.22f);

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh coneMesh;

    private PlayerShooting playerShooting;

    void Awake()
    {
        playerShooting =
            GetComponentInParent<PlayerShooting>();

        meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        coneMesh = new Mesh();
        coneMesh.name = "Shooting Cone Mesh";

        meshFilter.mesh = coneMesh;

        Material material = new Material(
            Shader.Find("Sprites/Default")
        );

        material.color = coneColor;

        meshRenderer.material = material;
        meshRenderer.sortingOrder = 2;
    }

    void LateUpdate()
    {
        if (playerShooting == null)
            return;

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        bool isMoving =
            Mathf.Abs(horizontal) > 0.1f ||
            Mathf.Abs(vertical) > 0.1f;

        float currentAngle = isMoving
            ? playerShooting.CurrentMovingConeAngle
            : playerShooting.CurrentStandingConeAngle;

        float currentLength =
            playerShooting.CurrentShootingRange * 0.7f;

        CreateCone(currentAngle, currentLength);
    }

    void CreateCone(float angleSize, float coneLength)
    {
        int segmentCount = Mathf.Max(3, segments);

        coneMesh.Clear();

        Vector3[] vertices =
            new Vector3[segmentCount + 2];

        int[] triangles =
            new int[segmentCount * 3];

        vertices[0] = Vector3.zero;

        Vector2 origin = transform.position;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle =
                -angleSize / 2f +
                angleSize * i / segmentCount;

            Vector3 localDirection =
                Quaternion.Euler(0f, 0f, angle)
                * Vector3.up;

            Vector2 worldDirection =
                transform.TransformDirection(localDirection);

            worldDirection.Normalize();

            // NEW: Stop shooting cone at walls.
            RaycastHit2D wallHit =
                Physics2D.Raycast(
                    origin,
                    worldDirection,
                    coneLength,
                    wallLayers
                );

            float rayDistance = coneLength;

            if (wallHit.collider != null)
            {
                rayDistance = wallHit.distance;
            }

            Vector3 worldEndpoint =
                transform.position +
                (Vector3)(worldDirection * rayDistance);

            vertices[i + 1] =
                transform.InverseTransformPoint(worldEndpoint);
        }

        for (int i = 0; i < segmentCount; i++)
        {
            int triangleIndex = i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;

        coneMesh.RecalculateBounds();
    }

    void OnDestroy()
    {
        if (coneMesh != null)
        {
            Destroy(coneMesh);
        }

        if (meshRenderer != null &&
            meshRenderer.material != null)
        {
            Destroy(meshRenderer.material);
        }
    }
}
