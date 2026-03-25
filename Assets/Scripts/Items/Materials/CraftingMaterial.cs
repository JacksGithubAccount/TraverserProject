using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Items/Materials/Crafting Material")]
    public class CraftingMaterial : Item
    {
        public List<ItemCategory> itemCategory;
    }
}