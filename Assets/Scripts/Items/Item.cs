using UnityEngine;

namespace TraverserProject
{

    public class Item : ScriptableObject
    {
        [Header("ItemInformation")]
        public string itemName;
        public Sprite itemIcon;

        //decides if item is stackable or not
        public int maxItemAmount = 1;
        public int currentItemAmount = 1;

        [TextArea] public string itemDescription;
        public int itemID;
        public ItemType itemType = ItemType.None;

    }
}