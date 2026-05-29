using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace TraverserProject
{
    [System.Serializable]
    public class CharacterClass
    {
        [Header("Class Information")]
        public string className;

        [Header("Class Stats")]
        public int vitality = 10;
        public int endurance = 10;
        public int mind = 10;
        public int strength = 10;
        public int dexterity = 10;
        public int intelligence = 10;
        public int faith = 10;
        public int luck = 10;

        [Header("Class Weapons")]
        public WeaponItem[] mainHandWeapons = new WeaponItem[3];
        public WeaponItem[] offHandWeapons = new WeaponItem[3];

        [Header("Class Armor")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public LegEquipmentItem legEquipment;
        public HandEquipmentItem handEquipment;
        public AccessoryEquipmentItem[] accessories = new AccessoryEquipmentItem[4];

        [Header("QuickSlotItems")]
        public QuickSlotItem[] quickSlotItems = new QuickSlotItem[3];

        [Header("Spells")]
        public  SpellItem[] spellItems = new SpellItem[3];

        [Header("Inventory")]
        public List<Item> inventory = new List<Item>();

        public void SetClass(PlayerManager player)
        {
            TitleScreenManager.Singleton.SetCharacterClass(player, vitality, endurance, mind, strength, dexterity, intelligence, faith, luck,
                mainHandWeapons, offHandWeapons, headEquipment, bodyEquipment, legEquipment, handEquipment, quickSlotItems, spellItems, accessories, inventory);
    
        }

    }
}