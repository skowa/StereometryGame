using Assets.Scripts.Constants;
using System.Collections.Generic;
using UnityEngine;

public class TriangularPyramidCreator : MonoBehaviour
{
    private Vector3[] _points;
    private List<Line> _edges;
    private Material _dotsMaterial;
    private Material _linesMaterial;

    void Start()
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var mesh = new Mesh();

        _edges = new List<Line>();
        _points = new []
        {
            new Vector3(-1.8f, -2, -1.8f),
            new Vector3(1.8f, -2, -1.8f),
            new Vector3(1.8f, -2, 1.8f),
            new Vector3(0.6f, 2, -1.8f)
        };

        mesh.vertices = new []
        {
            _points[0], _points[1], _points[2],
            _points[0], _points[1], _points[3],
            _points[0], _points[2], _points[3],
            _points[1], _points[2], _points[3]
        };

        mesh.triangles = new []
        {
            0, 1, 2,
            5, 4, 3,
            6, 7, 8,
            11, 10, 9
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        gameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>(PathConstants.PathToTransparentMaterial);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.AddComponent<MeshCollider>();

        _linesMaterial = Resources.Load<Material>(PathConstants.PathToLinesMaterial);

        InitEdges();
        CreateShape();
        CreateLetters();

        _dotsMaterial = Resources.Load<Material>(PathConstants.PathToDotsMaterial);
        var _objectCreator = new ObjectCreator();
        foreach (Vector3 point in _points)
        {
            _objectCreator.CreateDot(point, transform, _dotsMaterial);
        }

        transform.Rotate(-8, -12, 0.5f);
    }

    private void CreateShape()
    {
        foreach (var line in _edges)
        {
            CreateLine(line);
        }
    }

    private void CreateLetters()
    {
        CreateText(-2.01f, -2f, -1.86f, "A");
        CreateText(1.89f, -2f, -1.86f, "B");
        CreateText(1.89f, -2f, 1.86f, "C");
        CreateText(0.51f, 2.4f, 0.99f, "D");
    }

    private void CreateLine(Line edge)
    {
        var line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        line.name = "Line";
        line.tag = Tags.Edge;

        var lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = _linesMaterial;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.SetPosition(0, edge.StartPoint);
        lineRenderer.SetPosition(1, edge.EndPoint);
        lineRenderer.useWorldSpace = false;

        AddColliderToLine(line, edge.StartPoint, edge.EndPoint);
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

        text.name = Tags.Letter + letter;
    }

    private void AddColliderToLine(GameObject line, Vector3 start, Vector3 end)
    {
        var colliderWrapper = new GameObject();
        colliderWrapper.transform.SetParent(line.transform);
        colliderWrapper.tag = Tags.Edge;

        var collider = colliderWrapper.AddComponent<CapsuleCollider>();
        collider.radius = 0.04f;
        collider.center = Vector3.zero;
        collider.direction = 2;

        collider.transform.position = start + (end - start) / 2;
        collider.transform.LookAt(start);
        collider.height = (end - start).magnitude;
    }

    private void InitEdges()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            for (int j = i + 1; j < _points.Length; j++)
            {
                _edges.Add(new Line(_points[i], _points[j]));
            }
        }
    }
}
