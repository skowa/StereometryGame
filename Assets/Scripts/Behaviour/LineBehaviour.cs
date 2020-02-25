using UnityEngine;

public class LineBehaviour : MonoBehaviour
{
    private Vector3 _start;
    private Vector3 _end;
    private Material _material;
    private bool _isLongLine;
    private bool _isArMode;

    public void InstantiateFields(Vector3 start, Vector3 end, Material material, bool isLongLine, bool isArMode)
    {
        _start = start;
        _end = end;
        _material = material;
        _isLongLine = isLongLine;
        _isArMode = isArMode;
    }

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = _material;
        lineRenderer.positionCount = 2;

        lineRenderer.startWidth = _isArMode ? 0.009f : 0.03f;
        lineRenderer.endWidth = _isArMode ? 0.009f : 0.03f;

        lineRenderer.SetPosition(0, _start);
        lineRenderer.SetPosition(1, _end);
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

        collider.transform.position = _start + (_end - _start) / 2;
        collider.transform.LookAt(_start);
        collider.height = (_end - _start).magnitude;

        if (_isLongLine)
        {
            collider.height *= 3;
        }
    }
}
