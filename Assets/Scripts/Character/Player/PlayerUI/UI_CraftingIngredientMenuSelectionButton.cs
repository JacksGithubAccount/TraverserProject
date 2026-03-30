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
            itemAmountText.text = amountInInventory + "/" + amountRequired;
            itemNameText.text = item.name;
        }
        public void AddItemCategory(ItemCategory itemCategory, int amount)
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

                itemAmountText.text = "0/" + amount;
                itemNameText.text = itemCategory.ToString();
            

        }
        public void SelectSlot()
        {
            highlightIcon.enabled = true;            
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void SelectItemCategory()
        {
            if (currentItemCategory == ItemCategory.None)
                return;

            PlayerUIManager.Singleton.playerUICraftingManager.DisplayItemCategoryIngredientSelection(currentItemCategory);
        }
    }
}
