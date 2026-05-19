using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Key Item")]
    public class KeyItem : Item
    {
        [Header("Description")]
        [TextArea] public string itemEffect;
    }
}
