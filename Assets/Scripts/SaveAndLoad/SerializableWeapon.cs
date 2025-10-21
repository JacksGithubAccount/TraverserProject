using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class SerializableWeapon : ISerializationCallbackReceiver
    {
        [SerializeField] public int itemID;
        [SerializeField] public int upgradeLevel;
        [SerializeField] public int ashofWarID;

        public WeaponItem GetWeapon()
        {
            WeaponItem weapon = WorldItemDatabase.Singleton.GetWeaponFromSerializedData(this);
            return weapon;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {

        }


    }
}