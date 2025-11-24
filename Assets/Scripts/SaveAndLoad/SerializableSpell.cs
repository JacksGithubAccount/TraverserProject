using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableSpell : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;


        public SpellItem GetSpell()
        {
            SpellItem spell = WorldItemDatabase.Singleton.GetSpellFromSerializedData(this);
            return spell;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}