using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

namespace TraverserProject
{
    public class UI_CraftingRecipeSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public Image greyOutIcon;
        [SerializeField] TextMeshProUGUI craftedItemAmountText;
        [SerializeField] public Recipe currentRecipe;

        private void Awake()
        {
            greyOutIcon.enabled = false;
            highlightIcon.enabled = false;
        }
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
            craftedItemAmountText.text = "x" + recipe.craftedItemAmount.ToString();

            if (PlayerUIManager.Singleton.playerUICraftingManager.CheckInventoryForFullItemStack(currentRecipe))
            {
                greyOutIcon.enabled = true;
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
            if (PlayerUIManager.Singleton.playerUICraftingManager.CheckInventoryForFullItemStack(currentRecipe))
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
