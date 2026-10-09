using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [Header("Vision Cone Settings")]
    public float coneLength = 5.6f;
    public float coneAngle = 70f;

    [Range(3, 180)]
    public int segments = 80;

    [Header("Wall Detection")]
    public LayerMask wallLayers;

    [Header("Vision Cone Color")]
    public Color coneColor =
        new Color(1f, 0.9f, 0.35f, 0.18f);

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh coneMesh;

    void Awake()
    {
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
        coneMesh.name = "Vision Cone Mesh";

        meshFilter.mesh = coneMesh;

        Material material = new Material(
            Shader.Find("Sprites/Default")
        );

        material.color = coneColor;

        meshRenderer.material = material;
        meshRenderer.sortingOrder = 1;
    }

    void LateUpdate()
    {
        CreateCone();
    }

    void CreateCone()
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
                -coneAngle / 2f +
                coneAngle * i / segmentCount;

            Vector3 localDirection =
                Quaternion.Euler(0f, 0f, angle)
                * Vector3.up;

            Vector2 worldDirection =
                transform.TransformDirection(localDirection);

            worldDirection.Normalize();

            // Detect walls in each direction.
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
