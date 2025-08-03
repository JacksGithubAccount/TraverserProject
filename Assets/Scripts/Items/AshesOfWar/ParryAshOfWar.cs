using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Ash Of War/Parry")]
    public class ParryAshOfWar : AshOfWar
    {

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction);

            if (!CanIUseThisAbility(playerPerformingAction))
            {
                return;
            }

            DeductStaminaCost(playerPerformingAction);
            DeductFocusPointCost(playerPerformingAction);
            PerformParryTypeBasedOnWeapon(playerPerformingAction);
        }

        public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
            {
                Debug.Log("Cannot perform ash of war: Is alreadying performing an action");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
            {
                Debug.Log("Cannot perform ash of war: Is jumping");
                return false;
            }

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                Debug.Log("Cannot perform ash of war: Is not grounded");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                Debug.Log("Cannot perform ash of war: out of stamina");
                return false;
            }

            return true;

        }

        private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
        {
            WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;

            switch (weaponBeingUsed.weaponClass)
            {
                case WeaponClass.StraightSword:

                    break;
                case WeaponClass.Spear:

                    break;
                case WeaponClass.MediumShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Slow_Parry_01", true);
                    break;
                case WeaponClass.Fist:

                    break;
                case WeaponClass.Axe:

                    break;
                case WeaponClass.LightShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Fast_Parry_01", true);
                    break;
                default:
                    break;
            }
        }
    }
}