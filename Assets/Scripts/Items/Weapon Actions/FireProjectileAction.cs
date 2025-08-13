using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Fire Projtectile Action")]
    public class FireProjectileAction : WeaponItemAction
    {
        [SerializeField] ProjectileSlot projectileSlot;
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            RangedProjectileItem projectileItem = null;

            switch (projectileSlot)
            {
                case ProjectileSlot.Main:
                    projectileItem = playerPerformingAction.playerInventoryManager.mainProjectile;

                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = playerPerformingAction.playerInventoryManager.secondaryProjectile;

                    break;
                default:
                    break;
            }

            if (projectileItem == null)
                return;

            if (!playerPerformingAction.IsOwner)
                return;

            if (!playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
                {
                    playerPerformingAction.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                }
                else if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
                {
                    playerPerformingAction.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                }
            }

            if (!playerPerformingAction.playerNetworkManager.hasArrowNotched.Value)
            {
                bool canIDrawAProjectile = CanIFireThisProjectile(weaponPerformingAction, projectileItem);

                if (!canIDrawAProjectile)
                    return;

                if (projectileItem.currentAmmoAmount <= 0)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Out_Of_Ammo_01", true);
                    return;
                }
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Bow_Draw_01", true);
                playerPerformingAction.playerNetworkManager.NotifyTheServerOfDrawnProjectileServerRpc(projectileItem.itemID);
            }
        }

        private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem projectileItem)
        {
            return true;
        }

    }
}