using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject settingsScreen;
    public GameObject cube;

    void OnMouseUpAsButton()
    {
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }

        switch (gameObject.name)
        {
            case "play":
                int currentLevel = PlayerPrefs.GetInt("level");
                if (currentLevel > Game.MaxLevel)
                {
                    return;
                }

                Game.FillLevelData(currentLevel);
                SceneManager.LoadScene("Game");
                break;
            case "button":
                SceneManager.LoadScene("Levels");
                break;
            case "Settings":
                settingsScreen.SetActive(true);
                cube.SetActive(false);
                transform.parent.gameObject.SetActive(false);

                break;
            default:
                break;
        }
    }
}
