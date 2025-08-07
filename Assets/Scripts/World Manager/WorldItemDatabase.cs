using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TraverserProject
{

    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Singleton;

        public WeaponItem unarmedWeapon;

        public GameObject creatureDropPickUpItemPrefab;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        [Header("Head equipment")]
        [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();

        [Header("Body equipment")]
        [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();

        [Header("Hand equipment")]
        [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

        [Header("Leg equipment")]
        [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();

        [Header("Ashes of War")]
        [SerializeField] List<AshOfWar> ashesOfWar = new List<AshOfWar>();

        [Header("Spells")]
        [SerializeField] List<SpellItem> spells = new List<SpellItem>();

        [Header("Items")]
        private List<Item> items = new List<Item>();

        [Header("Item ID Prefix Keys")]
        [SerializeField] int weaponItemKey = 1000;
        [SerializeField] int headItemKey = 2000;
        [SerializeField] int bodyItemKey = 3000;
        [SerializeField] int handItemKey = 4000;
        [SerializeField] int legItemKey = 5000;
        [SerializeField] int ashOfWarItemKey = 6000;
        [SerializeField] int spellItemKey = 7000;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }

            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            foreach (var item in headEquipment)
            {
                items.Add(item);
            }

            foreach (var item in bodyEquipment)
            {
                items.Add(item);
            }

            foreach (var item in handEquipment)
            {
                items.Add(item);
            }

            foreach (var item in legEquipment)
            {
                items.Add(item);
            }

            foreach (var item in ashesOfWar)
            {
                items.Add(item);
            }

            foreach (var item in spells)
            {
                items.Add(item);
            }

            for (int i = 0; i < items.Count; i++)
            {
                int prefixKey = 0;

                if (items[i].GetType() == typeof(WeaponItem))
                    prefixKey = weaponItemKey;
                else if (items[i].GetType() == typeof(HeadEquipmentItem))
                    prefixKey = headItemKey;
                else if (items[i].GetType() == typeof(BodyEquipmentItem))
                    prefixKey = bodyItemKey;
                else if (items[i].GetType() == typeof(HandEquipmentItem))
                    prefixKey = handItemKey;
                else if (items[i].GetType() == typeof(LegEquipmentItem))
                    prefixKey = legItemKey;
                else if (items[i].GetType() == typeof(AshOfWar))
                    prefixKey = ashOfWarItemKey;
                else if (items[i].GetType() == typeof(SpellItem))
                    prefixKey = spellItemKey;

                items[i].itemID = prefixKey + i;
            }
        }

        public Item GetItemByID(int ID)
        {
            return items.FirstOrDefault(item => item.itemID == ID);
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

        public HeadEquipmentItem GetHeadEquipmentByID(int ID)
        {
            return headEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public BodyEquipmentItem GetBodyEquipmentByID(int ID)
        {
            return bodyEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public HandEquipmentItem GetHandEquipmentByID(int ID)
        {
            return handEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public LegEquipmentItem GetLegEquipmentByID(int ID)
        {
            return legEquipment.FirstOrDefault(equipment => equipment.itemID == ID);
        }

        public AshOfWar GetAshOfWarByID(int ID)
        {
            return ashesOfWar.FirstOrDefault(item => item.itemID == ID);
        }

        public SpellItem GetSpellByID(int ID)
        {
            return spells.FirstOrDefault(item => item.itemID == ID);
        }

    }
}