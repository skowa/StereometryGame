using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CreateLine(transform.position.x -0.3f, transform.position.y -0.3f, 0,
		 transform.position.x + 0.3f, transform.position.y + 0.3f, 0);
	}

	private void CreateLine(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetWidth(0.05f, 0.05f);
        lineRenderer.SetPosition(0, new Vector3(x1, y1, z1));
        lineRenderer.SetPosition(1, new Vector3(x2, y2, z2));
        lineRenderer.useWorldSpace = false;
	}
}
