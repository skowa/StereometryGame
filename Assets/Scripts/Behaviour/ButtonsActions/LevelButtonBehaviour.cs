using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonBehaviour : MonoBehaviour
{
    public Sprite number;
    public Sprite greyNumber;
    private int currentLevel;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level");

        int levelNumber = int.Parse(gameObject.name.Substring(5));
        var spriteWrapper = new GameObject { name = levelNumber.ToString() };
        spriteWrapper.transform.SetParent(transform);

        var spriteRenderer = spriteWrapper.AddComponent<SpriteRenderer>();
        if (levelNumber <= currentLevel)
        {
            spriteRenderer.sprite = number;
        }
        else
        {
            spriteRenderer.sprite = greyNumber;
        }

        spriteWrapper.transform.localScale = new Vector3(0.23f, 0.23f);
        spriteWrapper.transform.localPosition = Vector3.zero;
    }

    private void OnMouseUpAsButton()
    {
        int levelNumber;
        if (int.TryParse(gameObject.name.Substring(5), out levelNumber))
        {
            if (levelNumber <= currentLevel && levelNumber <= Game.MaxLevel)
            {
                Game.FillLevelData(levelNumber);
                SceneManager.LoadScene("Game");
            }
        }
    }
}
