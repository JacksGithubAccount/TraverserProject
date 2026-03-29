using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
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

        [Header("Recipe Information Window")]
        [SerializeField] GameObject craftingItemInformationWindow;
        [SerializeField] GameObject craftingItemInformationPrefab;
        [SerializeField] Image craftingItemInformationImage;
        [SerializeField] TextMeshProUGUI craftingItemInformationText;
        [SerializeField] Transform craftingItemInformationContentWindow;
        [HideInInspector] private List<GameObject> craftingItemInformationPrefabs = new List<GameObject>();

        [Header("Ingredient Selection Window")]
        [SerializeField] GameObject ingredientSelectionInformationWindow;
        [SerializeField] GameObject ingredientSelectionInformationPrefab;
        [SerializeField] Transform ingredientSelectionInformationContentWindow;
        [HideInInspector] private List<GameObject> ingredientSelectionInformationPrefabs = new List<GameObject>();

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

        public void DisplayRecipeInformation(Recipe recipe)
        {
            ClearCraftingItemInformationPrefabs();
            craftingItemInformationText.text = recipe.craftedItem.itemName;
            craftingItemInformationImage.sprite = recipe.craftedItem.itemIcon;

            for (int i = 0; i < recipe.itemIngredients.Count; i++)
            {
                GameObject craftingSlotGameObject = Instantiate(craftingItemInformationPrefab, craftingItemInformationContentWindow);
                UI_CraftingItemInformationSlot craftingItemInformationSlot = craftingSlotGameObject.GetComponent<UI_CraftingItemInformationSlot>();
                craftingItemInformationSlot.AddItem(recipe.itemIngredients[i], recipe.itemIngredientsAmount[i]);
                craftingItemInformationPrefabs.Add(craftingItemInformationSlot.gameObject);
            }

            for (int i = 0; i < recipe.itemCategoryIngredients.Count; i++)
            {
                GameObject craftingSlotGameObject = Instantiate(craftingItemInformationPrefab, craftingItemInformationContentWindow);
                UI_CraftingItemInformationSlot craftingItemInformationSlot = craftingSlotGameObject.GetComponent<UI_CraftingItemInformationSlot>();
                craftingItemInformationSlot.AddItemCategory(recipe.itemCategoryIngredients[i], recipe.itemCategoryIngredientsAmount[i]);
                craftingItemInformationPrefabs.Add(craftingItemInformationSlot.gameObject);
            }
        }

        private void ClearCraftingItemInformationPrefabs()
        {
            foreach(GameObject item in craftingItemInformationPrefabs)
            {
                Destroy(item);
            }
            craftingItemInformationPrefabs.Clear();
        }
        
        public void DisplayIngredientMenuSelection(Recipe recipe)
        {
            ClearIngredientSelectionInformationPrefabs();
            for (int i = 0; i < recipe.itemIngredients.Count; i++)
            {
                
            }

            for (int i = 0; i < recipe.itemCategoryIngredients.Count; i++)
            {
                GameObject craftingSlotGameObject = Instantiate(craftingItemInformationPrefab, craftingItemInformationContentWindow);
                UI_CraftingIngredientMenuSelectionButton ingredientSelectionButton = craftingSlotGameObject.GetComponent<UI_CraftingIngredientMenuSelectionButton>();
                ingredientSelectionButton.AddItemCategory(recipe.itemCategoryIngredients[i], recipe.itemCategoryIngredientsAmount[i]);
                ingredientSelectionInformationPrefabs.Add(ingredientSelectionButton.gameObject);
            }
        }

        private void ClearIngredientSelectionInformationPrefabs()
        {
            foreach (GameObject item in ingredientSelectionInformationPrefabs)
            {
                Destroy(item);
            }
            ingredientSelectionInformationPrefabs.Clear();
        }

        public void DisplayItemCategoryIngredientSelection()
        {

        }
    }
}
