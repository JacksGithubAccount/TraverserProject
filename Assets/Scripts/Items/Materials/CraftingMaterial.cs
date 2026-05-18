using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Items/Materials/Crafting Material")]
    public class CraftingMaterial : Item
    {
        [Header("Description")]
        [TextArea] public string itemEffect;
        [TextArea] public string itemObtained;

        public List<ItemCategory> itemCategory;
    }
}