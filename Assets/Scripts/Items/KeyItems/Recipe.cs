using UnityEngine;
using System.Collections.Generic;


namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Recipe")]
    public class Recipe : KeyItem
    {
        public Item craftedItem;
        public List<Item> itemIngredients;
        public List<int> itemIngredientsAmount;
        public List<ItemCategory> categoryIngredients;
        public List<int> categoryIngredientsAmount;
    }
}
