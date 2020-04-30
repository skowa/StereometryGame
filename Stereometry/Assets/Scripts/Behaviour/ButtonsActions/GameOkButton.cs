using Assets.Scripts.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviour.ButtonsActions
{
    public static class GameOkButton
    {
        public static void Enable(bool enable)
        {
            var okButton = GameObject.Find(GameObjectNames.GameScene.OkButton);
            okButton.GetComponent<Image>().enabled = enable;
            okButton.GetComponent<Collider2D>().enabled = enable;
        }
    }
}