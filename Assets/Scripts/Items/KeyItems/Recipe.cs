using UnityEngine;
using System.Collections.Generic;


namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Recipe")]
    public class Recipe : KeyItem
    {
        [SerializeField] Recipe craftedItem;
        [SerializeField] List<Item> itemIngredients;
        [SerializeField] List<itemCategory> categoryIngrdients;
    }
}
