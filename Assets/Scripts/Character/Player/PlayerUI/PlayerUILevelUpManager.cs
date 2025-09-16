using UnityEngine;

namespace TraverserProject
{

    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        public override void CloseMenu()
        {
            base.CloseMenu();
            CloseMenuAfterFixedFrame();
        }
    }
}