using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_EquipmentInventorySlot : MonoBehaviour
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

        public void EquipItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            switch (PlayerUIManager.Singleton.playerUIEquipmentManager.currentSelectedEquipmentSlot)
            {
                case EquipmentType.RightWeapon01:
                    WeaponItem currentWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
                    if (currentWeapon.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(currentWeapon);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[0] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.rightHandWeaponIndex == 0)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.OpenEquipmentManagerMenu();
                    break;
                case EquipmentType.RightWeapon02:
                    //LoadWeaponInventory();
                    break;
                case EquipmentType.RightWeapon03:
                    //LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon01:
                    //LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon02:
                    //LoadWeaponInventory();
                    break;
                case EquipmentType.LeftWeapon03:
                    //LoadWeaponInventory();
                    break;
                default:
                    break;
            }
        }
    }
}