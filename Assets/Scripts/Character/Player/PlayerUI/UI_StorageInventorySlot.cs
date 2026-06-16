using UnityEngine;

namespace TraverserProject
{

    public class UI_StorageInventorySlot : UI_InventorySlot
    {
        public override void SelectSlot()
        {
            base.SelectSlot();
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

    }
}