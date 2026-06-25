using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_InventoryCategorySelectSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public ItemType itemType;

        [Header("Inventory or Storage")]
        public bool isInventory = true;

        private void Awake()
        {
            highlightIcon.enabled = false;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
            if (isInventory)
                PlayerUIManager.Singleton.playerUIInventoryManager.SelectInventoryCategorySelectSlot((int)itemType);
            else
            {
                PlayerUIManager.Singleton.playerUIStorageManager.itemCategory = itemType;
            }
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void DisplayInventoryBasedOnItemType()
        {
            if (isInventory)
            {
                if (itemType == ItemType.None)
                    PlayerUIManager.Singleton.playerUIInventoryManager.LoadRecentItemsInventory();
                else
                    PlayerUIManager.Singleton.playerUIInventoryManager.LoadInventoryBasedOnItemType(itemType);
            }
            else
            {
                PlayerUIManager.Singleton.playerUIStorageManager.itemCategory = itemType;
                PlayerUIManager.Singleton.playerUIStorageManager.SortStorageByCategory();
            }
        }
    }
}