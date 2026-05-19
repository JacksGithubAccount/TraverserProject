using UnityEngine;

namespace TraverserProject
{

    public class UI_ShopInventorySlot : UI_InventorySlot
    {

        public void BuyItem()
        {
            //check for null
            if (currentItem == null)
            {
                ClearItem();
                return;
            }

            //check for item value, polish option to fade out and add SFX
            if (PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles < currentItem.itemValue)
                return;

            //check for remaining item amount
            if (currentItem.currentItemAmount <= 0)
            {
                ClearItem();
                return;
            }

            Item purchasedItem = Instantiate(currentItem);
            purchasedItem.isInfinite = false;
            purchasedItem.currentItemAmount = 1;
            PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToInventory(purchasedItem);
            PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles -= currentItem.itemValue;

            if (!currentItem.isInfinite)
            {
                currentItem.currentItemAmount -= 1;
            }

            if (currentItem.currentItemAmount <= 0)
            {
                ClearItem();
                return;
            }

        }

        public override void SelectSlot()
        {
            PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.color = WorldUtilityManager.Singleton.regularTextColor;


            if (currentItem == null)
            {
                PlayerUIManager.Singleton.playerUIShopManager.currentHighlightedItem.text = "";
                PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.text = "";

            }
            else
            {
                if (PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles < currentItem.itemValue)
                    PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.color = WorldUtilityManager.Singleton.negativeTextColor;

                PlayerUIManager.Singleton.playerUIShopManager.currentHighlightedItem.text = currentItem.itemName;
                PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.text = currentItem.itemValue.ToString();

            }

            base.SelectSlot();
        }
    }
}