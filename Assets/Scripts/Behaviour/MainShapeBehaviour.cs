using UnityEngine;

public class MainShapeBehaviour : MonoBehaviour
{
    private float _rotSpeed = 30;
    private ObjectCreator _objectCreator;
    Vector2 _lastAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

    void Start ()
	{
        _objectCreator = new ObjectCreator();

        CreateLevel();
		transform.Rotate(-8, -12, 0.5f);
	}

    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * _rotSpeed * Mathf.Deg2Rad;//-(_lastAxis.x - Input.mousePosition.x) * _rotSpeed * Mathf.Deg2Rad;//
       // float rotX = -(_lastAxis.x - Input.mousePosition.x) * _rotSpeed * Mathf.Deg2Rad;//
        float rotY = Input.GetAxis("Mouse Y") * _rotSpeed * Mathf.Deg2Rad;//-(_lastAxis.y - Input.mousePosition.y) * _rotSpeed * Mathf.Deg2Rad;//
        //float rotY = -(_lastAxis.y - Input.mousePosition.y) * _rotSpeed * Mathf.Deg2Rad;//
        _lastAxis = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);
    }

    private void CreateLevel()
    {
        foreach (Line line in Game.CurrentLevelData.Lines)
        {
            _objectCreator.CreateLine(line, transform, Resources.Load<Material>(Game.PathToLinesMaterial));
        }

        foreach (SerializableVector3 vertice in Game.CurrentLevelData.Vertices)
        {
            _objectCreator.CreateDot(vertice, transform, Resources.Load<Material>(Game.PathToDotsMaterial));
        }

        foreach (Letter letter in Game.CurrentLevelData.Letters)
        {
            _objectCreator.CreateLetter(letter, transform, Color.black);
        }
    }
}
