using UnityEngine;

namespace TraverserProject
{

    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    {
        public void OpenTeleportLocationMenu()
        {
            CloseMenuForOpeningAnotherMenu();
            PlayerUIManager.Singleton.playerUITeleportLocationManager.OpenMenu();
        }
        public void OpenLevelUpMenu()
        {
            CloseMenuForOpeningAnotherMenu();
            PlayerUIManager.Singleton.playerUILevelUpManager.OpenMenu();
        }

        public void OpenStorageMenu()
        {
            CloseMenuForOpeningAnotherMenu();
            PlayerUIManager.Singleton.playerUIStorageManager.OpenMenu();
        }
    }
}