using UnityEngine;

public class SettingsScreenButtons : MonoBehaviour
{
    public GameObject buttons;

    void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "ResetGame":
                PlayerPrefs.SetInt("level", 1);
                break;
            case "OK":
                buttons.SetActive(true);
                transform.parent.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }
}
