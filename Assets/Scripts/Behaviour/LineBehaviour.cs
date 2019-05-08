using UnityEngine;

public class LineBehaviour : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private Material material;
    private bool isLongLine;

    public void InstantiateFields(Vector3 start, Vector3 end, Material material, bool isLongLine)
    {
        this.start = start;
        this.end = end;
        this.material = material;
        this.isLongLine = isLongLine;
    }

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.positionCount = 2;
        lineRenderer.SetWidth(0.03f, 0.03f);
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.useWorldSpace = false;
    }

    public void AddColliderToLineObject()
    {
        GameObject colliderWrapper = new GameObject();
        colliderWrapper.transform.SetParent(transform);

        CapsuleCollider collider = colliderWrapper.AddComponent<CapsuleCollider>();
        collider.radius = 0.04f;
        collider.center = Vector3.zero;
        collider.direction = 2;

        collider.transform.position = start + (end - start) / 2;
        collider.transform.LookAt(start);
        collider.height = (end - start).magnitude;

        if (isLongLine)
        {
            collider.height *= 3;
        }
    }
}
