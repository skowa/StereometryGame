using UnityEngine;

public class MainShapeBehaviour : MonoBehaviour
{
    private float _rotationSpeed = 0.1f;
    private ObjectCreator _objectCreator;
    private GameObject _playerCamera;
    private GameObject[] _letters;
    private Rect _rotationRect;

    void Start ()
	{
		_rotationRect = new Rect(0, 360, Screen.width, Screen.height - 920);
        _objectCreator = new ObjectCreator();

        if (!Game.IsLevelFilled)
        {
	        CreateLevel();
	        Game.IsLevelFilled = true;
        }

		transform.Rotate(-8, -12, 0.5f);
		_playerCamera = GameObject.Find("Main Camera");
		_letters = GameObject.FindGameObjectsWithTag("Letter");
	}

    private void Update()
    {
	    if (Input.touchCount == 1)
	    {
		    Touch touch = Input.GetTouch(0);
		    if (_rotationRect.Contains(touch.position) && touch.phase == TouchPhase.Moved)
		    {
                transform.RotateAround(Vector3.up, -touch.deltaPosition.x * _rotationSpeed * Mathf.Deg2Rad);
                transform.RotateAround(Vector3.right, touch.deltaPosition.y * _rotationSpeed * Mathf.Deg2Rad);
		    }
	    }

        foreach (var letter in _letters)
        {
		    letter.transform.rotation = new Quaternion(0.0f, _playerCamera.transform.rotation.y, 0.0f, _playerCamera.transform.rotation.w);
        }
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
