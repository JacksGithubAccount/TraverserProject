using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
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
        [SerializeField] GameObject closeSubmenuWindow;
        [SerializeField] Button inventorySelectionMenuUseTextButton;
        private int selectedInventorySelectionMenuButton; // 0 none, 1 use, 2 drop, 3 discard

        [Header("InventorySelectionAmountMenu")]
        [SerializeField] GameObject inventorySelectionAmountMenuWindow;
        [SerializeField] Slider inventorySelectionAmountSlider;        
        [SerializeField] TextMeshProUGUI inventorySelectionAmountText;

        [Header("Inventory Detail Menu")]
        [SerializeField] GameObject inventoryDetailWindow;
        [SerializeField] TextMeshProUGUI inventoryDetailItemNameText;

        public override void OpenMenu()
        {
            base.OpenMenu();

            ToggleInventoryButtons(true);
            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //RefreshMenu();
            LoadRecentItemsInventory();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
            closeSubmenuWindow.SetActive(false);
            ToggleGameObjectPrefabs(inventorySlotPrefabs, true);
            ToggleGameObjectPrefabs(inventoryCategorySelectSlotPrefabs, true);
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
                UI_PlayerInventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_PlayerInventorySlot>();
                inventorySlot.AddItem(itemsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                inventorySlot.CurrentItemAmountText.enabled = false;
                if (inventorySlot.currentItem.currentItemAmount > 1)
                {
                    inventorySlot.CurrentItemAmountText.text = "x" + inventorySlot.currentItem.currentItemAmount;
                    inventorySlot.CurrentItemAmountText.enabled = true;
                }

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
                UI_PlayerInventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_PlayerInventorySlot>();
                inventorySlot.AddItem(itemsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                inventorySlot.CurrentItemAmountText.enabled = false;
                if (inventorySlot.currentItem.currentItemAmount > 1)
                {
                    inventorySlot.CurrentItemAmountText.text = "x" + inventorySlot.currentItem.currentItemAmount;
                    inventorySlot.CurrentItemAmountText.enabled = true;
                }

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

        public void OpenInventorySelectionMenu(UI_PlayerInventorySlot itemSlot)
        {
            currentlySelectedItem = itemSlot.currentItem;

            if (currentlySelectedItem.itemType != ItemType.Tool)
                inventorySelectionMenuUseTextButton.interactable = false;
            else
                inventorySelectionMenuUseTextButton.interactable = true;

            closeSubmenuWindow.SetActive(true);
            OpenSubMenu(inventorySelectionMenuWindow);
            ToggleGameObjectPrefabs(inventorySlotPrefabs, false);
            ToggleGameObjectPrefabs(inventoryCategorySelectSlotPrefabs, false);
            foreach(var slot in inventorySlotPrefabs)
            {
                UI_PlayerInventorySlot islot = slot.GetComponent<UI_PlayerInventorySlot>();
                islot.greyedOutIcon.enabled = false;            
            }
            itemSlot.GlowIcon.enabled = true;

            RectTransform imageRectTransform = itemSlot.GetComponent<RectTransform>();
            
            RectTransform menuWindowRectTransform = inventorySelectionMenuWindow.GetComponent<RectTransform>();
            inventorySelectionMenuWindow.transform.position = new Vector3(imageRectTransform.transform.position.x + imageRectTransform.rect.width * 2, imageRectTransform.transform.position.y, inventorySelectionMenuWindow.transform.position.z);

            
        }

        public void AttemptToOpenInventorySelectionAmountMenu()
        {
            //consider usable souls
            if (selectedInventorySelectionMenuButton == 1)
            {
                if (currentlySelectedItem.GetType() != typeof(BubblesItem))
                {
                    ConfirmInventorySelectionAmount();
                    return;
                }
            }

            OpenSubMenu(inventorySelectionAmountMenuWindow);

            RectTransform imageRectTransform = inventorySelectionMenuWindow.GetComponent<RectTransform>();
            RectTransform menuWindowRectTransform = inventorySelectionAmountMenuWindow.GetComponent<RectTransform>();
            inventorySelectionAmountMenuWindow.transform.position = new Vector3(inventorySelectionMenuWindow.transform.position.x + imageRectTransform.rect.width, imageRectTransform.transform.position.y, inventorySelectionAmountMenuWindow.transform.position.z);

            inventorySelectionAmountSlider.maxValue = currentlySelectedItem.currentItemAmount;
        }

        public void SelectInventorySelectionMenuButton(int number)
        {
            selectedInventorySelectionMenuButton = number;
        }

        public void ConfirmInventorySelectionAmount()
        {
            Item item = Instantiate(currentlySelectedItem);
            item.currentItemAmount = (int)inventorySelectionAmountSlider.value;
            

            if (selectedInventorySelectionMenuButton == 0)
                return;
            else if (selectedInventorySelectionMenuButton == 1)
                UseSelectedItem();
            else if (selectedInventorySelectionMenuButton == 2)
                DropSelectedItem(item);
            else if (selectedInventorySelectionMenuButton == 3)
                DiscardSelectedItem(item);
        }

        public void UpdateSliderValue()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            inventorySelectionAmountText.text = "x" + inventorySelectionAmountSlider.value.ToString();

            //if(inventorySelectionAmountSlider.value > currentlySelectedItem.currentItemAmount)
            //    inventorySelectionAmountSlider.value = currentlySelectedItem.currentItemAmount;
        }



        public void UseSelectedItem()
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            QuickSlotItem qsItem = currentlySelectedItem as QuickSlotItem;
            if (qsItem == null)
                return;

            qsItem.numberOfItemsToUse = (int)inventorySelectionAmountSlider.value;
            player.playerInventoryManager.menuSelectedQuickSlotItem = qsItem;

            qsItem.AttemptToUseItem(player);
            player.playerNetworkManager.NotifyTheServerOfQuickSlotItemActionServerRpc(NetworkManager.Singleton.LocalClientId, qsItem.itemID);
        }

        public void DropSelectedItem(Item item)
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerInventoryManager.DropItemFromInventory(item);
        }

        public void DiscardSelectedItem(Item item)
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerInventoryManager.RemoveItemFromQuickSlotOrInventory(item);
        }

    }

}