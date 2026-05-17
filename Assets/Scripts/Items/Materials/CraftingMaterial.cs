using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Items/Materials/Crafting Material")]
    public class CraftingMaterial : Item
    {
        [Header("Description")]
        [TextArea] public string itemUsage;
        [TextArea] public string itemWhereToFind;

        public List<ItemCategory> itemCategory;
    }
}