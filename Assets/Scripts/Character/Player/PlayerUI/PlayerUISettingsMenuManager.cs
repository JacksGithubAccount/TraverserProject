using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUISettingsMenuManager : PlayerUIMenu
    {
        public override void OpenMenu()
        {
            base.OpenMenu();

            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //LoadStatusInformation();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
        }
    }
}
