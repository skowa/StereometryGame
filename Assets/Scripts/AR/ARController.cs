using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class ARController : MonoBehaviour
{
	[SerializeField] private Camera _arCamera;

	private GameObject _placedObject;

	private Vector2 _touchPosition;

	private ARRaycastManager _arRaycastManager;

	private bool _onTouchHold;

	private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

	private readonly Vector3 _offset = new Vector3(0, 0.35f, 0);

	private void Start()
	{
		Game.IsAR = true;
	}

	private void Awake()
	{
		_arRaycastManager = GetComponent<ARRaycastManager>();
	}

	private void Update()
	{
		ProcessTouchOnShapeComponents();
		ProcessShapeMovement();
	}

	private void ProcessTouchOnShapeComponents()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			_touchPosition = touch.position;

			if (touch.phase == TouchPhase.Began)
			{
				Ray ray = _arCamera.ScreenPointToRay(touch.position);
				if (Physics.Raycast(ray, out RaycastHit hitObject))
				{
					switch (hitObject.transform.tag)
					{
						case "Edge":
							{
								if (CreateButtonsHelper.Action == ActionType.Dot || CreateButtonsHelper.Action == ActionType.ParallelLine)
								{
									if (CreateButtonsHelper.Action == ActionType.ParallelLine && CreateButtonsHelper.SelectedObjects.Count == 1 &&
										CreateButtonsHelper.SelectedObjects[0].tag == "Edge")
									{
										return;
									}

									GameObject parentObject = hitObject.transform.parent.gameObject;

									CreateButtonsHelper.SelectedObjects.Add(parentObject);

									var renderer = parentObject.GetComponent<Renderer>();
									renderer.material.color = Color.magenta;
								}

								break;
							}

						case "Dot":
							{
								if (CreateButtonsHelper.Action == ActionType.Line || CreateButtonsHelper.Action == ActionType.Check ||
								    CreateButtonsHelper.Action == ActionType.ParallelLine)
								{
									if (CreateButtonsHelper.Action == ActionType.ParallelLine && CreateButtonsHelper.SelectedObjects.Count == 1 &&
									    CreateButtonsHelper.SelectedObjects[0].tag == "Dot")
									{
										return;
									}

									CreateButtonsHelper.SelectedObjects.Add(hitObject.transform.gameObject);

									var hitObjectRenderer = hitObject.transform.GetComponent<Renderer>();
									hitObjectRenderer.material.color = Color.magenta;
								}

								break;
							}

						default:
							{
								PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
								if (placementObject != null)
								{
									_onTouchHold = true;
								}

								break;
							}
					}
				}
			}

			if (touch.phase == TouchPhase.Ended)
			{
				_onTouchHold = false;
			}
		}
	}

	private void ProcessShapeMovement()
	{
		if (_arRaycastManager.Raycast(_touchPosition, Hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
		{
			Pose hitPose = Hits[0].pose;
			if (_placedObject == null)
			{
				_placedObject = Game.MainShape;

				if (_placedObject != null)
				{
					_placedObject.SetActive(true);

					var placedObjectShape = _placedObject.transform.GetChild(0).gameObject;
					placedObjectShape.transform.position = hitPose.position + _offset;
					placedObjectShape.transform.rotation = hitPose.rotation;
					_placedObject.transform.localScale = new Vector3(0.16f, 0.1875f, 0.16f);
					placedObjectShape.AddComponent<PlacementObject>();
					foreach (var lineRenderer in placedObjectShape.transform.GetComponentsInChildren<LineRenderer>())
					{
						lineRenderer.widthMultiplier = 0.3f;
					}

					Destroy(placedObjectShape.GetComponent<MainShapeBehaviour>());
					CreateButtonsHelper.Action = ActionType.None;
					CreateButtonsHelper.SelectedObjects.Clear();
				}
			}
			else
			{
				if (_onTouchHold)
				{
					_placedObject.transform.position = hitPose.position + _offset;
					_placedObject.transform.rotation = hitPose.rotation;
				}
			}
		}
	}
}