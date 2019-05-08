using UnityEngine;
using UnityEngine.UI;

public class LevelNameBehaviour : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Text>().text += Game.CurrentLevelData.Number;
    }
}
