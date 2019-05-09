using UnityEngine;

public class SettingsScreenButtons : MonoBehaviour
{
    public GameObject buttons;
    public GameObject cube;

    void OnMouseUpAsButton()
    {
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }

        switch (gameObject.name)
        {
            case "ResetGame":
                PlayerPrefs.SetInt("level", 1);
                break;
            case "OK":
                buttons.SetActive(true);
                cube.SetActive(true);
                transform.parent.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }
}
