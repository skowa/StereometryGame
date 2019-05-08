using UnityEngine;

public class CreateCircleBehaviour : MonoBehaviour {
    public float radius;
    private float theta_scale = 0.01f;
    private int size;
    private LineRenderer lineRenderer;
 
    private void Awake()
    {
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = (int)sizeValue;
        size++;

        GameObject line = new GameObject();
        line.transform.SetParent(transform);

        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color")) {color = Color.white};
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = size;
    }
 
    private void Update()
    {
        Vector3 position;
        float theta = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);

            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);

            x += gameObject.transform.position.x;
            y += gameObject.transform.position.y;

            position = new Vector3(x, y, 1);
            lineRenderer.SetPosition(i, position);
        }
    }
}
