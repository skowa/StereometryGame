using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeButtons : MonoBehaviour
{
    public GameObject _description;

    void OnMouseUpAsButton()
    {
        switch (gameObject.name)
        {
            case "back":
            case "rightAnswer":
                SceneManager.LoadScene("Levels");

                break;
            case "toDo":
                var shape = GameObject.Find(Game.CurrentLevelData.Type.ToString());
                shape.SetActive(false);

                _description.SetActive(true);
                GameObject text = _description.transform.GetChild(1).gameObject;
                text.GetComponent<Text>().text = Game.CurrentLevelData.Description;

                break;
            case "onceAgain":
                CreateButtonsHelper.Action = ActionType.None;
                foreach (GameObject t in Game.Actions)
                {
                    Destroy(t);
                }

                Game.Actions.Clear();

                break;
            case "cancel":
                if (Game.Actions.Count >= 1)
                {
                    GameObject toBeRemoved = Game.Actions[Game.Actions.Count - 1];
                    Game.Actions.Remove(toBeRemoved);
                    Destroy(toBeRemoved);
                }

                break;
        }
    }
}
