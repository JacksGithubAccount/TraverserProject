using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]

    public class OffHandMeleeAction : WeaponItemAction
    {
        [Header("Attack Animations")]
        [SerializeField] string dw_Attack_01 = "DW_Light_Attack_01";
        [SerializeField] string dw_Attack_02 = "DW_Light_Attack_02";
        [SerializeField] string dw_Light_Jump_Attack_01 = "DW_Light_Jump_Attack_01";
        [SerializeField] string dw_Light_Roll_Attack_01 = "DW_Light_Roll_Attack_01";
        [SerializeField] string dw_Light_Backstep_Attack_01 = "DW_Light_Backstep_Attack_01";
        [SerializeField] string dw_Light_Run_Attack_01 = "DW_Light_Run_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value && !playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                if (playerPerformingAction.playerInventoryManager.currentRightHandWeapon.weaponClass == playerPerformingAction.playerInventoryManager.currentLeftHandWeapon.weaponClass)
                {
                    PerformPowerStanceLeftHandAction(playerPerformingAction, weaponPerformingAction);
                    return;
                }
            }

            if (playerPerformingAction.playerCombatManager.isUsingItem)
                return;

            if (!playerPerformingAction.playerCombatManager.canBlock)
                return;

            if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
            {
                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerNetworkManager.isBlocking.Value = false;

                return;
            }

            if (playerPerformingAction.playerNetworkManager.isBlocking.Value)
                return;

            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
            }
        }

        private void PerformPowerStanceLeftHandAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                if (playerPerformingAction.isPerformingAction)
                    return;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualJumpingLightAttack01, dw_Light_Jump_Attack_01, true);


                return;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            if (playerPerformingAction.playerCombatManager.canPerformRollingAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRollingLightAttack01, dw_Light_Roll_Attack_01, true);

                return;
            }

            if (playerPerformingAction.playerCombatManager.canPerformBackstepAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;

                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualBackstepLightAttack01, dw_Light_Backstep_Attack_01, true);

                return;
            }

            if (playerPerformingAction.playerNetworkManager.isSprinting.Value)
            {
                if (playerPerformingAction.IsOwner)
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRunningLightAttack01, dw_Light_Run_Attack_01, true);

                return;
            }

            if (playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon = false;

                if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack02, dw_Attack_02, true);
                }
                else if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && !playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
            }
        }

    }
}