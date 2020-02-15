using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private Vector3 offset = new Vector3(0, 0.5f, 0);

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
                    PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    if (placementObject != null)
                    {
                        onTouchHold = true;
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
	            placedObject = Instantiate(Resources.Load<GameObject>($"{Game.PathToPrefabs}AR{Game.CurrentLevelData.Type}"), hitPose.position + offset, hitPose.rotation);
	            if (gameObject != null)
	            {
		            gameObject.name = Game.CurrentLevelData.Type.ToString();
		            Game.Actions.Clear();
		            CreateButtonsHelper.Action = ActionType.None;
		            CreateButtonsHelper.SelectedObjects.Clear();
	            }
              //  placedObject = Instantiate(placedPrefab, hitPose.position + offset, hitPose.rotation);
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
