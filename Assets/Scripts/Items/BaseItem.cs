using UnityEngine;

namespace TraverserProject
{

    public class BaseItem : ScriptableObject
    {
        [Header("ItemInformation")]
        public string itemName;
        public Sprite itemIcon;
        [TextArea] public string itemDescription;
        public int itemID;

    }
}