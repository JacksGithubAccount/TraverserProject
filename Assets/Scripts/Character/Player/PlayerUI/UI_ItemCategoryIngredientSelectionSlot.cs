using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_ItemCategoryIngredientSelectionSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        [SerializeField] public Item currentItem;

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

        public void SelectItem()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            if (PlayerUIManager.Singleton.playerUICraftingManager.currentlySelectedIngredientMenuButton == null)
                return;

            PlayerUIManager.Singleton.playerUICraftingManager.currentlySelectedIngredientMenuButton.AddSelectedItem(currentItem);
        }
    }
}