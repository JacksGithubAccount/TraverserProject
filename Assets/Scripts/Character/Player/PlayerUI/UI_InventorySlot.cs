using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_InventorySlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public Item currentItem;


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

        public void ClearItem()
        {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

    }
}