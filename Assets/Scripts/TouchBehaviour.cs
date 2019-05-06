using UnityEngine;

public class TouchBehaviour : MonoBehaviour
{
    void Update()
    {
        // if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
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
                        if (CreateButtonsHelper.Action == ActionType.Dot)
                        {
                            GameObject parentObject = gameObject.transform.parent.gameObject;

                            CreateButtonsHelper.SelectedObjects.Add(parentObject);

                            var renderer = parentObject.GetComponent<Renderer>();
                            renderer.material.color = Color.magenta;
                        }
                    }

                    if (gameObject.tag == "Dot")
                    {
                        if (CreateButtonsHelper.Action == ActionType.Line)
                        {
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
