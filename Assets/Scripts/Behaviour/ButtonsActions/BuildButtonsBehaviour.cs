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
            case "check":
                CreateButtonsHelper.Action = ActionType.Check;
                break;
        }

        var okButton = GameObject.Find("OK");
        okButton.GetComponent<Renderer>().enabled = true;
        okButton.GetComponent<Collider2D>().enabled = true;
    }
}
