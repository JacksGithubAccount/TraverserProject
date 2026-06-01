using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableTimedEffect : ISerializationCallbackReceiver
    {
        [SerializeField] public int effectID;
        [SerializeField] public float timeRemainingOnEffect;


        public TimedCharacterEffect GetTimedEffect()
        {
            TimedCharacterEffect effect = WorldCharacterEffectsManager.Singleton.GetTimedEffectFromSerializedData(this);
            return effect;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }
    }
}
