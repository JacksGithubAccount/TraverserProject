using UnityEngine;
using System.Collections.Generic;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableShopInventory : ISerializationCallbackReceiver
    {
        [SerializeField] public List<int> itemIDs = new List<int>();
        [SerializeField] public List<int> itemAmounts = new List<int>();
        [SerializeField] public List<bool> itemInfinites = new List<bool>();

        public List<Item> GetItems()
        {
            List<Item> items = WorldItemDatabase.Singleton.GetShopItemsFromSerializedData(this);
            return items;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}