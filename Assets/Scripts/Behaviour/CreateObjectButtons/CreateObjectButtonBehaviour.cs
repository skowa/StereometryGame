using UnityEngine;

public class CreateObjectButtonBehaviour : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        CreateButtonsHelper.CreateObject();
    }
}
