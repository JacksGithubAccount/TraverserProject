using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_EquipmentInventorySlot : UI_InventorySlot
    {
        public Image greyedOutIcon;
        public bool incompatableEquipment = false;

        private void Awake()
        {
            highlightIcon.enabled = false;
            greyedOutIcon.enabled = false;
        }
        
        public void EquipItem()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            Item equippedItem;

            if(incompatableEquipment)
            {
                PlayerUIManager.Singleton.playerUIEquipmentManager.OpenSubMenu(PlayerUIManager.Singleton.playerUIEquipmentManager.IncompatableEquipmentWindow);
                return;
            }

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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInRightHandSlots[0]);
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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInRightHandSlots[1]);
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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInRightHandSlots[2]);
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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInLeftHandSlots[0]);
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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInLeftHandSlots[1]);
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

                    player.playerStatsManager.CalculateWeaponAttackPower(player.playerInventoryManager.weaponsInLeftHandSlots[2]);
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
                case EquipmentType.QuickSlot01:
                    equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[0];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = currentItem as QuickSlotItem;
                    int itemCount0 = 1;
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[0].isConsumable)
                    {
                        itemCount0 = player.playerInventoryManager.quickSlotItemsInQuickSlots[0].currentItemAmount;
                    }
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[0].currentItemAmount = itemCount0;

                    if (player.playerInventoryManager.quickSlotItemIndex == 0)
                        player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.QuickSlot02:
                    equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[1];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = currentItem as QuickSlotItem;
                    int itemCount1 = 1;
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[1].isConsumable)
                    {
                        itemCount1 = player.playerInventoryManager.quickSlotItemsInQuickSlots[1].currentItemAmount;
                    }
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[1].currentItemAmount = itemCount1;

                    if (player.playerInventoryManager.quickSlotItemIndex == 1)
                        player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.QuickSlot03:
                    equippedItem = player.playerInventoryManager.quickSlotItemsInQuickSlots[2];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                    }
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = currentItem as QuickSlotItem;
                    int itemCount2 = 1;
                    if (player.playerInventoryManager.quickSlotItemsInQuickSlots[2].isConsumable)
                    {
                        itemCount2 = player.playerInventoryManager.quickSlotItemsInQuickSlots[2].currentItemAmount;
                    }
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[2].currentItemAmount = itemCount2;

                    if (player.playerInventoryManager.quickSlotItemIndex == 2)
                        player.playerNetworkManager.currentQuickSlotItemID.Value = currentItem.itemID;

                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();
                    break;
                case EquipmentType.Accessory01:                  
                    equippedItem = player.playerInventoryManager.accessoryEquipment[0];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                        StaticCharacterEffect equippedAccessoryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(equippedItem as AccessoryEquipmentItem));
                        player.playerEffectsManager.RemoveStaticEffect(equippedAccessoryEffect.staticEffectID);
                    }
                    player.playerInventoryManager.accessoryEquipment[0] = currentItem as AccessoryEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();

                    player.playerEquipmentManager.LoadAccessoryEquipment(player.playerInventoryManager.accessoryEquipment[0], 1);

                    StaticCharacterEffect accessoryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(player.playerInventoryManager.accessoryEquipment[0]));
                    player.playerEffectsManager.AddStaticEffect(accessoryEffect);
                    break;
                case EquipmentType.Accessory02:
                    equippedItem = player.playerInventoryManager.accessoryEquipment[1];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                        StaticCharacterEffect equippedAccessoryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(equippedItem as AccessoryEquipmentItem));
                        player.playerEffectsManager.RemoveStaticEffect(equippedAccessoryEffect.staticEffectID);
                    }
                    player.playerInventoryManager.accessoryEquipment[1] = currentItem as AccessoryEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();

                    player.playerEquipmentManager.LoadAccessoryEquipment(player.playerInventoryManager.accessoryEquipment[1], 2);

                    StaticCharacterEffect accessoryEffect2 = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(player.playerInventoryManager.accessoryEquipment[1]));
                    player.playerEffectsManager.AddStaticEffect(accessoryEffect2);
                    break;
                case EquipmentType.Accessory03:
                    equippedItem = player.playerInventoryManager.accessoryEquipment[2];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                        StaticCharacterEffect equippedAccessoryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(equippedItem as AccessoryEquipmentItem));
                        player.playerEffectsManager.RemoveStaticEffect(equippedAccessoryEffect.staticEffectID);
                    }
                    player.playerInventoryManager.accessoryEquipment[2] = currentItem as AccessoryEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();

                    player.playerEquipmentManager.LoadAccessoryEquipment(player.playerInventoryManager.accessoryEquipment[2], 3);

                    StaticCharacterEffect accessoryEffect3 = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(player.playerInventoryManager.accessoryEquipment[2]));
                    player.playerEffectsManager.AddStaticEffect(accessoryEffect3);
                    break;
                case EquipmentType.Accessory04:
                    equippedItem = player.playerInventoryManager.accessoryEquipment[3];
                    if (equippedItem != null)
                    {
                        player.playerInventoryManager.AddItemToInventory(equippedItem);
                        StaticCharacterEffect equippedAccessoryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(equippedItem as AccessoryEquipmentItem));
                        player.playerEffectsManager.RemoveStaticEffect(equippedAccessoryEffect.staticEffectID);
                    }
                    player.playerInventoryManager.accessoryEquipment[3] = currentItem as AccessoryEquipmentItem;
                    player.playerInventoryManager.RemoveItemFromInventory(currentItem);
                    PlayerUIManager.Singleton.playerUIEquipmentManager.RefreshMenu();

                    player.playerEquipmentManager.LoadAccessoryEquipment(player.playerInventoryManager.accessoryEquipment[3], 4);

                    StaticCharacterEffect accessoryEffect4 = Instantiate(WorldCharacterEffectsManager.Singleton.RetrieveAccessoryStaticEffect(player.playerInventoryManager.accessoryEquipment[3]));
                    player.playerEffectsManager.AddStaticEffect(accessoryEffect4);
                    break;
                default:
                    break;
            }

            PlayerUIManager.Singleton.playerUIEquipmentManager.SelectLastSelectedEquipmentSlot();
            PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);
        }

    }
}