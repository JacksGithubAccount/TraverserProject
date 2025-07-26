using UnityEngine;

namespace TraverserProject
{

    public class PlayerUIToggleHud : MonoBehaviour
    {

        private void OnEnable()
        {
            PlayerUIManager.Singleton.playerUIHudManager.ToggleHUD(false);
        }

    private void OnDisable()
        {
            PlayerUIManager.Singleton.playerUIHudManager.ToggleHUD(true);
        }

    }
}