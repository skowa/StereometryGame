using UnityEditor;
using UnityEngine;

public class sample : MonoBehaviour
{
    public float height = 1.4f;
    public float width = 2;
    public float length = 2;
    private float _rotSpeed = 3;

    void Start()
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var mesh = new Mesh();

        var widthOffset = width * 0.5f;
        var lengthOffset = length * 0.5f;

        var points = new Vector3[] {
            new Vector3(-widthOffset, -height / 2, -lengthOffset),
            new Vector3(widthOffset, -height / 2, -lengthOffset),
            new Vector3(widthOffset, -height / 2, lengthOffset),
            new Vector3(0.2f, height / 2, 0.3f)
        };

        mesh.vertices = new Vector3[] {
            points[0], points[1], points[2],
            points[0], points[1], points[3],
            points[0], points[2], points[3],
            points[1], points[2], points[3]
        };

        mesh.triangles = new int[] {
            0, 1, 2,
            5, 4, 3,
            6, 7, 8,
            11, 10, 9
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        gameObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Transparent");
        gameObject.transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(3f, 4, 3f);
        gameObject.AddComponent<MeshCollider>();


        InitEdges();
        CreateShape();
        CreateLetters();
        ObjectCreator _objectCreator = new ObjectCreator();
        _objectCreator.CreateDot(new Vector3(-1.8f, -2f, -1.8f), transform, Resources.Load<Material>(Game.PathToDotsMaterial));
        _objectCreator.CreateDot(new Vector3(1.8f, -2f, -1.8f), transform, Resources.Load<Material>(Game.PathToDotsMaterial));
        _objectCreator.CreateDot(new Vector3(1.8f, -2f, 1.8f), transform, Resources.Load<Material>(Game.PathToDotsMaterial));
        _objectCreator.CreateDot(new Vector3(0.6f, 2f, 0.9f), transform, Resources.Load<Material>(Game.PathToDotsMaterial));
        transform.Rotate(-8, -12, 0.5f);
    }

    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * _rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * _rotSpeed * Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }

    private Vector3[][] edges;

    private void CreateShape()
    {
        foreach (var list in edges)
        {
            CreateLine(list[0], list[1]);
        }
    }

    private void CreateLetters()
    {
        CreateText(-0.67f, -0.5f, -0.62f, "A");
        CreateText(0.63f, -0.5f, -0.62f, "B");
        CreateText(0.63f, -0.5f, 0.62f, "C");
        CreateText(0.17f, 0.6f, 0.33f, "D");
    }

    private void CreateLine(Vector3 first, Vector3 second)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        line.AddComponent<LineRenderer>();
        line.name = "Line";
        line.tag = "Edge";

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>(Game.PathToLinesMaterial);
        lineRenderer.positionCount = 2;
        lineRenderer.SetWidth(0.03f, 0.03f);
        lineRenderer.SetPosition(0, first);
        lineRenderer.SetPosition(1, second);
        lineRenderer.useWorldSpace = false;
        addColliderToLine(line, first, second);
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

        text.name = "Letter" + letter;
    }

    private void addColliderToLine(GameObject line, Vector3 start, Vector3 end)
    {
        GameObject colliderWrapper = new GameObject();
        colliderWrapper.transform.SetParent(line.transform);
        colliderWrapper.tag = "Edge";

        CapsuleCollider collider = colliderWrapper.AddComponent<CapsuleCollider>();
        collider.radius = 0.04f;
        collider.center = Vector3.zero;
        collider.direction = 2;

        collider.transform.position = start + (end - start) / 2;
        collider.transform.LookAt(start);
        collider.height = (end - start).magnitude;
    }

    private void InitEdges()
    {
        edges = new Vector3[][]
        {
            new []
            {
                new Vector3(-1.8f, -2f, -1.8f), new Vector3(1.8f, -2f, -1.8f)
            },
            new []
            {
                new Vector3(-1.8f, -2f, -1.8f), new Vector3(1.8f, -2f, 1.8f)
            },
            new []
            {
                new Vector3(-1.8f, -2f, -1.8f), new Vector3(0.6f, 2f, 0.9f)
            },
            new []
            {
                new Vector3(1.8f, -2f, -1.8f), new Vector3(1.8f, -2f, 1.8f)
            },

            new []
            {
                new Vector3(1.8f, -2f, -1.8f), new Vector3(0.6f, 2f, 0.9f)
            },
            new []
            {
                new Vector3(1.8f, -2f, 1.8f), new Vector3(0.6f, 2f, 0.9f)
            }
        };
    }
}
