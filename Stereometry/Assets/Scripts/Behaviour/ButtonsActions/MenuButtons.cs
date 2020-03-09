using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
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
            case "Profile":
                SceneManager.LoadScene("Profile");
                break;
            default:
                break;
        }
    }
}
