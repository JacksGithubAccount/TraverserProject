using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableFlask : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        //[SerializeField] public int maxFlaskCharges;
        //[SerializeField] public int flaskHealAmount;

        public FlaskItem GetFlask()
        {
            FlaskItem flask = WorldItemDatabase.Singleton.GetFlaskFromSerializedData(this);
            return flask;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}