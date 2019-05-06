using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeButtons : MonoBehaviour {

    void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "back":
                Application.LoadLevel("Game");
                break;
        }
    }
}
