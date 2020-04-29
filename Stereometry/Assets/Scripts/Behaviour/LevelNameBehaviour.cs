using UnityEngine;
using UnityEngine.UI;

public class LevelNameBehaviour : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Text>().text += Game.CurrentLevelData.Number;
    }
}
