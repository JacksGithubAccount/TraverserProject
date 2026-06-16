using UnityEngine;
using TMPro;

namespace TraverserProject
{

    public class PlayerUIStorageManager : PlayerUIMenu
    {
        [Header("Inventories")]
        [SerializeField] GameObject storageInventorySlotGameObject;
	    [SerializeField] Transform playerInventoryInstantiationParent;
        private UI_StorageInventorySlot[] playerInventory;

        [Header("Currently Selecting From")]
        public bool isSelectingFromPlayerInventory = true;

        [Header("Titles")]
        public TextMeshProUGUI playerInventoryCurrentItemSelectedText;
        public TextMeshProUGUI playerStorageCurrentItemSelectedText;

        public override void OpenMenu()
        {
            base.OpenMenu();

            PopulateInventory();
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();

            PopulateInventory();
        }

        private void PopulateInventory()
        {
            isSelectingFromPlayerInventory = true;

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            for (int i = playerInventoryInstantiationParent.transform.childCount; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                Instantiate(storageInventorySlotGameObject, playerInventoryInstantiationParent);
            }

            playerInventory = playerInventoryInstantiationParent.GetComponentsInChildren<UI_StorageInventorySlot>();

            // resets any old items from previous shops
            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i] == null)
                    continue;

                playerInventory[i].ClearItem();

            }

            //enables all shop slots
            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i] == null)
                    continue;

                playerInventory[i].gameObject.SetActive(true);
            }



            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (player.playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                playerInventory[i].AddItem(player.playerInventoryManager.itemsInInventory[i]);
            }

            //disables any empty shop slots
            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i] == null)
                    continue;

                if (playerInventory[i].currentItem == null)
                    playerInventory[i].gameObject.SetActive(false);
            }


        }

    }
}