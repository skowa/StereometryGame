using UnityEngine;

public class GameModeButtons : MonoBehaviour {

    void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "back":
                Application.LoadLevel("Levels");
                break;
        }
    }
}
