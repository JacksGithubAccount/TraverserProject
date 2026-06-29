using TMPro;
using UnityEngine;

namespace TraverserProject
{

    public class UI_StorageInventorySlot : UI_InventorySlot
    {
        public TextMeshProUGUI CurrentItemAmountText;
        [Header("Inventory Slot Type")]
        public bool isSelectingFromPlayerInventory;

        public override void SelectSlot()
        {
            base.SelectSlot();

            PlayerUIManager.Singleton.playerUIStorageManager.isSelectingFromPlayerInventory = isSelectingFromPlayerInventory;

            if (PlayerUIManager.Singleton.playerUIStorageManager.isSelectingFromPlayerInventory)
            {
                if (currentItem == null)
                {
                    PlayerUIManager.Singleton.playerUIStorageManager.playerInventoryCurrentItemSelectedText.text = "";
                    return;
                }

                PlayerUIManager.Singleton.playerUIStorageManager.playerInventoryCurrentItemSelectedText.text = currentItem.itemName;
            }
            else
            {
                if (currentItem == null)
                {
                    PlayerUIManager.Singleton.playerUIStorageManager.playerStorageCurrentItemSelectedText.text = "";
                    return;
                }

                PlayerUIManager.Singleton.playerUIStorageManager.playerStorageCurrentItemSelectedText.text = currentItem.itemName;
            }
        }

        public void AttemptToOpenInventorySelectionMenu()
        {
            if (currentItem == null)
                return;

            if (currentItem.maxItemAmount == 1)
            {
                SwapItemLocation();
            }
            else
            {
                PlayerUIManager.Singleton.playerUIStorageManager.AttemptToOpenInventorySelectionAmountMenu(this, currentItem);
            }
        }

        public void SwapItemLocation()
        {
            if (currentItem == null)
                return;

            if (isSelectingFromPlayerInventory)
            {
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToStorage(Instantiate(currentItem));
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.RemoveItemFromInventory(currentItem);                
            }
            else
            {
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToInventory(currentItem);
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.RemoveItemFromStorage(currentItem);                
            }
            PlayerUIManager.Singleton.playerUIStorageManager.RefreshStorage();
            PlayerUIManager.Singleton.playerUIStorageManager.SelectFirstButton();
        }

    }
}