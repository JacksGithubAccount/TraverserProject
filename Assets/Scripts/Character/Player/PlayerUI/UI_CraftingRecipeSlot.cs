using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_CraftingRecipeSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public Image greyOutIcon;
        [SerializeField] public Recipe currentRecipe;

        public void AddRecipe(Recipe recipe)
        {
            highlightIcon.enabled = false;
            if (recipe == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentRecipe = recipe;
            itemIcon.sprite = recipe.craftedItem.itemIcon;

            if(PlayerUIManager.Singleton.playerUICraftingManager.CheckInventoryForFullItemStack(currentRecipe.craftedItem))
            {
                highlightIcon.enabled = true;
            }
        }

        
        public void SelectSlot()
        {
            highlightIcon.enabled = true;
            PlayerUIManager.Singleton.playerUICraftingManager.DisplayRecipeInformation(currentRecipe);
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void AttemptToSelectRecipe()
        {
            if (PlayerUIManager.Singleton.playerUICraftingManager.CheckInventoryForFullItemStack(currentRecipe.craftedItem))
            {
                PlayerUIManager.Singleton.playerUICraftingManager.SendCraftMessagePopUp("Item has reached full capacity.");
            }
            else
                SelectRecipe();


        }
        public void SelectRecipe() 
        {
            PlayerUIManager.Singleton.playerUICraftingManager.DisplayIngredientMenuSelection(currentRecipe);
        }

        
    }
}
