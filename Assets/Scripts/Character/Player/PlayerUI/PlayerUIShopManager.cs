using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUIShopManager : PlayerUIMenu
    {
        UI_InventorySlot[] shopInventory;
        [SerializeField] GameObject shopInventorySlotPrefab;
        [SerializeField] Transform inventoryInstantiationParent;

        [Header("Current Highlighted Item")]
        public TextMeshProUGUI currentHighlightedItem;
        public TextMeshProUGUI currentItemPrice;

        public ShopBuyOrSell buyingOrSelling;

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();
        }

        public void OpenBuyMenu()
        {
            buyingOrSelling = ShopBuyOrSell.Buying;
            OpenMenu();
            PopulateShopInventory();
        }

        public void OpenSellMenu()
        {
            buyingOrSelling = ShopBuyOrSell.Selling;
            OpenMenu();
            PopulatePlayerInventory();
        }

        private void PopulateShopInventory()
        {
            AICharacterManager shopKeeper = PlayerUIManager.Singleton.localPlayer.playerInteractionManager.dialogueCharacter;

            // if no shopkeeper, return
            if (shopKeeper == null)
            {
                CloseMenuAfterFixedFrame();
                return;
            }

            shopKeeper.aiCharacterInventoryManager.GenerateShop();

            // if shopkeeper has no inventory, return
            if (shopKeeper.aiCharacterInventoryManager.itemsInInventory.Count <= 0)
            {
                CloseMenuAfterFixedFrame();
                return;
            }

            // create an inventory slot for each item the shop keeper has
            for (int i = inventoryInstantiationParent.transform.childCount; i < shopKeeper.aiCharacterInventoryManager.itemsInInventory.Count; i++)
            {
                Instantiate(shopInventorySlotPrefab, inventoryInstantiationParent);
            }

            // get the inventory items
            shopInventory = inventoryInstantiationParent.GetComponentsInChildren<UI_InventorySlot>();

            // resets any old items from previous shops
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                shopInventory[i].ClearItem();

            }

            //enables all shop slots
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                shopInventory[i].gameObject.SetActive(true);
            }



            for (int i = 0; i < shopKeeper.aiCharacterInventoryManager.itemsInInventory.Count; i++)
            {
                if (shopKeeper.aiCharacterInventoryManager.itemsInInventory[i] == null)
                    continue;

                shopInventory[i].AddItem(shopKeeper.aiCharacterInventoryManager.itemsInInventory[i]);
            }

            //disables any empty shop slots
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                if (shopInventory[i].currentItem == null)
                    shopInventory[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                if (!shopInventory[i].gameObject.activeInHierarchy)
                    continue;

                Button firstSelectedButton = shopInventory[i].GetComponent<Button>();
                firstSelectedButton.Select();
                break;
            }

        }

        private void PopulatePlayerInventory()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            // if no shopkeeper, return
            if (player == null)
            {
                CloseMenuAfterFixedFrame();
                return;
            }


            // if shopkeeper has no inventory, return
            if (player.playerInventoryManager.itemsInInventory.Count <= 0)
            {
                CloseMenuAfterFixedFrame();
                return;
            }

            // create an inventory slot for each item the shop keeper has
            for (int i = inventoryInstantiationParent.transform.childCount; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                Instantiate(shopInventorySlotPrefab, inventoryInstantiationParent);
            }

            // get the inventory items
            shopInventory = inventoryInstantiationParent.GetComponentsInChildren<UI_InventorySlot>();

            // resets any old items from previous shops
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                shopInventory[i].ClearItem();

            }

            //enables all shop slots
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                shopInventory[i].gameObject.SetActive(true);
            }



            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (player.playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                shopInventory[i].AddItem(player.playerInventoryManager.itemsInInventory[i]);
            }

            //disables any empty shop slots
            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                if (shopInventory[i].currentItem == null)
                    shopInventory[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i] == null)
                    continue;

                if (!shopInventory[i].gameObject.activeInHierarchy)
                    continue;

                Button firstSelectedButton = shopInventory[i].GetComponent<Button>();
                firstSelectedButton.Select();
                break;
            }

        }

    }
}