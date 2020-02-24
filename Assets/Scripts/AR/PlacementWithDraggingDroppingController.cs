using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementWithDraggingDroppingController : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    [SerializeField]
    private Camera arCamera;

    private GameObject placedObject;

    private Vector2 touchPosition;

    private ARRaycastManager arRaycastManager;

    private bool onTouchHold = false;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private Vector3 offset = new Vector3(0, 0.35f, 0);

    private void Start()
    {
	    Game.IsAR = true;
    }

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
	                if (hitObject.transform.tag == "Edge")
	                {
		                if (CreateButtonsHelper.Action == ActionType.Dot || CreateButtonsHelper.Action == ActionType.ParallelLine)
		                {
			                if (CreateButtonsHelper.Action == ActionType.ParallelLine
			                    && CreateButtonsHelper.SelectedObjects.Count == 1 && CreateButtonsHelper.SelectedObjects[0].tag == "Edge")
			                {
				                return;
			                }

			                GameObject parentObject = hitObject.transform.parent.gameObject;

			                CreateButtonsHelper.SelectedObjects.Add(parentObject);

			                var renderer = parentObject.GetComponent<Renderer>();
			                renderer.material.color = Color.magenta;
		                }
	                }
	                else if (hitObject.transform.tag == "Dot")
	                {
		                if (CreateButtonsHelper.Action == ActionType.Line
		                    || CreateButtonsHelper.Action == ActionType.Check
		                    || CreateButtonsHelper.Action == ActionType.ParallelLine)
		                {
			                if (CreateButtonsHelper.Action == ActionType.ParallelLine
			                    && CreateButtonsHelper.SelectedObjects.Count == 1 && CreateButtonsHelper.SelectedObjects[0].tag == "Dot")
			                {
				                return;
			                }

			                CreateButtonsHelper.SelectedObjects.Add(gameObject);

			                var renderer = hitObject.transform.GetComponent<Renderer>();
			                renderer.material.color = Color.magenta;
		                }
	                }
                    else
	                {
		                PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
		                if (placementObject != null)
		                {
			                onTouchHold = true;
		                }
	                }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                onTouchHold = false;
            }
        }

        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
	        Pose hitPose = hits[0].pose;
            if (placedObject == null)
            {
	            placedObject = Game.MainShape;

                if (placedObject != null)
                {
                    placedObject.SetActive(true);

                    var placedObjectShape = placedObject.transform.GetChild(0).gameObject;
                    placedObjectShape.transform.position = hitPose.position + offset;
                    placedObjectShape.transform.rotation = hitPose.rotation;
                    placedObject.transform.localScale = new Vector3(0.16f, 0.1875f, 0.16f);
                    placedObjectShape.AddComponent<PlacementObject>();
                    foreach (var lineRenderer in placedObjectShape.transform.GetComponentsInChildren<LineRenderer>())
                    {
	                    lineRenderer.widthMultiplier = 0.4f;
                    }

                    Destroy(placedObjectShape.GetComponent<MainShapeBehaviour>());
                    CreateButtonsHelper.Action = ActionType.None;
		            CreateButtonsHelper.SelectedObjects.Clear();
	            }
            }
            else
            {
                if (onTouchHold)
                {
                    placedObject.transform.position = hitPose.position + offset;
                    placedObject.transform.rotation = hitPose.rotation;
                }
            }
        }
    }
}
