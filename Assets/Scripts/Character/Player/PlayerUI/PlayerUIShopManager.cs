using UnityEngine;

namespace TraverserProject
{

    public class PlayerUIShopManager : PlayerUIMenu
    {
        UI_InventorySlot[] shopInventory;
        [SerializeField] GameObject shopInventorySlotPrefab;
        [SerializeField] Transform inventoryInstantiationParent;

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
            OpenMenu();
            PopulateShopInventory();
        }

        private void PopulateShopInventory()
        {
            AICharacterManager shopKeeper = PlayerUIManager.Singleton.localPlayer.playerInteractionManager.dialogueCharacter;

            // if no shopkeeper, return
            if (shopKeeper == null)
            {
                CloseMenu();
                return;
            }

            // if shopkeeper has no inventory, return
            if (shopKeeper.aiCharacterInventoryManager.itemsInInventory.Count <= 0)
            {
                CloseMenu();
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

        }

    }
}