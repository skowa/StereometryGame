using UnityEngine;

public class CreateButtonBehaviour : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        CreateButtonsHelper.CreateObject();
    }
}
