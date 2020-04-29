using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    void OnMouseUpAsButton()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsConstants.LevelPref))
        {
            PlayerPrefs.SetInt(PlayerPrefsConstants.LevelPref, 1);
        }

        switch (gameObject.name)
        {
            case "play":
                int currentLevel = PlayerPrefs.GetInt(PlayerPrefsConstants.LevelPref);
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
