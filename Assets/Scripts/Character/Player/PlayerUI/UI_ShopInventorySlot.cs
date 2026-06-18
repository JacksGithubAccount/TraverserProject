using UnityEngine;

namespace TraverserProject
{

    public class UI_ShopInventorySlot : UI_InventorySlot
    {
        public void BuyOrSellItem()
        {
            switch (PlayerUIManager.Singleton.playerUIShopManager.buyingOrSelling)
            {
                case ShopBuyOrSell.Buying:
                    BuyItem();
                    break;
                case ShopBuyOrSell.Selling:
                    SellItem();
                    break;
                default:
                    break;
            }
        }

        private void BuyItem()
        {
            AICharacterManager shopKeeper = PlayerUIManager.Singleton.localPlayer.playerInteractionManager.dialogueCharacter;

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
                if (shopKeeper != null)
                    shopKeeper.aiCharacterInventoryManager.RemoveItemFromInventory(currentItem);
                ClearItem();
                return;
            }




            Item purchasedItem = Instantiate(currentItem);
            purchasedItem.isInfinite = false;
            purchasedItem.currentItemAmount = 1;
            PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToInventory(purchasedItem);
            PlayerUIManager.Singleton.localPlayer.playerStatsManager.AddBubbles(-currentItem.itemValue);

            if (!currentItem.isInfinite)
            {
                currentItem.currentItemAmount -= 1;
            }

            if (currentItem.currentItemAmount <= 0)
            {
                if (shopKeeper != null)
                    shopKeeper.aiCharacterInventoryManager.RemoveItemFromInventory(currentItem);
                ClearItem();
                return;
            }

        }

        private void SellItem()
        {
            //check for null
            if (currentItem == null)
            {
                ClearItem();
                return;
            }


            //check for remaining item amount
            if (currentItem.currentItemAmount <= 0)
            {
                ClearItem();
                return;
            }


            PlayerUIManager.Singleton.localPlayer.playerInventoryManager.RemoveItemFromInventory(currentItem);
            int bubblesGained = Mathf.RoundToInt((float)currentItem.itemValue / WorldUtilityManager.Singleton.itemSellDivisionValue);
            PlayerUIManager.Singleton.localPlayer.playerStatsManager.AddBubbles(bubblesGained);
            ClearItem();

        }

        public override void SelectSlot()
        {
            PlayerUIManager.Singleton.playerUIInventoryManager.DispayItemDetail(currentItem);
            switch (PlayerUIManager.Singleton.playerUIShopManager.buyingOrSelling)
            {
                case ShopBuyOrSell.Buying:
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
                    break;
                case ShopBuyOrSell.Selling:
                    PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.color = WorldUtilityManager.Singleton.regularTextColor;


                    if (currentItem == null)
                    {
                        PlayerUIManager.Singleton.playerUIShopManager.currentHighlightedItem.text = "";
                        PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.text = "";

                    }
                    else
                    {

                        PlayerUIManager.Singleton.playerUIShopManager.currentHighlightedItem.text = currentItem.itemName;
                        PlayerUIManager.Singleton.playerUIShopManager.currentItemPrice.text = (currentItem.itemValue / WorldUtilityManager.Singleton.itemSellDivisionValue).ToString();

                    }
                    break;
                default:
                    break;
            }



            base.SelectSlot();
        }
    }
}