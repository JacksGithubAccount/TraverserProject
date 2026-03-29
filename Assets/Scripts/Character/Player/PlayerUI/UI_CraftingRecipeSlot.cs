using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_CraftingRecipeSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        [SerializeField] public Recipe currentRecipe;

        public void AddRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentRecipe = recipe;
            itemIcon.sprite = recipe.craftedItem.itemIcon;
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

        public void SelectRecipe() 
        {
            PlayerUIManager.Singleton.playerUICraftingManager.DisplayIngredientMenuSelection(currentRecipe);
        }

        
    }
}
