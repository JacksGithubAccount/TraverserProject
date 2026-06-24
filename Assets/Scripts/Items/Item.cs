using UnityEngine;

namespace TraverserProject
{

    public class Item : ScriptableObject
    {
        [Header("Name")]
        public string itemName;

        [Header("Icon")]
        public Sprite itemIcon;

        [Header("Capacity")]
        //decides if item is stackable or not
        public int maxItemAmount = 1;
        public int currentItemAmount = 1;
        public int maxStorageAmount = 1;
        public bool isInfinite = false;

        [Header("Value")]
        public int itemValue = 1;

        [Header("Description")]
        [TextArea] public string itemDescription;

        [Header("Item ID")]
        public int itemID;

        [Header("Item Type")]
        public ItemType itemType = ItemType.None;

    }
}