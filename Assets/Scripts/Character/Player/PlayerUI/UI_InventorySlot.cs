using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_InventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        [SerializeField] public Item currentItem;

        private void Awake()
        {
            highlightIcon.enabled = false;
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
            PlayerUIManager.Singleton.playerUIInventoryManager.LoadInventoryBasedOnItemType((ItemType)itemType);
        }
    }
}