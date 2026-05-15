using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class PlayerInventoryManager : CharacterInventoryManager
    {
        PlayerManager player;

        [Header("Weapons")]
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;
        public WeaponItem currentTwoHandWeapon;

        [Header("Quick Slots")]
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;
        public SpellItem[] spellItemsInQuickSlots = new SpellItem[3];
        public int quickSlotSpellIndex = 0;
        public SpellItem currentSpell;
        public QuickSlotItem[] quickSlotItemsInQuickSlots = new QuickSlotItem[3];
        public int quickSlotItemIndex = 0;
        public QuickSlotItem currentQuickSlotItem;
        public QuickSlotItem menuSelectedQuickSlotItem;

        [Header("Armor")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public HandEquipmentItem handEquipment;
        public LegEquipmentItem legEquipment;

        [Header("Projectiles")]
        public RangedProjectileItem mainProjectile;
        public RangedProjectileItem secondaryProjectile;

        [Header("Debug")]
        [SerializeField] bool dropItem = false;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            if (dropItem)
            {
                dropItem = false;
                Item test = new Item();
                test.itemID = 28;
                test.currentItemAmount = 1;

                DropItemFromInventory(test);
            }
        }

        public bool CheckIfItemIsInInventoryOrEquipSlots(Item item)
        {
            bool isInInventoryorEquipped = false;

            isInInventoryorEquipped = itemsInInventory.Find(x => x.itemID == item.itemID);
            if (!isInInventoryorEquipped)
            {
                foreach (var qsItem in quickSlotItemsInQuickSlots)
                {
                    if (qsItem.itemID == item.itemID)
                        isInInventoryorEquipped = true;
                }
                foreach (var rhItem in weaponsInRightHandSlots)
                {
                    if (rhItem.itemID == item.itemID)
                        isInInventoryorEquipped = true;
                }
                foreach (var lhItem in weaponsInLeftHandSlots)
                {
                    if (lhItem.itemID == item.itemID)
                        isInInventoryorEquipped = true;
                }

            }

            return isInInventoryorEquipped;
        }

        public Item GetItemInInventoryOrEquipSlots(Item item)
        {
            Item itemInInventoryOrEquipped;

            itemInInventoryOrEquipped = player.playerInventoryManager.itemsInInventory.Find(x => x.itemID == item.itemID);

            if (itemInInventoryOrEquipped == null)
            {
                foreach (var qsItem in quickSlotItemsInQuickSlots)
                {
                    if (qsItem.itemID == item.itemID)
                        itemInInventoryOrEquipped = qsItem;
                }
                if (itemInInventoryOrEquipped == null)
                {
                    foreach (var rhItem in weaponsInRightHandSlots)
                    {
                        if (rhItem.itemID == item.itemID)
                            itemInInventoryOrEquipped = rhItem;
                    }

                    if (itemInInventoryOrEquipped == null)
                    {
                        foreach (var lhItem in weaponsInLeftHandSlots)
                        {
                            if (lhItem.itemID == item.itemID)
                                itemInInventoryOrEquipped = lhItem;
                        }
                    }
                }
            }

            return itemInInventoryOrEquipped;
        }

        public override void AddItemToInventory(Item item)
        {
            bool isStackable = false;


            if (item.maxItemAmount > 1)
                isStackable = true;

            if (isStackable)
            {
                bool isInQuickSlot = false;

                foreach (var qsItem in quickSlotItemsInQuickSlots)
                {
                    if (qsItem.itemID == item.itemID)
                        isInQuickSlot = true;
                }

                if (isInQuickSlot)
                {
                    foreach (var qsItem in quickSlotItemsInQuickSlots)
                    {
                        if (qsItem.itemID == item.itemID)
                        {
                            qsItem.currentItemAmount += item.currentItemAmount;

                            if (qsItem.currentItemAmount > 99)
                                qsItem.currentItemAmount = 99;
                        }
                    }
                }

                if (itemsInInventory.Find(x => x.itemID == item.itemID) && !isInQuickSlot)
                {
                    Item itemInInventory = itemsInInventory.Find(x => x.itemID == item.itemID);
                    itemInInventory.currentItemAmount = itemInInventory.currentItemAmount + item.currentItemAmount;
                    
                    if (itemInInventory.currentItemAmount > 99)
                        itemInInventory.currentItemAmount = 99;
                }
                else if (!itemsInInventory.Find(x => x.itemID == item.itemID) && !isInQuickSlot)
                {
                    itemsInInventory.Add(item);
                }
            }
            else
            {
                itemsInInventory.Add(item);
            }
        }
        public void RemoveItemFromQuickSlotOrInventory(Item item)
        {
            bool isStackable = false;

            if (item.maxItemAmount > 1)
                isStackable = true;

            if (!CheckIfItemIsInInventoryOrEquipSlots(item))
                return;

            Item itemInInventory = GetItemInInventoryOrEquipSlots(item);

            if (isStackable)
            {

                itemInInventory.currentItemAmount -= item.currentItemAmount;

                if (itemInInventory.currentItemAmount <= 0)
                {
                    itemsInInventory.Remove(itemInInventory);
                    for (int i = 0; i < quickSlotItemsInQuickSlots.Length; i++)
                    {
                        if (quickSlotItemsInQuickSlots[i].itemID == item.itemID)
                        {
                            quickSlotItemsInQuickSlots[i] = null;
                        }
                    }
                }
            }

            else
            {
                itemsInInventory.Remove(itemInInventory);
                for (int i = 0; i < quickSlotItemsInQuickSlots.Length; i++)
                {
                    if (quickSlotItemsInQuickSlots[i].itemID == item.itemID)
                    {
                        quickSlotItemsInQuickSlots[i] = null;
                    }
                }
            }
        }
        public void DropItemFromInventory(Item item)
        {

            GameObject itemDrop = Instantiate(WorldItemDatabase.Singleton.inventoryDropItemPickUpInteractable);
            PickUpItemInteractable itemInteractable = itemDrop.GetComponent<PickUpItemInteractable>();
            itemDrop.GetComponent<NetworkObject>().Spawn();            
            itemInteractable.itemID.Value = item.itemID;
            itemInteractable.networkPosition.Value = transform.position;
            itemInteractable.itemAmount = item.currentItemAmount;
            RemoveItemFromQuickSlotOrInventory(item);
        }
    }
}