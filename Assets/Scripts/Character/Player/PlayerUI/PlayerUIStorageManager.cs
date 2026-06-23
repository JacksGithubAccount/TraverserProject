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

        [Header("Currently Selecting From")]
        public bool isSelectingFromPlayerInventory = true;

        [Header("Titles")]
        public TextMeshProUGUI playerInventoryCurrentItemSelectedText;
        public TextMeshProUGUI playerStorageCurrentItemSelectedText;

        [Header("Categories")]
        public ShopCategory shopCategory;
        public UI_ShopItemCategory[] storageCategories;
        [HideInInspector] public int storageCategoriesIndex;

        public override void OpenMenu()
        {
            base.OpenMenu();

            RefreshStorage();
            SelectFirstButton();
            storageCategories[storageCategoriesIndex].SetCategory();
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();

            RefreshStorage();
            SelectFirstButton();
            storageCategories[storageCategoriesIndex].SetCategory();
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

            switch (shopCategory)
            {
                case ShopCategory.AllItems:
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
                case ShopCategory.Tool:
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
                case ShopCategory.CraftingMaterial:
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
                case ShopCategory.UpgradeMaterial:
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
                case ShopCategory.KeyItem:
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
                case ShopCategory.Spells:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is SpellItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is SpellItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.AshesOfWar:
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
                case ShopCategory.Weapons:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is WeaponItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is WeaponItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.ArrowAndBolt:
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
                case ShopCategory.Armor:
                    for (int i = 0; i < playerInventory.Length; i++)
                    {
                        if (playerInventory[i].currentItem == null)
                            continue;

                        if (playerInventory[i].currentItem is ArmorItem)
                            playerInventory[i].gameObject.SetActive(true);
                    }
                    for (int i = 0; i < playerStorage.Length; i++)
                    {
                        if (playerStorage[i].currentItem == null)
                            continue;

                        if (playerStorage[i].currentItem is ArmorItem)
                            playerStorage[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Accessory:
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
                case ShopCategory.Info:
                    break;
                default: break;
            }

            SelectFirstButton();
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

            storageCategories[storageCategoriesIndex].SetCategory();
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

    }
}