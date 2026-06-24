using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_InventoryCategorySelectSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public ItemType itemType;

        private void Awake()
        {
            highlightIcon.enabled = false;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
            PlayerUIManager.Singleton.playerUIInventoryManager.SelectInventoryCategorySelectSlot((int)itemType);
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void SetStorageCategory()
        {
            PlayerUIManager.Singleton.playerUIStorageManager.itemCategory = itemType;
            PlayerUIManager.Singleton.playerUIStorageManager.SortStorageByCategory();
        }

        public void DisplayInventoryBasedOnItemType()
        {
            if (itemType == ItemType.None)
                PlayerUIManager.Singleton.playerUIInventoryManager.LoadRecentItemsInventory();
            else
                PlayerUIManager.Singleton.playerUIInventoryManager.LoadInventoryBasedOnItemType(itemType);
        }
    }
}