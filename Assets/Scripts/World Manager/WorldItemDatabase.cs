using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TraverserProject
{

    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Singleton;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();


        [Header("Items")]
        private List<BaseItem> items = new List<BaseItem>();

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
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

    }
}