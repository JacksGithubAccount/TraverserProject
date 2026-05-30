using TraverserProject;
using UnityEngine;

public class SerializableUpgradeMaterial : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;
    [SerializeField] public int itemAmount;


    public UpgradeMaterial GetUpgradeMaterialItem()
    {
        UpgradeMaterial item = WorldItemDatabase.Singleton.GetUpgradeMaterialFromSerializedData(this);
        return item;
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
