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
        public TextMeshProUGUI shopTitle;
        public TextMeshProUGUI currentHighlightedItem;
        public TextMeshProUGUI currentItemPrice;

        public ShopBuyOrSell buyingOrSelling;
        public ShopCategory shopCategory;
        public UI_ShopItemCategory[] shopCategories;
        [HideInInspector] public int shopCategoriesIndex;

        public override void OpenMenu()
        {
            base.OpenMenu();

            shopCategoriesIndex = 0;
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();

            shopCategoriesIndex = 0;
        }

        public void OpenBuyMenu()
        {
            buyingOrSelling = ShopBuyOrSell.Buying;
            shopTitle.text = "Purchase Item";
            OpenMenu();
            PopulateShopInventory();
        }

        public void OpenSellMenu()
        {
            buyingOrSelling = ShopBuyOrSell.Selling;
            shopTitle.text = "Sell Item";
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


            // when opening a shop, the first category auto selected should always be all items
            shopCategories[shopCategoriesIndex].SetCategory();

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


            // when opening a shop, the first category auto selected should always be all items
            shopCategories[shopCategoriesIndex].SetCategory();
        }

        public void SortShopByCategory()
        {
            //deselects all slots
            for (int i = 0; i < shopInventory.Length; i++)
            {
                shopInventory[i].DeselectSlot();
            }

            bool hasFirstInventorySlotSelected = false;

            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i].currentItem == null)
                    continue;

                shopInventory[i].gameObject.SetActive(false);
            }

            switch (shopCategory)
            {
                case ShopCategory.AllItems:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Tool:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is QuickSlotItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.CraftingMaterial:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is CraftingMaterial)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.UpgradeMaterial:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is UpgradeMaterial)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.KeyItem:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is KeyItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Spells:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is SpellItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.AshesOfWar:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is AshOfWar)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Weapons:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is WeaponItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.ArrowAndBolt:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is RangedProjectileItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Armor:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is ArmorItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Accessory:
                    for (int i = 0; i < shopInventory.Length; i++)
                    {
                        if (shopInventory[i].currentItem == null)
                            continue;

                        if (shopInventory[i].currentItem is AccessoryEquipmentItem)
                            shopInventory[i].gameObject.SetActive(true);
                    }
                    break;
                case ShopCategory.Info:
                    break;
                default: break;
            }

            for (int i = 0; i < shopInventory.Length; i++)
            {
                if (shopInventory[i].currentItem == null)
                    continue;

                if (!shopInventory[i].gameObject.activeInHierarchy)
                    continue;

                if (!hasFirstInventorySlotSelected)
                {
                    hasFirstInventorySlotSelected = true;
                    Button buttonToSelect = shopInventory[i].gameObject.GetComponent<Button>();
                    buttonToSelect.Select();
                    shopInventory[i].SelectSlot();
                    break;
                }
            }
        }

        public void UpdateShopCategoryIndex(bool raiseIndex)
        {
            if (raiseIndex)
            {
                shopCategoriesIndex += 1;
            }
            else
            {
                shopCategoriesIndex -= 1;
            }

            if (shopCategoriesIndex > shopCategories.Length - 1)
                shopCategoriesIndex = 0;

            if (shopCategoriesIndex < 0)
                shopCategoriesIndex = shopCategories.Length - 1;

            shopCategories[shopCategoriesIndex].SetCategory();
        }

    }
}