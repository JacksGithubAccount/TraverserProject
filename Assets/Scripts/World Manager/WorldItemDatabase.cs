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

        [Header("Upgrade Stones")]
        public UpgradeMaterial smallUpgradeStone;
        public UpgradeMaterial mediumUpgradeStone;
        public UpgradeMaterial largeUpgradeStone;
        public UpgradeMaterial veryLargeUpgradeStone;

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

        [Header("Projectiles")]
        [SerializeField] List<RangedProjectileItem> projectiles = new List<RangedProjectileItem>();

        [Header("Quick Slot")]
        [SerializeField] List<QuickSlotItem> quickSlotItems = new List<QuickSlotItem>();

        [Header("Upgrade Materials")]
        [SerializeField] List<UpgradeMaterial> upgradeMaterials = new List<UpgradeMaterial>();

        [Header("Key Items")]
        [SerializeField] List<KeyItem> keyItems = new List<KeyItem>();

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
        [SerializeField] int projectileItemKey = 8000;
        [SerializeField] int quickSlotItemKey = 9000;
        [SerializeField] int upgradeMaterialItemKey = 10000;
        [SerializeField] int keyItemKey = 11000;

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

            foreach (var item in projectiles)
            {
                items.Add(item);
            }

            foreach (var item in upgradeMaterials)
            {
                items.Add(item);
            }

            foreach (var item in quickSlotItems)
            {
                items.Add(item);
            }

            foreach(var item in keyItems)
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
                else if (items[i].GetType() == typeof(RangedProjectileItem))
                    prefixKey = projectileItemKey;
                else if (items[i].GetType() == typeof(UpgradeMaterial))
                    prefixKey = upgradeMaterialItemKey;
                else if (items[i].GetType() == typeof(QuickSlotItem))
                    prefixKey = quickSlotItemKey;
                else if (items[i].GetType() == typeof(KeyItem))
                    prefixKey = keyItemKey;

                items[i].itemID = prefixKey + i;
            }
        }

        //item database

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

        public RangedProjectileItem GetProjectileByID(int ID)
        {
            return projectiles.FirstOrDefault(item => item.itemID == ID);
        }

        public UpgradeMaterial GetUpgradeMaterialByID(int ID)
        {
            return upgradeMaterials.FirstOrDefault(item => item.itemID == ID);
        }

        public QuickSlotItem GetQuickSlotItemByID(int ID)
        {
            return quickSlotItems.FirstOrDefault(item => item.itemID == ID);
        }

        public KeyItem GetKeyItemByID(int ID)
        {
            return keyItems.FirstOrDefault(item => item.itemID == ID);
        }

        //item serialization

        public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)
        {
            WeaponItem weapon = null;
            if (GetWeaponByID(serializableWeapon.itemID))
                weapon = Instantiate(GetWeaponByID(serializableWeapon.itemID));

            if (weapon == null)
                return Instantiate(unarmedWeapon);

            if (GetAshOfWarByID(serializableWeapon.ashofWarID))
            {
                AshOfWar ashOfWar = Instantiate(GetAshOfWarByID(serializableWeapon.ashofWarID));
                weapon.ashOfWarAction = ashOfWar;
            }

            weapon.upgradeLevel = (UpgradeLevel)serializableWeapon.upgradeLevel;

            return weapon;
        }

        public RangedProjectileItem GetRangedProjectileFromSerializedData(SerializableRangedProjectile serializableProjectile)
        {
            RangedProjectileItem projectile = null;
            if (GetProjectileByID(serializableProjectile.itemID))
            {
                projectile = Instantiate(GetProjectileByID(serializableProjectile.itemID));
                projectile.currentAmmoAmount = serializableProjectile.itemAmount;
            }

            return projectile;
        }

        public FlaskItem GetFlaskFromSerializedData(SerializableFlask serializableFlask)
        {
            FlaskItem flask = null;
            if (GetQuickSlotItemByID(serializableFlask.itemID))
            {
                flask = Instantiate(GetQuickSlotItemByID(serializableFlask.itemID)) as FlaskItem;
            }

            return flask;
        }

        public SpellItem GetSpellFromSerializedData(SerializableSpell serializableSpell)
        {
            SpellItem spellItem = null;
            if (GetSpellByID(serializableSpell.itemID))
            {
                spellItem = Instantiate(GetSpellByID(serializableSpell.itemID));
            }

            return spellItem;
        }

        public QuickSlotItem GetQuickSlotItemFromSerializedData(SerializableQuickSlotItem serializableQuickSlotItem)
        {
            QuickSlotItem quickSlotItem = null;
            if (GetQuickSlotItemByID(serializableQuickSlotItem.itemID))
            {
                quickSlotItem = Instantiate(GetQuickSlotItemByID(serializableQuickSlotItem.itemID));
                quickSlotItem.currentItemAmount = serializableQuickSlotItem.itemAmount;
            }

            return quickSlotItem;
        }

        public KeyItem GetKeyItemFromSerializedData(SerializableKeyItem serializableKeyItem)
        {
            KeyItem keyItem = null;
            if (GetQuickSlotItemByID(serializableKeyItem.itemID))
            {
                keyItem = Instantiate(GetKeyItemByID(serializableKeyItem.itemID));
                keyItem.currentItemAmount = serializableKeyItem.itemAmount;
            }

            return keyItem;
        }

    }
}