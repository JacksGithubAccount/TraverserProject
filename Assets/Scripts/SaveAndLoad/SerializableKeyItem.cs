using TraverserProject;
using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableKeyItem : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;


        public KeyItem GetKeyItem()
        {
            KeyItem item = WorldItemDatabase.Singleton.GetKeyItemFromSerializedData(this);
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
