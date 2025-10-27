using TraverserProject;
using UnityEngine;

namespace TraverserProject
{
    public class ThrowableItem : QuickSlotItem
    {
        public override void AttemptToUseItem(PlayerManager player)
        {
            base.AttemptToUseItem(player);
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isAttacking.Value)
                return false;

            return true;
        }
    }
}
