using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TransparentBehaviour : MonoBehaviour
{
    private float rotSpeed = 3;

    private Vector3[][] edges;
    
	void Start ()
	{
        InitEdges();
		CreateShape();
		CreateLetters();        
		transform.Rotate(-8, -12, 0.5f);
	}
	
	private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }

	private void CreateShape() 
	{
	    foreach (var list in edges)
	    {
	        CreateLine(list[0], list[1]);
	    }
	}

	private void CreateLetters() 
	{
		CreateText(-0.53f, 0.59f, -0.5f, "A1");
		CreateText(0.5f, 0.59f, -0.5f, "B1");
		CreateText(-0.53f, -0.53f, -0.5f, "C1");
		CreateText(0.5f, -0.53f, -0.5f, "D1");

		CreateText(-0.53f, 0.59f, 0.5f, "A2");
		CreateText(0.5f, 0.59f, 0.5f, "B2");
		CreateText(-0.53f, -0.53f, 0.5f, "C2");
		CreateText(0.5f, -0.53f, 0.5f, "D2");
	}

    private void CreateLine(Vector3 first, Vector3 second)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.gray;
        lineRenderer.positionCount = 2;
        lineRenderer.SetWidth(0.03f, 0.03f);
        lineRenderer.SetPosition(0, first);
        lineRenderer.SetPosition(1, second);
        lineRenderer.useWorldSpace = false;
        addColliderToLine(lineRenderer, first, second);
    }

	private void CreateText(float x1, float y1, float z1, string letter)
	{
		GameObject text = new GameObject();
        text.transform.SetParent(transform);
        TextMesh t = text.AddComponent<TextMesh>();
		t.color = Color.black;
        t.text = letter;
        t.fontSize = 300;
		t.characterSize = 0.009f;
        t.transform.localPosition += new Vector3(x1, y1, z1);
	}

    private void addColliderToLine(LineRenderer line, Vector3 startPos, Vector3 endPos)
    {
        PolygonCollider2D collider = line.gameObject.AddComponent<PolygonCollider2D>();
        collider.points = new [] {new Vector2(startPos.x, startPos.y), new Vector2(endPos.x, endPos.y)};
       // collider.gameObject.AddComponent<Rigidbody2D>();
    }

    private void InitEdges()
    {
        edges = new Vector3[][]
        {
            new []
            {
                new Vector3(-1.5f, 2f, 1.5f), new Vector3(-1.5f, 2f, -1.5f)
            },
            new []
            {
                new Vector3(-1.5f, -2f, 1.5f), new Vector3(-1.5f, -2f, -1.5f)
            },
            new []
            {
                new Vector3(1.5f, 2f, 1.5f), new Vector3(1.5f, 2f, -1.5f)
            },
            new []
            {
                new Vector3(1.5f, -2f, 1.5f), new Vector3(1.5f, -2f, -1.5f)
            },

            new []
            {
                new Vector3(-1.5f, 2f, 1.5f), new Vector3(1.5f, 2f, 1.5f)
            },
            new []
            {
                new Vector3(-1.5f, -2f, 1.5f), new Vector3(1.5f, -2f, 1.5f)
            },
            new []
            {
                new Vector3(1.5f, 2f, 1.5f), new Vector3(-1.5f, 2f, 1.5f)
            },
            new []
            {
                new Vector3(1.5f, -2f, 1.5f), new Vector3(-1.5f, -2f, 1.5f)
            },

            new []
            {
                new Vector3(-1.5f, 2f, 1.5f), new Vector3(-1.5f, -2f, 1.5f)
            },
            new []
            {
                new Vector3(-1.5f, -2f, 1.5f), new Vector3(-1.5f, 2f, 1.5f)
            },
            new []
            {
                new Vector3(1.5f, 2f, 1.5f), new Vector3(1.5f, -2f, 1.5f)
            },
            new []
            {
                new Vector3(1.5f, -2f, 1.5f), new Vector3(1.5f, 2f, 1.5f)
            },

            new []
            {
                new Vector3(-1.5f, 2f, -1.5f), new Vector3(1.5f, 2f, -1.5f)
            },
            new []
            {
                new Vector3(-1.5f, -2f, -1.5f), new Vector3(1.5f, -2f, -1.5f)
            },
            new []
            {
                new Vector3(1.5f, 2f, -1.5f), new Vector3(1.5f, -2f, -1.5f)
            },
            new []
            {
                new Vector3(-1.5f, -2f, -1.5f), new Vector3(-1.5f, 2f, -1.5f)
            }
        };
    }
}
