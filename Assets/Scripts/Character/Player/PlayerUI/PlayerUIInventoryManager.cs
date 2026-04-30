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

        [Header("Inventory Category Select")]
        [SerializeField] Scrollbar inventoryCategorySelectScrollbar;
        public int inventoryCategorySelectScrollbarIndex;
        public ItemType currentSelectedInventoryCategorySelectSlot;
        public List<GameObject> inventoryCategorySelectSlotPrefabs = new List<GameObject>();

        [Header("InventorySelectionMenu")]
        [SerializeField] GameObject inventorySelectionMenuWindow;


        public override void OpenMenu()
        {
            base.OpenMenu();

            ToggleInventoryButtons(true);
            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //RefreshMenu();
            LoadRecentItemsInventory();
        }

        public void ToggleInventoryButtons(bool isEnabled)
        {
            foreach (var gameObject in inventorySlotPrefabs)
            {
                gameObject.SetActive(isEnabled);
            }

        }

        //Inventory Category Select
        public void LoadRecentItemsInventory()
        {
            ClearInventorySlotPrefabs();
            categoryNameText.text = "Recent Items";
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<Item> itemsInInventory = new List<Item>();

            for (int i = player.playerInventoryManager.itemsInInventory.Count - 1; i > 0; i--)
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

        public void LoadInventoryBasedOnItemType(ItemType itemType)
        {
            ClearInventorySlotPrefabs();
            categoryNameText.text = itemType.ToString();
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<Item> itemsInInventory = new List<Item>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {                
                Item item = player.playerInventoryManager.itemsInInventory[i];

                if (item == null)
                    continue;

                if(item.itemType == itemType)
                    itemsInInventory.Add(item);
            }

            if (itemsInInventory.Count <= 0)
            {
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


        public void SelectInventoryCategorySelectSlot(int slotNumber)
        {
            currentSelectedInventoryCategorySelectSlot = (ItemType)slotNumber;            
        }

        public void ChangeSelectedInventoryCategorySelectSlot(int slotNumber)
        {
            Button button = inventoryCategorySelectSlotPrefabs[slotNumber].GetComponent<Button>();
            button.Select();
            button.OnSelect(null);
        }

        private void ClearInventorySlotPrefabs()
        {
            foreach (GameObject item in inventorySlotPrefabs)
            {
                Destroy(item);
            }
            inventorySlotPrefabs.Clear();
        }

        public void OpenInventorySelectionMenu(UI_InventorySlot itemSlot)
        {
            currentlySelectedItem = itemSlot.currentItem;
            OpenSubMenu(inventorySelectionMenuWindow);
            ToggleGameObjectPrefabs(inventorySlotPrefabs, false);
            ToggleGameObjectPrefabs(inventoryCategorySelectSlotPrefabs, false);
            foreach(var slot in inventorySlotPrefabs)
            {
                UI_InventorySlot islot = slot.GetComponent<UI_InventorySlot>();
                islot.greyedOutIcon.enabled = false;
            }
            itemSlot.GlowIcon.enabled = true;
            Image image = itemSlot.GetComponent<Image>();

            inventorySelectionMenuWindow.transform.position = new Vector3(itemSlot.transform.position.x + image.flexibleWidth, inventorySelectionMenuWindow.transform.position.y, inventorySelectionMenuWindow.transform.position.z);

        }

        public void UseSelectedItem()
        {

        }

        public void DropSelectedItem()
        {

        }

        public void DiscardSelectedItem()
        {

        }

    }

}