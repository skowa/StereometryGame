using UnityEngine;

public class ProfileButtons : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject buttons;

    void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "Settings":
                settingsScreen.SetActive(true);
                buttons.SetActive(false);

                break;
            default:
                break;
        }
    }
}
