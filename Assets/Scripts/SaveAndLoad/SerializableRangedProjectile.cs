using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableRangedProjectile : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int itemAmount;

        public RangedProjectileItem GetProjectile()
        {
            RangedProjectileItem projectile = WorldItemDatabase.Singleton.GetRangedProjectileFromSerializedData(this);
            return projectile;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }

    }
}