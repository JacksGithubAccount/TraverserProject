using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Items/Materials/Upgrade Material")]
    public class UpgradeMaterial : Item
    {
        [Header("Description")]
        [TextArea] public string itemEffect;

        public UpgradeStone upgradeStone;


    }
}