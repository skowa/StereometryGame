using UnityEngine;

public class GameModeCameraBehaviour : MonoBehaviour
{
    private void Start()
    {
        GameObject gameObject = Instantiate(Resources.Load<GameObject>(Game.PathToPrefabs + Game.CurrentLevelData.Type));
        if (gameObject != null)
        {
            gameObject.name = Game.CurrentLevelData.Type.ToString();
            Game.Actions.Clear();
            CreateButtonsHelper.Action = ActionType.None;
            CreateButtonsHelper.SelectedObjects.Clear();
        }
    }

    void Update()
    {
        //if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
                            if (CreateButtonsHelper.Action == ActionType.ParallelLine 
                                && CreateButtonsHelper.SelectedObjects.Count == 1 && CreateButtonsHelper.SelectedObjects[0].tag == "Edge")
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

                            var renderer = gameObject.GetComponent<Renderer>();
                            renderer.material.color = Color.magenta;
                        }
                    }
                }
            }
        }
    }
}
