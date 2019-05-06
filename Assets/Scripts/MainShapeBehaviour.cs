using System;
using UnityEngine;

public class MainShapeBehaviour : MonoBehaviour
{
    private float _rotSpeed = 3;
    private Vector3[][] _edges;
    private ObjectCreator _objectCreator;

	void Start ()
	{
        _objectCreator = new ObjectCreator();

        InitEdges();
		CreateShape();
		CreateLetters();        
        CreateDots();
        CreateLevel();
		transform.Rotate(-8, -12, 0.5f);
	}
	
	private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * _rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * _rotSpeed * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }

    private void CreateLevel()
    {
        CreateDot(-1.5f, -2, 0.6f );
        CreateLetter(-0.53f, -0.53f, 0.2f, "T");
        
        CreateDot(1.5f, 1, 1.5f);
        CreateLetter(0.53f, 0.28f, 0.5f, "V");

        CreateDot(1.5f, -1.3f, -1.5f);
        CreateLetter(0.53f, -0.3f, -0.5f, "U");
    }

    private void CreateShape() 
	{
	    foreach (var list in _edges)
	    {
	        CreateLine(list[0], list[1]);
	    }
	}

    private void CreateDots()
    {
        CreateDot(-1.5f, 2f, 1.5f);
        CreateDot(-1.5f, 2f, -1.5f);
        CreateDot(-1.5f, -2f, 1.5f);
        CreateDot(-1.5f, -2f, -1.5f);

        CreateDot(1.5f, 2f, 1.5f);
        CreateDot(1.5f, 2f, -1.5f);
        CreateDot(1.5f, -2f, 1.5f);
        CreateDot(1.5f, -2f, -1.5f);
    }

    private void CreateLetters() 
	{
		CreateLetter(-0.53f, 0.59f, -0.5f, "A1");
		CreateLetter(0.5f, 0.59f, -0.5f, "B1");
		CreateLetter(-0.53f, -0.53f, -0.5f, "C1");
		CreateLetter(0.5f, -0.53f, -0.5f, "D1");

		CreateLetter(-0.53f, 0.59f, 0.5f, "A2");
		CreateLetter(0.5f, 0.59f, 0.5f, "B2");
		CreateLetter(-0.53f, -0.53f, 0.5f, "C2");
		CreateLetter(0.5f, -0.53f, 0.5f, "D2");
	}

    private void CreateDot(float x, float y, float z)
    {
        GameObject sphere = _objectCreator.CreateSphereObject(new Color(0.3f, 0.3f, 0.3f));

        sphere.name = "Dot";
        sphere.tag = "Dot";
        sphere.transform.SetParent(transform);
        sphere.transform.position = new Vector3(x, y, z);
        sphere.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
    }

    private void CreateLine(Vector3 start, Vector3 end)
    {
        var line = new GameObject();

        line.transform.SetParent(transform);
        var scriptableObject = line.AddComponent<LineBehaviour>();
        scriptableObject.InstantiateFields(start, end, Color.gray, false);

        line.tag = "Edge";
        line.name = "Line";
        
        scriptableObject.AddColliderToLineObject();
        line.transform.GetChild(0).tag = "Edge";
    }

	private void CreateLetter(float x, float y, float z, string letter)
	{
		GameObject text = _objectCreator.CreateTextObject(letter, Color.black);
        text.name = "Letter" + letter;
        text.transform.SetParent(transform);
        text.transform.localPosition += new Vector3(x, y, z);
	}
    
    private void InitEdges()
    {
        _edges = new Vector3[][]
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
