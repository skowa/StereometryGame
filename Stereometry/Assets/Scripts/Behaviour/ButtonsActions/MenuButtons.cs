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
            case GameObjectNames.MainScene.PlayButton:
                int currentLevel = PlayerPrefs.GetInt(PlayerPrefsConstants.LevelPref);
                if (currentLevel > Game.MaxLevel)
                {
                    return;
                }

                Game.FillLevelData(currentLevel);
                SceneManager.LoadScene(Scenes.Game);
                break;
            case GameObjectNames.MainScene.LevelsButton:
                SceneManager.LoadScene(Scenes.Levels);
                break;
            case GameObjectNames.MainScene.ProfileButton:
                SceneManager.LoadScene(Scenes.Profile);
                break;
        }
    }
}
