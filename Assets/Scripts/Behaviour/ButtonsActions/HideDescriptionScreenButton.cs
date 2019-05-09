using System.Linq;
using UnityEngine;

public class HideDescriptionScreenButton : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        transform.parent.gameObject.SetActive(false);

        Resources.FindObjectsOfTypeAll<GameObject>().First(go => go.name == Game.CurrentLevelData.Type.ToString()).SetActive(true);
    }
}
