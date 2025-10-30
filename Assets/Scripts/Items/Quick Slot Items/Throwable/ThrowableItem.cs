using TraverserProject;
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Throwable")]
    public class ThrowableItem : QuickSlotItem
    {
        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            if (currentItemAmount < 1)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc();
            }
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (player.IsOwner)
            {
                currentItemAmount--;

                PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

            }
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
