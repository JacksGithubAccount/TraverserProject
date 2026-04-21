using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUIInventoryManager : PlayerUIMenu
    {
        [Header("Inventory")]
        [SerializeField] GameObject inventoryWindow;
        [SerializeField] GameObject inventorySlotPrefab;
        [SerializeField] Transform inventoryContentWindow;
        [SerializeField] Item currentlySelectedItem;
        [HideInInspector] List<GameObject> inventorySlotPrefabs = new List<GameObject>();
        [SerializeField] TextMeshProUGUI categoryNameText;


        public override void OpenMenu()
        {
            base.OpenMenu();

            ToggleInventoryButtons(true);
            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //RefreshMenu();
            LoadInventory();
        }

        public void ToggleInventoryButtons(bool isEnabled)
        {
            foreach (var gameObject in inventorySlotPrefabs)
            {
                gameObject.SetActive(isEnabled);
            }

        }

        public void LoadInventory()
        {
            ClearInventorySlotPrefabs();
            categoryNameText.text = "Recent Items";
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<Item> itemsInInventory = new List<Item>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                Item item = player.playerInventoryManager.itemsInInventory[i];

                if (item != null)
                    itemsInInventory.Add(item);
            }

            if (itemsInInventory.Count <= 0)
            {
                inventoryWindow.SetActive(false);
                ToggleInventoryButtons(true);
                //RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(inventorySlotPrefab, inventoryContentWindow);
                UI_InventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_InventorySlot>();
                inventorySlot.AddItem(itemsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        private void ClearInventorySlotPrefabs()
        {
            foreach (GameObject item in inventorySlotPrefabs)
            {
                Destroy(item);
            }
            inventorySlotPrefabs.Clear();
        }

    }

}