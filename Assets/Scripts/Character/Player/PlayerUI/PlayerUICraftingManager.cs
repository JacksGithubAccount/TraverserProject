using Steamworks.Ugc;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUICraftingManager : PlayerUIMenu
    {
        [Header("Craft Message Pop Up")]
        [SerializeField] GameObject craftMessagePopUpGameObject;
        [SerializeField] TextMeshProUGUI craftMessagePopUpText;

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

        [Header("Craft Button")]
        [HideInInspector] public List<Item> selectedItems = new List<Item>();
        public UI_CraftingIngredientMenuSelectionButton currentlySelectedIngredientMenuButton;
        public Slider craftItemAmountSlider;
        [SerializeField] TextMeshProUGUI craftingItemAmountText;
        [SerializeField] Button craftConfimButton;

        [Header("Item Amount Texts")]
        [SerializeField] TextMeshProUGUI heldCraftedItemAmountText;
        private string heldCraftedItemAmountTextString = "Held Items: ";
        [SerializeField] TextMeshProUGUI willCraftItemAmountText;
        private string willCraftItemAmountTextString = "Crafted Amount: ";
        [SerializeField] TextMeshProUGUI totalCraftItemAmountText;
        private string totalCraftItemAmountTextString = "Total Amount: ";

        [Header("Text Colors")]
        [SerializeField] Color standardColor = Color.white;
        [SerializeField] Color negativeColor = Color.red;
        [SerializeField] Color positiveColor = Color.blue;


        public override void OpenMenu()
        {
            base.OpenMenu();

            CheckForUnlockedRecipes();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
            ToggleGameObjectPrefabs(craftingRecipeSlotPrefabs, true);
        }

        public void SendCraftMessagePopUp(string messageText)
        {
            craftMessagePopUpText.text = messageText;
            OpenSubMenu(craftMessagePopUpGameObject);
            ToggleGameObjectPrefabs(craftingRecipeSlotPrefabs, false);
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
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            PlayerUIManager.Singleton.ConfirmSFX();
            CloseSubMenu();

            //adds crafted item to inventory
            Item craftedItem = Instantiate(currentlySelectedRecipe.craftedItem);
            craftedItem.currentItemAmount = currentlySelectedRecipe.craftedItemAmount * (int)craftItemAmountSlider.value;
            player.playerInventoryManager.AddItemToInventory(craftedItem);

            //removes ingredients from inventory
            List<Item> ingredients = new List<Item>();
            foreach (var prefab in ingredientSelectionInformationPrefabs)
            {
                UI_CraftingIngredientMenuSelectionButton button;
                button = prefab.GetComponent<UI_CraftingIngredientMenuSelectionButton>();

                if (button == null)
                    continue;

                Item ingredient = Instantiate(button.selectedItem);
                ingredient.currentItemAmount = button.currentTotalItemAmountRequired;
                ingredients.Add(ingredient);
            }
            foreach(Item ingredient in ingredients)
            {
                player.playerInventoryManager.RemoveItemFromInventory(ingredient);
            }
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
            currentlySelectedRecipe = recipe;
            ClearGameObjectPrefabs(ingredientSelectionInformationPrefabs);
            OpenSubMenu(craftingInputWindow);
            ToggleGameObjectPrefabs(craftingRecipeSlotPrefabs, false);
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
            //resets craft mount
            craftItemAmountSlider.value = 1;

            craftConfimButton.interactable = false;
            UpdateSliderBasedOnCraftItemAmount();

            //changes max craftable so you cant craft more than max stacks and sets items text
            if (PlayerUIManager.Singleton.localPlayer.playerInventoryManager.CheckIfItemIsInInventoryOrEquipSlots(currentlySelectedRecipe.craftedItem))
            {
                Item item = PlayerUIManager.Singleton.localPlayer.playerInventoryManager.GetItemInInventoryOrEquipSlots(currentlySelectedRecipe.craftedItem);
                craftItemAmountSlider.maxValue = (item.maxItemAmount - item.currentItemAmount) / currentlySelectedRecipe.craftedItemAmount;
                heldCraftedItemAmountText.text = heldCraftedItemAmountTextString + item.currentItemAmount;
                totalCraftItemAmountText.text = totalCraftItemAmountTextString + ((currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value) + item.currentItemAmount);
            }
            else
            {
                heldCraftedItemAmountText.text = heldCraftedItemAmountTextString + 0;
                totalCraftItemAmountText.text = totalCraftItemAmountTextString + (currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value);
            }
            willCraftItemAmountText.text = willCraftItemAmountTextString + currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value;
            
        }
   

        public void DisplayItemCategoryIngredientSelection(ItemCategory itemCategory)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            itemCategoryIngredientSelectionInformationWindow.SetActive(true);
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
        public void SelectIngredientMenuButtonSlot(UI_CraftingIngredientMenuSelectionButton ingredientMenuSelectionButton)
        {
            currentlySelectedIngredientMenuButton = ingredientMenuSelectionButton;
        }
        public void SelectLastSelectedIngredientMenuButton()
        {
            Button lastSelectedButton = null;
            lastSelectedButton = currentlySelectedIngredientMenuButton.GetComponent<Button>();

            if (lastSelectedButton != null)
            {
                lastSelectedButton.Select();
                lastSelectedButton.OnSelect(null);
            }

            UpdateSliderBasedOnCraftItemAmount();
            itemCategoryIngredientSelectionInformationWindow.SetActive(false);
        }

        public bool CheckInventoryForFullItemStack(Recipe recipe)
        {
            bool isInventoryItemStackFull = false;

            if (PlayerUIManager.Singleton.localPlayer.playerInventoryManager.CheckIfItemIsInInventoryOrEquipSlots(recipe.craftedItem))
            {

                Item itemInInventory = PlayerUIManager.Singleton.localPlayer.playerInventoryManager.GetItemInInventoryOrEquipSlots(recipe.craftedItem);
                if (itemInInventory.currentItemAmount + recipe.craftedItemAmount > recipe.craftedItem.maxItemAmount)
                    isInventoryItemStackFull = true;


            }
            return isInventoryItemStackFull;
        }

        public void UpdateSliderBasedOnCraftItemAmount()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            craftingItemAmountText.text = "x" + craftItemAmountSlider.value.ToString();
            willCraftItemAmountText.text = willCraftItemAmountTextString + currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value;
            if (PlayerUIManager.Singleton.localPlayer.playerInventoryManager.CheckIfItemIsInInventoryOrEquipSlots(currentlySelectedRecipe.craftedItem))
            {
                Item item = PlayerUIManager.Singleton.localPlayer.playerInventoryManager.GetItemInInventoryOrEquipSlots(currentlySelectedRecipe.craftedItem);
                totalCraftItemAmountText.text = totalCraftItemAmountTextString + ((currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value) + item.currentItemAmount);
            }
            else
            {
                totalCraftItemAmountText.text = totalCraftItemAmountTextString + (currentlySelectedRecipe.craftedItemAmount * craftItemAmountSlider.value);
            }

            bool sufficientIndredientAmount = true;
            foreach (var item in ingredientSelectionInformationPrefabs)
            {
                UI_CraftingIngredientMenuSelectionButton button;
                button = item.GetComponent<UI_CraftingIngredientMenuSelectionButton>();

                if (button == null)
                    continue;

                button.UpdateItemRequirementTextBasedOnCraftItemAmounts((int)craftItemAmountSlider.value);

                if (button.selectedItem != null)
                {

                    if (button.currentTotalItemAmountRequired > button.selectedItem.currentItemAmount)
                    {
                        craftConfimButton.interactable = false;
                        sufficientIndredientAmount = false;
                    }
                    ChangeTextFieldToSpecificColorBasedOnAmount(button.itemAmountText, button.selectedItem.currentItemAmount, button.currentTotalItemAmountRequired);
                }
                else
                {
                    sufficientIndredientAmount = false;
                    ChangeTextFieldToSpecificColorBasedOnAmount(button.itemAmountText, 0, button.currentTotalItemAmountRequired);
                }                    
            }

            if(sufficientIndredientAmount)
            {
                craftConfimButton.interactable = true;
            }

        }

        private void ChangeTextFieldToSpecificColorBasedOnAmount(TextMeshProUGUI textField, int itemAmount, int requiredAmount)
        {
            if (requiredAmount == itemAmount)
                textField.color = standardColor;

            if (requiredAmount > itemAmount)
            {
                textField.color = negativeColor;
            }
            else
            {
                textField.color = standardColor;
            }

        }
    }
}
