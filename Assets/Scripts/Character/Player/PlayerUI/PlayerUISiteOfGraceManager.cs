using UnityEngine;

namespace TraverserProject
{

    public class PlayerUISiteOfGraceManager : PlayerUIMenu
    {
        public void OpenTeleportLocationMenu()
        {
            CloseMenu();
            PlayerUIManager.Singleton.playerUITeleportLocationManager.OpenMenu();
        }
        public void OpenLevelUpMenu()
        {
            CloseMenu();
            PlayerUIManager.Singleton.playerUILevelUpManager.OpenMenu();
        }
    }
}