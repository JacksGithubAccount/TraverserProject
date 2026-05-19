using UnityEngine;
using System.Collections.Generic;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Shop Inventory")]
    public class CharacterShop : ScriptableObject
    {
        [SerializeField] List<Item> items = new List<Item>();
        [SerializeField] List<int> itemCounts = new List<int>();
        [SerializeField] List<bool> itemInfinites = new List<bool>();


        public void GenerateCharacterInventoryFromShopItems(AICharacterManager aiCharacter)
        {
            if (items.Count != itemCounts.Count && items.Count != itemInfinites.Count)
            {
                Debug.LogError("Item list count in character shop != item capacity or inte infinite list count, please make sure all lists are equal length");
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                    continue;

                Item generatedItem = Instantiate(items[i]);
                generatedItem.currentItemAmount = itemCounts[i];
                generatedItem.isInfinite = itemInfinites[i];

                aiCharacter.aiCharacterInventoryManager.AddItemToInventory(generatedItem);
            }
        }
    }
}