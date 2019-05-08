using UnityEngine;

public class CreatePerpendecularLineBehaviour : MonoBehaviour
{
    private ObjectCreator _objectCreator;

    void Start ()
    {
        _objectCreator = new ObjectCreator();

		 CreateLine(transform.position.x -0.27f, transform.position.y -0.14f, 0,
		 transform.position.x + 0.27f, transform.position.y -0.14f, 0);
		 CreateLine(transform.position.x, transform.position.y -0.14f, 0,
		 transform.position.x, transform.position.y + 0.2f, 0);
	}

    private void CreateLine(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        _objectCreator.CreateLineObjectForButtons(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2), transform, Color.white);
    }
}
