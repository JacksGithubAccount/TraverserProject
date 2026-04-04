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
        [HideInInspector] private List<GameObject> craftingRecipeSlotPrefabs = new List<GameObject>();

        [Header("Recipe Information Window")]
        [SerializeField] GameObject craftingItemInformationWindow;
        [SerializeField] GameObject craftingItemInformationPrefab;
        [SerializeField] Image craftingItemInformationImage;
        [SerializeField] TextMeshProUGUI craftingItemInformationText;
        [SerializeField] Transform craftingItemInformationContentWindow;
        [HideInInspector] private List<GameObject> craftingItemInformationPrefabs = new List<GameObject>();

        [Header("Crafting Input Window")]
        [SerializeField] GameObject craftingInputWindow;

        [Header("Ingredient Selection Window")]
        [SerializeField] GameObject ingredientSelectionInformationWindow;
        [SerializeField] GameObject ingredientSelectionInformationPrefab;
        [SerializeField] Transform ingredientSelectionInformationContentWindow;
        [HideInInspector] private List<GameObject> ingredientSelectionInformationPrefabs = new List<GameObject>();

        [Header("Item Category Ingredient Selection Window")]
        [SerializeField] GameObject itemCategoryIngredientSelectionInformationWindow;
        [SerializeField] GameObject itemCategoryIngredientSelectionInformationPrefab;
        [SerializeField] Transform itemCategoryIngredientSelectionInformationContentWindow;
        [HideInInspector] private List<GameObject> itemCategoryIngredientSelectionInformationPrefabs = new List<GameObject>();

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

            ClearGameObjectPrefabs(craftingRecipeSlotPrefabs);
            for (int i = 0; i < player.playerRecipeManager.recipesLearnt.Count; i++)
            {
                GameObject craftingSlotGameObject = Instantiate(craftingRecipeSlotPrefab, craftingRecipeContentWindow);
                UI_CraftingRecipeSlot craftingRecipeSlot = craftingSlotGameObject.GetComponent<UI_CraftingRecipeSlot>();
                craftingRecipeSlot.AddRecipe(player.playerRecipeManager.recipesLearnt[i]);
                craftingRecipeSlotPrefabs.Add(craftingRecipeSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button craftingSlotButton = craftingSlotGameObject.GetComponent<Button>();
                    craftingSlotButton.Select();
                    craftingSlotButton.OnSelect(null);

                }
            }

        }

        private void ClearGameObjectPrefabs(List<GameObject> listOfGameObjectPrefabs)
        {
            foreach (GameObject item in listOfGameObjectPrefabs)
            {
                Destroy(item);
            }
            listOfGameObjectPrefabs.Clear();
        }

        public void CraftSelectedItem()
        {

        }

        public void DisplayRecipeInformation(Recipe recipe)
        {
            ClearGameObjectPrefabs(craftingItemInformationPrefabs);
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
        
        public void DisplayIngredientMenuSelection(Recipe recipe)
        {
            ClearGameObjectPrefabs(ingredientSelectionInformationPrefabs);
            OpenSubMenu(craftingInputWindow);
            for (int i = 0; i < recipe.itemIngredients.Count; i++)
            {
                GameObject ingredientSelectionGameObject = Instantiate(ingredientSelectionInformationPrefab, ingredientSelectionInformationContentWindow);
                UI_CraftingIngredientMenuSelectionButton ingredientSelectionButton = ingredientSelectionGameObject.GetComponent<UI_CraftingIngredientMenuSelectionButton>();
                ingredientSelectionButton.AddItem(recipe.itemIngredients[i], recipe.itemIngredientsAmount[i]);
                ingredientSelectionInformationPrefabs.Add(ingredientSelectionButton.gameObject);
            }

            for (int i = 0; i < recipe.itemCategoryIngredients.Count; i++)
            {
                GameObject ingredientSelectionGameObject = Instantiate(ingredientSelectionInformationPrefab, ingredientSelectionInformationContentWindow);
                UI_CraftingIngredientMenuSelectionButton ingredientSelectionButton = ingredientSelectionGameObject.GetComponent<UI_CraftingIngredientMenuSelectionButton>();
                ingredientSelectionButton.AddItemCategory(recipe.itemCategoryIngredients[i], recipe.itemCategoryIngredientsAmount[i]);
                ingredientSelectionInformationPrefabs.Add(ingredientSelectionButton.gameObject);
            }
        }
   

        public void DisplayItemCategoryIngredientSelection(ItemCategory itemCategory)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<CraftingMaterial> itemCategoryInInventory = new List<CraftingMaterial>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                CraftingMaterial craftingMaterial = player.playerInventoryManager.itemsInInventory[i] as CraftingMaterial;

                if (craftingMaterial != null)
                {
                    if (craftingMaterial.itemCategory.Contains(itemCategory))
                        itemCategoryInInventory.Add(craftingMaterial);
                }
            }

            bool hasSelectedFirstInventorySlot = false;
            ClearGameObjectPrefabs(itemCategoryIngredientSelectionInformationPrefabs);

            for (int i = 0; i < itemCategoryInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(itemCategoryIngredientSelectionInformationPrefab, itemCategoryIngredientSelectionInformationContentWindow);
                UI_ItemCategoryIngredientSelectionSlot itemCategoryIngredientSelectionSlot = inventorySlotGameObject.GetComponent<UI_ItemCategoryIngredientSelectionSlot>();
                itemCategoryIngredientSelectionSlot.AddItem(itemCategoryInInventory[i]);
                itemCategoryIngredientSelectionInformationPrefabs.Add(itemCategoryIngredientSelectionSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }
    }
}
