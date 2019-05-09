using UnityEngine;

public class BuildButtonsBehaviour : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "dot":
                CreateButtonsHelper.Action = ActionType.Dot;
                break;
            case "line":
                CreateButtonsHelper.Action = ActionType.Line;
                break;
            case "parallelLine":
                CreateButtonsHelper.Action = ActionType.ParallelLine;
                break;
            case "rotate":
                var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());

                Quaternion rotation = shape.transform.rotation;
                shape.transform.localRotation = Quaternion.identity;
                shape.transform.Rotate(-8, -12, 0.5f);

                break;
            case "check":
                CreateButtonsHelper.Action = ActionType.Check;
                break;
        }

        if (gameObject.name != "rotate")
        {
            var okButton = GameObject.Find("OK");
            okButton.GetComponent<Renderer>().enabled = true;
            okButton.GetComponent<Collider2D>().enabled = true;
        }
    }
}
