using TraverserProject;
using UnityEngine;

public class SerializableCraftingMaterial : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;
    [SerializeField] public int itemAmount;


    public CraftingMaterial GetCraftingMaterialItem()
    {
        CraftingMaterial item = WorldItemDatabase.Singleton.GetCraftingMaterialFromSerializedData(this);
        return item;
    }

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {

    }
}
