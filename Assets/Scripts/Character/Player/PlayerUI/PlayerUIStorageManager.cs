using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUIStorageManager : PlayerUIMenu
    {
        [Header("Inventories")]
        [SerializeField] GameObject playerInventorySlotGameObject;
        [SerializeField] GameObject storageInventorySlotGameObject;
        [SerializeField] Transform playerInventoryInstantiationParent;
        [SerializeField] Transform playerStorageInstantiationParent;
        private UI_StorageInventorySlot[] playerInventory;
        private UI_StorageInventorySlot[] playerStorage;
        [SerializeField] Item currentlySelectedItem;

        [Header("Currently Selecting From")]
        public bool isSelectingFromPlayerInventory = true;

        [Header("Titles")]
        public TextMeshProUGUI playerInventoryCurrentItemSelectedText;
        public TextMeshProUGUI playerStorageCurrentItemSelectedText;

        [Header("Categories")]
        [SerializeField] TextMeshProUGUI categoryNameText;
        public ItemType itemCategory;
        public UI_InventoryCategorySelectSlot[] storageCategories;
        [HideInInspector] public int storageCategoriesIndex;

        [Header("InventorySelectionAmountMenu")]
        [SerializeField] GameObject inventorySelectionAmountMenuWindow;
        [SerializeField] Slider inventorySelectionAmountSlider;
        [SerializeField] TextMeshProUGUI inventorySelectionAmountText;
        [SerializeField] GameObject closeSubmenuWindow;

        public override void OpenMenu()
        {
            base.OpenMenu();

            RefreshStorage();
            SelectFirstButton();
            storageCategories[storageCategoriesIndex].DisplayInventoryBasedOnItemType();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
            closeSubmenuWindow.SetActive(false);
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();

            RefreshStorage();
            SelectFirstButton();
            storageCategories[storageCategoriesIndex].DisplayInventoryBasedOnItemType();
        }

        private void PopulateInventory()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            //creates inventory slot for each item the shop keeper has
            for (int i = playerInventoryInstantiationParent.transform.childCount; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                Instantiate(playerInventorySlotGameObject, playerInventoryInstantiationParent);
            }

            //get the inventory items
            playerInventory = playerInventoryInstantiationParent.GetComponentsInChildren<UI_StorageInventorySlot>(true);

            //deselects all buttons
            DeselectAllButtons();

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

                playerInventory[i].CurrentItemAmountText.enabled = false;
                if (playerInventory[i].currentItem.maxItemAmount > 1)
                {
                    playerInventory[i].CurrentItemAmountText.text = "x" + playerInventory[i].currentItem.currentItemAmount;
                    playerInventory[i].CurrentItemAmountText.enabled = true;
                }
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

        private void PopulateStorage()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            //creates inventory slot for each item the shop keeper has
            for (int i = playerStorageInstantiationParent.transform.childCount; i < player.playerInventoryManager.itemsInStorage.Count; i++)
            {
                Instantiate(storageInventorySlotGameObject, playerStorageInstantiationParent);
            }

            //get the inventory items
            playerStorage = playerStorageInstantiationParent.GetComponentsInChildren<UI_StorageInventorySlot>(true);

            //deselects all buttons
            DeselectAllButtons();

            // resets any old items from previous shops
            for (int i = 0; i < playerStorage.Length; i++)
            {
                if (playerStorage[i] == null)
                    continue;

                playerStorage[i].ClearItem();

            }

            //enables all shop slots
            for (int i = 0; i < playerStorage.Length; i++)
            {
                if (playerStorage[i] == null)
                    continue;

                playerStorage[i].gameObject.SetActive(true);
            }



            for (int i = 0; i < player.playerInventoryManager.itemsInStorage.Count; i++)
            {
                if (player.playerInventoryManager.itemsInStorage[i] == null)
                    continue;

                playerStorage[i].AddItem(player.playerInventoryManager.itemsInStorage[i]);

                playerStorage[i].CurrentItemAmountText.enabled = false;
                if (playerStorage[i].currentItem.maxItemAmount > 1)
                {
                    playerStorage[i].CurrentItemAmountText.text = "x" + playerStorage[i].currentItem.currentItemAmount;
                    playerStorage[i].CurrentItemAmountText.enabled = true;
                }
            }

            //disables any empty shop slots
            for (int i = 0; i < playerStorage.Length; i++)
            {
                if (playerStorage[i] == null)
                    continue;

                if (playerStorage[i].currentItem == null)
                    playerStorage[i].gameObject.SetActive(false);
            }
        }

        public void RefreshStorage()
        {
            PopulateInventory();
            PopulateStorage();
        }

        public void SelectFirstButton()
        {
            bool hasFirstInventorySlotSelected = false;

            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i].currentItem == null)
                    continue;

                if (!playerInventory[i].gameObject.activeInHierarchy)
                    continue;

                if (!hasFirstInventorySlotSelected)
                {
                    hasFirstInventorySlotSelected = true;
                    Button buttonToSelect = playerInventory[i].gameObject.GetComponent<Button>();
                    buttonToSelect.Select();
                    playerInventory[i].SelectSlot();
                    break;
                }
            }

            if (!hasFirstInventorySlotSelected)
            {
                for (int i = 0; i < playerStorage.Length; i++)
                {
                    if (playerStorage[i].currentItem == null)
                        continue;

                    if (!playerStorage[i].gameObject.activeInHierarchy)
                        continue;

                    if (!hasFirstInventorySlotSelected)
                    {
                        hasFirstInventorySlotSelected = true;
                        Button buttonToSelect = playerStorage[i].gameObject.GetComponent<Button>();
                        buttonToSelect.Select();
                        playerStorage[i].SelectSlot();
                        break;
                    }
                }
            }
        }

        public void SortStorageByCategory()
        {
            //deselects all slots
            DeselectAllButtons();

            for (int i = 0; i < playerInventory.Length; i++)
            {
                if (playerInventory[i].currentItem == null)
                    continue;

                playerInventory[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < playerStorage.Length; i++)
            {
                if (playerStorage[i].currentItem == null)
                    continue;

                playerStorage[i].gameObject.SetActive(false);
            }

            switch (itemCategory)
            {
                case ItemType.None:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Tool:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is QuickSlotItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is QuickSlotItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.CraftingMaterial:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is CraftingMaterial)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is CraftingMaterial)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.UpgradeMaterial:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is UpgradeMaterial)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is UpgradeMaterial)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.KeyItem:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is KeyItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is KeyItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Sorcery:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Sorcery)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Sorcery)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Incantation:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Incantation)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Incantation)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Pyromancy:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Pyromancy)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is SpellItem && playerInventory[i].currentItem.itemType == ItemType.Pyromancy)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.AshesOfWar:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is AshOfWar)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is AshOfWar)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.MeleeWeapon:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is MeleeWeaponItem && playerInventory[i].currentItem.itemType != ItemType.Shield)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is MeleeWeaponItem && playerInventory[i].currentItem.itemType != ItemType.Shield)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.RangedWeaponAndCatalyst:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is RangedWeaponItem || playerInventory[i].currentItem is CasterWeaponItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is RangedWeaponItem || playerInventory[i].currentItem is CasterWeaponItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.ArrowAndBolt:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is RangedProjectileItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is RangedProjectileItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Shield:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is MeleeWeaponItem && playerInventory[i].currentItem.itemType == ItemType.Shield)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is MeleeWeaponItem && playerInventory[i].currentItem.itemType == ItemType.Shield)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.HeadEquipment:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is HeadEquipmentItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is HeadEquipmentItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.ChestEquipment:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is BodyEquipmentItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is BodyEquipmentItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.ArmEquipment:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is HandEquipmentItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is HandEquipmentItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.LegEquipment:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is LegEquipmentItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is LegEquipmentItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Accessory:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is AccessoryEquipmentItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is AccessoryEquipmentItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Info:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem.itemType == ItemType.Info)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem.itemType == ItemType.Info)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ItemType.Gestures:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem.itemType == ItemType.Gestures)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem.itemType == ItemType.Gestures)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                default: break;
            }            

            SelectFirstButton();
            categoryNameText.text = itemCategory.ToString();
        }

        public void UpdateStorageCategoryIndex(bool raiseIndex)
        {
            if (raiseIndex)
            {
                storageCategoriesIndex += 1;
            }
            else
            {
                storageCategoriesIndex -= 1;
            }

            if (storageCategoriesIndex > storageCategories.Length - 1)
                storageCategoriesIndex = 0;

            if (storageCategoriesIndex < 0)
                storageCategoriesIndex = storageCategories.Length - 1;

            storageCategories[storageCategoriesIndex].DisplayInventoryBasedOnItemType();
        }

        private void DeselectAllButtons()
        {
            if (playerInventory != null)
            {
                for (int i = 0; i < playerInventory.Length; i++)
                {
                    playerInventory[i].DeselectSlot();
                }
            }

            if (playerStorage != null)
            {
                for (int i = 0; i < playerStorage.Length; i++)
                {
                    playerStorage[i].DeselectSlot();
                }
            }




        }

        public void AttemptToOpenInventorySelectionAmountMenu(UI_StorageInventorySlot itemSlot, Item item)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            currentlySelectedItem = item;
            isSelectingFromPlayerInventory = itemSlot.isSelectingFromPlayerInventory;

            closeSubmenuWindow.SetActive(true);
            OpenSubMenu(inventorySelectionAmountMenuWindow);

            RectTransform imageRectTransform = itemSlot.GetComponent<RectTransform>();
            inventorySelectionAmountMenuWindow.transform.position = new Vector3(imageRectTransform.transform.position.x + imageRectTransform.rect.width * 2, imageRectTransform.transform.position.y, inventorySelectionAmountMenuWindow.transform.position.z);


            inventorySelectionAmountSlider.value = 1;

            if (isSelectingFromPlayerInventory)
            {
                Item itemInStorage = player.playerInventoryManager.itemsInStorage.Find(x => x.itemID == item.itemID);
                if (itemInStorage == null)
                {
                    inventorySelectionAmountSlider.maxValue = item.currentItemAmount;
                } else
                {
                    int storableAmountLeftInStorage = 0;
                    storableAmountLeftInStorage = itemInStorage.maxStorageAmount - itemInStorage.currentItemAmount;
                    if(storableAmountLeftInStorage > item.currentItemAmount)
                    {
                        inventorySelectionAmountSlider.maxValue = item.currentItemAmount;
                    }else
                    {
                        inventorySelectionAmountSlider.maxValue = storableAmountLeftInStorage;
                    }
                }                
            }
            else
            {
                Item itemInInventory = player.playerInventoryManager.itemsInInventory.Find(x => x.itemID == item.itemID);
                if (itemInInventory == null)
                {
                    inventorySelectionAmountSlider.maxValue = item.currentItemAmount;
                }
                else
                {
                    int storableAmountLeftInInventory = 0;
                    storableAmountLeftInInventory = itemInInventory.maxItemAmount - itemInInventory.currentItemAmount;
                    if (storableAmountLeftInInventory > item.currentItemAmount)
                    {
                        inventorySelectionAmountSlider.maxValue = item.currentItemAmount;
                    }
                    else
                    {
                        inventorySelectionAmountSlider.maxValue = storableAmountLeftInInventory;
                    }
                }
            }
        }

        public void ConfirmInventorySelectionAmount()
        {
            Item item = Instantiate(currentlySelectedItem);
            item.currentItemAmount = (int)inventorySelectionAmountSlider.value;

            if (item == null)
                return;

            if (isSelectingFromPlayerInventory)
            {
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToStorage(Instantiate(item));
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.RemoveItemFromInventory(item);
            }
            else
            {
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.AddItemToInventory(item);
                PlayerUIManager.Singleton.localPlayer.playerInventoryManager.RemoveItemFromStorage(item);
            }
            PlayerUIManager.Singleton.playerUIStorageManager.RefreshStorage();
            PlayerUIManager.Singleton.playerUIStorageManager.SelectFirstButton();
            PlayerUIManager.Singleton.playerUIStorageManager.CloseSubMenu();
        }

        public void UpdateSliderValue()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            inventorySelectionAmountText.text = "x" + inventorySelectionAmountSlider.value.ToString();
        }

    }
}