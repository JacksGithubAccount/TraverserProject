using UnityEngine;
using System.Collections.Generic;


namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Recipe")]
    public class Recipe : KeyItem
    {
        public Item craftedItem;
        public int craftedItemAmount;
        public List<Item> itemIngredients;
        public List<int> itemIngredientsAmount;
        public List<ItemCategory> itemCategoryIngredients;
        public List<int> itemCategoryIngredientsAmount;
    }
}
