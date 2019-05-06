using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScreenBackButton : MonoBehaviour
{

    void OnMouseUpAsButton()
    {
        Application.LoadLevel("Main");
    }
}
