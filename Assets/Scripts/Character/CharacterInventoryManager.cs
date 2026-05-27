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
            if (item == null)
                return;

            bool isStackable = false;

            if (item.maxItemAmount > 1)
                isStackable = true;

            Item itemInInventory = itemsInInventory.Find(x => x.itemID == item.itemID);

            if (isStackable)
            {
                for (int i = itemsInInventory.Count - 1; i > -1; i--)
                {
                    if (itemsInInventory[i].itemID == item.itemID)
                    {
                        itemsInInventory[i].currentItemAmount -= item.currentItemAmount;

                        if (itemsInInventory[i].currentItemAmount <= 0)
                            itemsInInventory.Remove(itemInInventory);
                    }
                }
            }
            else
            {
                itemsInInventory.Remove(itemInInventory);
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