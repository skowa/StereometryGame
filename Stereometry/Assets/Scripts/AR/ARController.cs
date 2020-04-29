using System.Collections.Generic;
using Assets.Scripts.Constants;
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

	private Vector3 _offset;

	private readonly float _yAxisMultiplier = 1.171875f;
	private readonly float _minScale = 0.05f;
	private readonly float _maxScale = 1f;

	private void Start()
	{
		_offset = new Vector3(0, 0.35f, 0);
		Game.IsAR = true;
	}

	private float CountWidth()
	{
		float actualWidth = Game.MainShape.transform.GetChild(0).GetComponent<Collider>().bounds.size.y / 2;
		float outScale = Game.MainShape.transform.localScale.y;
		float inScale = Game.MainShape.transform.GetChild(0).localScale.y;

		return actualWidth / outScale / inScale;
	}

	private void Awake()
	{
		_arRaycastManager = GetComponent<ARRaycastManager>();
	}

	private void Update()
	{
		ProcessPinchTouch();
		ProcessTouchOnShapeComponents();
		ProcessShapeMovement();
	}

	private void ProcessPinchTouch()
	{
		if (Input.touchCount == 2)
		{
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);

			Vector2 prevPosition0 = touch0.position - touch0.deltaPosition;
			Vector2 prevPosition1 = touch1.position - touch1.deltaPosition;

			float prevMagnitude = (prevPosition0 - prevPosition1).magnitude;
			float currentMagnitude = (touch0.position - touch1.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			float incrementValue = difference * 0.0005f;
			Vector3 currentScale = Game.MainShape.transform.localScale;
			float xValue = GetScalingValueAccordingToBoundaries(currentScale.x, incrementValue);
			float yValue = GetScalingValueAccordingToBoundaries(currentScale.y, incrementValue, _yAxisMultiplier);

			float zValue = GetScalingValueAccordingToBoundaries(currentScale.z, incrementValue);

			Game.MainShape.transform.localScale = new Vector3(xValue, yValue, zValue);
			foreach (var lineRenderer in Game.MainShape.transform.GetChild(0).GetComponentsInChildren<LineRenderer>())
			{
				lineRenderer.startWidth = 0.0045f;
				lineRenderer.endWidth = 0.0045f;
			}
		}
	}

	private float GetScalingValueAccordingToBoundaries(float currentScale, float scalingValue, float multiplier = 1)
	{
		float newValue = currentScale + scalingValue * multiplier;
		if (newValue > _maxScale * multiplier || newValue < _minScale * multiplier)
		{
			return currentScale;
		}

		return newValue;
	}

	private void ProcessTouchOnShapeComponents()
	{
		if (Input.touchCount == 1)
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
						case Tags.Edge:
							{
								if (CreateNewConstructionHelper.Action == ActionType.Dot || CreateNewConstructionHelper.Action == ActionType.ParallelLine)
								{
									if (CreateNewConstructionHelper.Action == ActionType.ParallelLine && CreateNewConstructionHelper.SelectedObjects.Count == 1 &&
										CreateNewConstructionHelper.SelectedObjects[0].tag == Tags.Edge)
									{
										return;
									}

									GameObject parentObject = hitObject.transform.parent.gameObject;

									CreateNewConstructionHelper.SelectedObjects.Add(parentObject);

                                    var renderer = parentObject.GetComponent<Renderer>();
									renderer.material.color = Color.magenta;
								}

								break;
							}

						case Tags.Dot:
							{
								if (CreateNewConstructionHelper.Action == ActionType.Line || CreateNewConstructionHelper.Action == ActionType.Check ||
									CreateNewConstructionHelper.Action == ActionType.ParallelLine)
								{
									if (CreateNewConstructionHelper.Action == ActionType.ParallelLine && CreateNewConstructionHelper.SelectedObjects.Count == 1 &&
										CreateNewConstructionHelper.SelectedObjects[0].tag == Tags.Dot)
									{
										return;
									}

									CreateNewConstructionHelper.SelectedObjects.Add(hitObject.transform.gameObject);

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
					_placedObject.transform.localPosition = hitPose.position + _offset;
					_placedObject.transform.rotation = new Quaternion();
					placedObjectShape.transform.rotation = new Quaternion();
					_placedObject.transform.localScale = new Vector3(0.16f, 0.1875f, 0.16f);
					placedObjectShape.AddComponent<PlacementObject>();
					foreach (var lineRenderer in placedObjectShape.transform.GetComponentsInChildren<LineRenderer>())
					{
						lineRenderer.startWidth = 0.0045f;
						lineRenderer.endWidth = 0.0045f;
					}

					Destroy(placedObjectShape.GetComponent<MainShapeBehaviour>());
					CreateNewConstructionHelper.Action = ActionType.None;
					CreateNewConstructionHelper.SelectedObjects.Clear();
				}
			}
			else
			{
				if (_onTouchHold)
				{
					_placedObject.transform.localPosition = hitPose.position + _offset;
				}
			}
		}
	}
}