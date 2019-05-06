using UnityEngine;

public class CreateDotBehaviour : MonoBehaviour {
	float theta_scale = 0.01f; 
    int size; 
    float radius = 0.03f;
    LineRenderer lineRenderer;

    private void OnMouseUpAsButton()
    {
        CreateButtonsHelper.Action = ActionType.Dot;

        var okButton = GameObject.Find("OK");
        okButton.GetComponent<Renderer>().enabled = true;
        okButton.GetComponent<Collider2D>().enabled = true;
    }

    void Awake()
    {
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = (int)sizeValue;
        size++;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.white;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = size;
    }
 
    void Update()
    {
        Vector3 pos;
        float theta = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            x += gameObject.transform.position.x;
            y += gameObject.transform.position.y;
            pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
        }
    }
}
