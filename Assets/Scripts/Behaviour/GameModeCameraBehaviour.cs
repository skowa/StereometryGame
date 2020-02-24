using UnityEngine;

public class GameModeCameraBehaviour : MonoBehaviour
{
	private float _zoomOutMin = 1;
	private float _zoomOutMax = 8;

	private void Start()
	{
		Game.IsAR = false;
		if (Game.MainShape == null)
		{
			GameObject mainShape = Instantiate(Resources.Load<GameObject>(Game.PathToPrefabs + Game.CurrentLevelData.Type));
			if (mainShape != null)
			{
				mainShape.name = Game.CurrentLevelData.Type.ToString();
				Game.Actions.Clear();
				CreateButtonsHelper.Action = ActionType.None;
				CreateButtonsHelper.SelectedObjects.Clear();
			}
		}
		else
		{
			Game.MainShape.transform.position = new Vector3();
			Game.MainShape.transform.rotation = new Quaternion();
			Game.MainShape.transform.GetChild(0).rotation = new Quaternion();
			Game.MainShape.transform.localScale = new Vector3(1, 1, 1);
			foreach (var lineRenderer in Game.MainShape.transform.GetComponentsInChildren<LineRenderer>())
			{
				lineRenderer.widthMultiplier = 1;
			}

			Game.MainShape.transform.GetChild(0).gameObject.AddComponent<MainShapeBehaviour>();

			Destroy(Game.MainShape.transform.GetChild(0).gameObject.GetComponent<PlacementObject>());
			CreateButtonsHelper.Action = ActionType.None;
			CreateButtonsHelper.SelectedObjects.Clear();
		}
	}

	private void Update()
	{
		if (Input.touchCount == 2)
		{
			ProcessPinch();
		}
		else if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			ProcessSingleTouch();
		}
	}

	private void ProcessPinch()
	{
		Touch touch0 = Input.GetTouch(0);
		Touch touch1 = Input.GetTouch(1);

		Vector2 prevPosition0 = touch0.position - touch0.deltaPosition;
		Vector2 prevPosition1 = touch1.position - touch1.deltaPosition;

		float prevMagnitude = (prevPosition0 - prevPosition1).magnitude;
		float currentMagnitude = (touch0.position - touch1.position).magnitude;

		float difference = currentMagnitude - prevMagnitude;

		Zoom(difference * 0.01f);
	}

	private void ProcessSingleTouch()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 100.0f))
		{
			if (hit.collider.transform != null)
			{
				GameObject gameObject = hit.collider.transform.gameObject;

				if (gameObject.tag == "Edge")
				{
					if (CreateButtonsHelper.Action == ActionType.Dot || CreateButtonsHelper.Action == ActionType.ParallelLine)
					{
						if (CreateButtonsHelper.Action == ActionType.ParallelLine && CreateButtonsHelper.SelectedObjects.Count == 1 &&
						    CreateButtonsHelper.SelectedObjects[0].tag == "Edge")
						{
							return;
						}

						GameObject parentObject = gameObject.transform.parent.gameObject;

						CreateButtonsHelper.SelectedObjects.Add(parentObject);

						var renderer = parentObject.GetComponent<Renderer>();
						renderer.material.color = Color.magenta;
					}
				}

				if (gameObject.tag == "Dot")
				{
					if (CreateButtonsHelper.Action == ActionType.Line || CreateButtonsHelper.Action == ActionType.Check
					                                                  || CreateButtonsHelper.Action == ActionType.ParallelLine)
					{
						if (CreateButtonsHelper.Action == ActionType.ParallelLine && CreateButtonsHelper.SelectedObjects.Count == 1 &&
						    CreateButtonsHelper.SelectedObjects[0].tag == "Dot")
						{
							return;
						}

						CreateButtonsHelper.SelectedObjects.Add(gameObject);

						var renderer = gameObject.GetComponent<Renderer>();
						renderer.material.color = Color.magenta;
					}
				}
			}
		}
	}

	private void Zoom(float increment)
	{
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomOutMin, _zoomOutMax);
	}
}
