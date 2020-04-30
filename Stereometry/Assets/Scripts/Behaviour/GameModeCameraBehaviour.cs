using Assets.Scripts.Constants;
using UnityEngine;

public class GameModeCameraBehaviour : MonoBehaviour
{
    private const float ZoomOutMin = 1;
    private const float ZoomOutMax = 8;

    private void Start()
    {
        Game.IsAR = false;
        if (Game.MainShape == null)
        {
            GameObject mainShape =
                Instantiate(Resources.Load<GameObject>(PathConstants.PathToPrefabs + Game.CurrentLevelData.Type));
            if (mainShape != null)
            {
                mainShape.name = Game.CurrentLevelData.Type.ToString();
                Game.Actions.Clear();
                CreateNewConstructionHelper.Action = ActionType.None;
                CreateNewConstructionHelper.SelectedObjects.Clear();
            }

            Game.MainShape = mainShape;
        }
        else
        {
            Game.MainShape.transform.position = new Vector3();
            Game.MainShape.transform.rotation = new Quaternion();
            Game.MainShape.transform.GetChild(0).rotation = new Quaternion();
            Game.MainShape.transform.localScale = new Vector3(1, 1, 1);
            foreach (var lineRenderer in Game.MainShape.transform.GetComponentsInChildren<LineRenderer>())
            {
                lineRenderer.startWidth = 0.03f;
                lineRenderer.endWidth = 0.03f;
            }

            Game.MainShape.transform.GetChild(0).gameObject.AddComponent<MainShapeBehaviour>();

            Destroy(Game.MainShape.transform.GetChild(0).gameObject.GetComponent<PlacementObject>());
            CreateNewConstructionHelper.Action = ActionType.None;
            CreateNewConstructionHelper.SelectedObjects.Clear();
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
                GameObject hitObject = hit.collider.transform.gameObject;

                switch (hitObject.tag)
                {
                    case Tags.Edge:
                    {
                        if (CreateNewConstructionHelper.Action == ActionType.Dot ||
                            CreateNewConstructionHelper.Action == ActionType.ParallelLine)
                        {
                            if (CreateNewConstructionHelper.Action == ActionType.ParallelLine &&
                                CreateNewConstructionHelper.SelectedObjects.Count == 1 &&
                                CreateNewConstructionHelper.SelectedObjects[0].CompareTag(Tags.Edge))
                            {
                                return;
                            }

                            GameObject parentObject = hitObject.transform.parent.gameObject;

                            CreateNewConstructionHelper.SelectedObjects.Add(parentObject);
                            var renderer = parentObject.GetComponent<Renderer>();
                            renderer.material.color = Color.magenta;

                            if (CreateNewConstructionHelper.SelectedObjects.Count == 2)
                            {
                                CreateNewConstructionHelper.CreateObject();
                            }
                        }

                        break;
                    }

                    case Tags.Dot:
                    {
                        if (CreateNewConstructionHelper.Action == ActionType.Line
                            || CreateNewConstructionHelper.Action == ActionType.ParallelLine
                            || CreateNewConstructionHelper.Action == ActionType.CrossSection)
                        {
                            if (CreateNewConstructionHelper.Action == ActionType.ParallelLine &&
                                CreateNewConstructionHelper.SelectedObjects.Count == 1 &&
                                CreateNewConstructionHelper.SelectedObjects[0].CompareTag(Tags.Dot))
                            {
                                return;
                            }

                            CreateNewConstructionHelper.SelectedObjects.Add(hitObject);

                            var renderer = hitObject.GetComponent<Renderer>();
                            renderer.material.color = Color.magenta;

                            if (CreateNewConstructionHelper.SelectedObjects.Count == 2 &&
                                CreateNewConstructionHelper.Action != ActionType.CrossSection)
                            {
                                CreateNewConstructionHelper.CreateObject();
                            }
                        }

                        break;
                    }
                }
            }
        }
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, ZoomOutMin, ZoomOutMax);
    }
}
