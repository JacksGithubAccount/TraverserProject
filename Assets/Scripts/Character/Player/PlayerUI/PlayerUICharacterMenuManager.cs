using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class PlayerUICharacterMenuManager : PlayerUIMenu
    {
        public override void CloseMenu()
        {
            base.CloseMenu();
            //PlayerUIManager.Singleton.openMenus.Pop();
        }
    }
}