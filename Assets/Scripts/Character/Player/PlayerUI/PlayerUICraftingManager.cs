using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUICraftingManager : PlayerUIMenu
    {
        [Header("Recipes Window")]
        [SerializeField] GameObject craftingRecipeWindow;
        [SerializeField] GameObject craftingRecipeSlotPrefab;
        [SerializeField] Transform craftingRecipeContentWindow;
        [SerializeField] Recipe currentlySelectedRecipe;

        public override void OpenMenu()
        {
            base.OpenMenu();

            CheckForUnlockedRecipes();

        }

        private void CheckForUnlockedRecipes()
        {
            for (int s = 0; s < PlayerRecipeManager.Singleton.recipesLearnt.Count; s++)
            {
                if (PlayerRecipeManager.Singleton.recipesLearnt[s] == null)
                    PlayerRecipeManager.Singleton.recipesLearnt.RemoveAt(s);
            }

            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();


            if (player.playerRecipeManager.recipesLearnt.Count <= 0)
            {
                //equipmentInventoryWindow.SetActive(false);
                //ToggleEquipmentButtons(true);
                //RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < player.playerRecipeManager.recipesLearnt.Count; i++)
            {
                GameObject craftingSlotGameObject = Instantiate(craftingRecipeSlotPrefab, craftingRecipeContentWindow);
                UI_CraftingRecipeSlot craftingRecipeSlot = craftingSlotGameObject.GetComponent<UI_CraftingRecipeSlot>();
                craftingRecipeSlot.AddRecipe(player.playerRecipeManager.recipesLearnt[i]);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button craftingSlotButton = craftingSlotGameObject.GetComponent<Button>();
                    craftingSlotButton.Select();
                    craftingSlotButton.OnSelect(null);

                }
            }

        }

        public void CraftSelectedItem()
        {

        }
    }
}
