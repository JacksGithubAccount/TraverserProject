using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_ItemCategoryIngredientSelectionSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        [SerializeField] public Item currentItem;

        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void SelectItem()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            Item equippedItem;

            switch (PlayerUIManager.Singleton.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[0];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
            }
    }
}