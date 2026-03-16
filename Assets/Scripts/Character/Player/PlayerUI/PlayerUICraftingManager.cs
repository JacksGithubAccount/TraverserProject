using UnityEngine;

namespace TraverserProject
{
    public class PlayerUICraftingManager : PlayerUIMenu
    {
        

        public override void OpenMenu()
        {
            base.OpenMenu();

            CheckForUnlockedRecipes();

        }

        private void CheckForUnlockedRecipes()
        {
            
        }

        public void CraftSelectedItem()
        {

        }
    }
}
