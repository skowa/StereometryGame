using UnityEngine;

public class LevelScreenBackButton : MonoBehaviour
{

    void OnMouseUpAsButton()
    {
        Application.LoadLevel("Main");
    }
}
