using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_CraftingIngredientMenuSelectionButton : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemAmountText;
        [SerializeField] public Item currentItem;
        [SerializeField] public ItemCategory currentItemCategory;
        [SerializeField] public int currentItemAmountRequired;
        [SerializeField] public int currentTotalItemAmountRequired;
        [SerializeField] public Item selectedItem;
        public bool hasItemsInInventory = false;


        public void AddItem(Item item, int amountRequired)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            Item itemInPlayerInventory = null;
            if (player.playerInventoryManager.itemsInInventory.Contains(item))
                itemInPlayerInventory = player.playerInventoryManager.itemsInInventory.Find(x =>  x.name == item.name);

            int amountInInventory;
            if (itemInPlayerInventory == null)
                amountInInventory = 0;
            else
                amountInInventory = itemInPlayerInventory.currentItemAmount;

            currentItem = item;
            currentItemAmountRequired = amountRequired;
            itemAmountText.text = amountInInventory + "/" + currentItemAmountRequired;
            itemNameText.text = item.name;
        }
        public void AddItemCategory(ItemCategory itemCategory, int amountRequired)
        {
            itemIcon.enabled = true;
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<CraftingMaterial> itemCategoryInInventory = new List<CraftingMaterial>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                CraftingMaterial craftingMaterial = player.playerInventoryManager.itemsInInventory[i] as CraftingMaterial;

                if (craftingMaterial != null)
                {
                    if(craftingMaterial.itemCategory.Contains(itemCategory))
                        itemCategoryInInventory.Add(craftingMaterial);
                }
            }

            currentItemCategory = itemCategory;

            currentItemAmountRequired = amountRequired;
            itemAmountText.text = "0/" + currentItemAmountRequired;
            itemNameText.text = itemCategory.ToString();           

        }

        public void AddSelectedItem(Item item)
        {
            selectedItem = item;
            
            itemAmountText.text = selectedItem.currentItemAmount + "/" + currentItemAmountRequired;
            PlayerUIManager.Singleton.playerUICraftingManager.SelectLastSelectedIngredientMenuButton();
        }

        public void UpdateItemRequirementTextBasedOnCraftItemAmounts(int amount)
        {
            currentTotalItemAmountRequired = currentItemAmountRequired * amount;
            if(selectedItem == null)
                itemAmountText.text = "0" + "/" + currentTotalItemAmountRequired;
            else
                itemAmountText.text = selectedItem.currentItemAmount + "/" + currentTotalItemAmountRequired;
        }
        public void SelectSlot()
        {
            highlightIcon.enabled = true;            
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void ButtonClick()
        {
            PlayerUIManager.Singleton.playerUICraftingManager.SelectIngredientMenuButtonSlot(this);
            if (currentItem != null)
            {
                SelectItem();
            }
            else if (currentItemCategory != ItemCategory.None)
            {
                SelectItemCategory();
            }
        }

        private void SelectItem()
        {
            //should do nothing or indicate that item is fulfilled/unfulfilled
        }

        private void SelectItemCategory()
        {
            PlayerUIManager.Singleton.playerUICraftingManager.DisplayItemCategoryIngredientSelection(currentItemCategory);
        }
    }
}
