using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class CharacterInventoryManager : MonoBehaviour
    {
        [Header("Inventory")]
        public List<Item> itemsInInventory;

        protected virtual void Awake()
        {

        }

        public virtual void AddItemToInventory(Item item)
        {
            bool isStackable = false;

            if (item.maxItemAmount > 1)
                isStackable = true;

            if (isStackable)
            {
                if (itemsInInventory.Find(x => x.itemID == item.itemID))
                {
                    Item itemInInventory = itemsInInventory.Find(x => x.itemID == item.itemID);
                    itemInInventory.currentItemAmount += item.currentItemAmount;

                    if (itemInInventory.currentItemAmount > 99)
                        itemInInventory.currentItemAmount = 99;
                }
                else if (!itemsInInventory.Find(x => x.itemID == item.itemID))
                {
                    itemsInInventory.Add(item);
                }
            }
            else
            {
                itemsInInventory.Add(item);
            }
        }

        public virtual void RemoveItemFromInventory(Item item)
        {
            bool isStackable = false;

            if (item.maxItemAmount > 1)
                isStackable = true;

            if (isStackable)
            {
                for (int i = itemsInInventory.Count - 1; i > -1; i--)
                {
                    if (itemsInInventory[i].itemID == item.itemID)
                    {
                        itemsInInventory[i].currentItemAmount -= item.currentItemAmount;

                        if (itemsInInventory[i].currentItemAmount <= 0)
                            itemsInInventory.Remove(item);
                    }
                }
            }
            else
            {
                itemsInInventory.Remove(item);
            }

            //null checker
            for (int i = itemsInInventory.Count - 1; i > -1; i--)
            {
                if (itemsInInventory[i] == null)
                {
                    itemsInInventory.RemoveAt(i);
                }
            }
        }
    }
}