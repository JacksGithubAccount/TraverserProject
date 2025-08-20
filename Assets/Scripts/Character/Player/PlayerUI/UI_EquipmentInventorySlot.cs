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
                case EquipmentType.RightWeapon02:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[1];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[1] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.rightHandWeaponIndex == 1)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.RightWeapon03:
                    equippedItem = player.playerInventoryManager.weaponsInRightHandSlots[2];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInRightHandSlots[2] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.rightHandWeaponIndex == 2)
                        player.playerNetworkManager.currentRightHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon01:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[0];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[0] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.leftHandWeaponIndex == 0)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon02:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[1];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[1] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.leftHandWeaponIndex == 1)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.LeftWeapon03:
                    equippedItem = player.playerInventoryManager.weaponsInLeftHandSlots[2];
                    if (equippedItem.itemID != WorldItemDatabase.Singleton.unarmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.weaponsInLeftHandSlots[2] = currentItem as WeaponItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    if (player.playerInventoryManager.leftHandWeaponIndex == 2)
                        player.playerNetworkManager.currentLeftHandWeaponID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Head:
                    equippedItem = player.playerInventoryManager.headEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Body:
                    equippedItem = player.playerInventoryManager.bodyEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Hands:
                    equippedItem = player.playerInventoryManager.handEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Legs:
                    equippedItem = player.playerInventoryManager.legEquipment;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.MainProjectile:
                    equippedItem = player.playerInventoryManager.mainProjectile;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.mainProjectile = currentItem as RangedProjectileItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadMainProjectileEquipment(player.playerInventoryManager.mainProjectile);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.SecondaryProjectile:
                    equippedItem = player.playerInventoryManager.secondaryProjectile;
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.secondaryProjectile = currentItem as RangedProjectileItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);

                    player.playerEquipmentManager.LoadSecondaryProjectileEquipment(player.playerInventoryManager.secondaryProjectile);

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                default:
                    break;
            }

            PlayerUIManager.Singleton.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
        }

    }
}