
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Bubbles Item")]
    public class BubblesItem : QuickSlotItem
    {
        [Header("Bubbles Gain On Use")]
        public int bubblesGainOnUse;

        protected GameObject onUseVFX;


        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);
            player.playerStatsManager.AddBubbles(bubblesGainOnUse * numberOfItemsToUse);

            onUseVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
            onUseVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            onUseVFX.transform.root.rotation = Quaternion.identity;
        }
    }
}