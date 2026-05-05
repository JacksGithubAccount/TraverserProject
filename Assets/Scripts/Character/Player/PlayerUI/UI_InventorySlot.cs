using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_InventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public Image greyedOutIcon;
        public Image GlowIcon;
        public TextMeshProUGUI CurrentItemAmountText;
        [SerializeField] public Item currentItem;

        [Header("Inventory Category Select")]
        public ItemType itemType;

        private void Awake()
        {
            highlightIcon.enabled = false;
            greyedOutIcon.enabled = false;
            GlowIcon.enabled = false;
        }
        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void DisplayInventoryBasedOnItemType(int itemType)
        {
            if(itemType == 0)
                PlayerUIManager.Singleton.playerUIInventoryManager.LoadRecentItemsInventory();
            else
                PlayerUIManager.Singleton.playerUIInventoryManager.LoadInventoryBasedOnItemType((ItemType)itemType);
        }

        public void OpenInventorySelectionMenu()
        {
            PlayerUIManager.Singleton.playerUIInventoryManager.OpenInventorySelectionMenu(this);
        }
    }
}