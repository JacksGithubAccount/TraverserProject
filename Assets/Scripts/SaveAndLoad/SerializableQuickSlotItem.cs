using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableQuickSlotItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;


        public QuickSlotItem GetQuickSlotItem()
        {
            QuickSlotItem item = WorldItemDatabase.Singleton.GetQuickSlotItemFromSerializedData(this);
            return item;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}