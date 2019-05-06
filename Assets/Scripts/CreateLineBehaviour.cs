using UnityEngine;

public class CreateLineBehaviour : MonoBehaviour {
    private ObjectCreator _objectCreator;

    void Start () {
        _objectCreator = new ObjectCreator();

		CreateButtonLine(transform.position.x -0.3f, transform.position.y -0.3f, 0,
		 transform.position.x + 0.3f, transform.position.y + 0.3f, 0);
	}

    private void OnMouseUpAsButton()
    {
        CreateButtonsHelper.Action = ActionType.Line;

        var okButton = GameObject.Find("OK");
        okButton.GetComponent<Renderer>().enabled = true;
        okButton.GetComponent<Collider2D>().enabled = true;
    }

    private void CreateButtonLine(float x1, float y1, float z1, float x2, float y2, float z2)
	{
		GameObject line = _objectCreator.CreateLineObject(new Vector3(x1, y1, z1), new Vector3(x2, y2, z2), Color.white);
        line.transform.SetParent(transform);
	}
}