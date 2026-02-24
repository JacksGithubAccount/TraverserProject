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

        [SerializeField] float leftLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 leftLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 leftLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float leftLAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 leftLAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 leftLAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float rightLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rightLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rightLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float rightLAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rightLAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rightLAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float jumpLeftLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpLeftLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpLeftLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float jumpRightLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpRightLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpRightLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [SerializeField] float rollLeftLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rollLeftLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rollLeftLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float rollRightLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rollRightLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rollRightLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [SerializeField] float runLeftLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 runLeftLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 runLeftLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float runRightLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 runRightLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 runRightLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [SerializeField] float bsLeftLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 bsLeftLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 bsLeftLAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float bsRightLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 bsRightLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 bsRightLAtk01VFXRotation = new Vector3(-35, -85, -40);

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
                playerPerformingAction.playerNetworkManager.isSneaking.Value = false;
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
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualJumpingLightAttack01, dw_Light_Jump_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpLeftLAtk01VFXstartDelay, jumpLeftLAtk01VFXPosition, jumpLeftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpRightLAtk01VFXstartDelay, jumpRightLAtk01VFXPosition, jumpRightLAtk01VFXRotation);
                }
                return;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            if (playerPerformingAction.playerCombatManager.canPerformRollingAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;

                if (playerPerformingAction.IsOwner)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRollingLightAttack01, dw_Light_Roll_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rollLeftLAtk01VFXstartDelay, rollLeftLAtk01VFXPosition, rollLeftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rollRightLAtk01VFXstartDelay, rollRightLAtk01VFXPosition, rollRightLAtk01VFXRotation);
                }
                return;
            }

            if (playerPerformingAction.playerCombatManager.canPerformBackstepAttack)
            {
                playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;

                if (playerPerformingAction.IsOwner)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualBackstepLightAttack01, dw_Light_Backstep_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, bsLeftLAtk01VFXstartDelay, bsLeftLAtk01VFXPosition, bsLeftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, bsRightLAtk01VFXstartDelay, bsRightLAtk01VFXPosition, bsRightLAtk01VFXRotation);
                }
                return;
            }

            if (playerPerformingAction.playerNetworkManager.isSprinting.Value)
            {
                if (playerPerformingAction.IsOwner)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualRunningLightAttack01, dw_Light_Run_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, runLeftLAtk01VFXstartDelay, runLeftLAtk01VFXPosition, runLeftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, runRightLAtk01VFXstartDelay, runRightLAtk01VFXPosition, runRightLAtk01VFXRotation);
                }
                return;
            }

            if (playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon = false;

                if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack02, dw_Attack_02, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, leftLAtk02VFXstartDelay, leftLAtk02VFXPosition, leftLAtk02VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rightLAtk02VFXstartDelay, rightLAtk02VFXPosition, rightLAtk02VFXRotation);
                }
                else if (playerPerformingAction.playerCombatManager.lastAttackAnimationPerformed == dw_Attack_02)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, leftLAtk01VFXstartDelay, leftLAtk01VFXPosition, leftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rightLAtk01VFXstartDelay, rightLAtk01VFXPosition, rightLAtk01VFXRotation);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, leftLAtk01VFXstartDelay, leftLAtk01VFXPosition, leftLAtk01VFXRotation);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rightLAtk01VFXstartDelay, rightLAtk01VFXPosition, rightLAtk01VFXRotation);
                }
            }
            else if (!playerPerformingAction.playerCombatManager.canComboWithOffHandWeapon && !playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.DualLightAttack01, dw_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, leftLAtk01VFXstartDelay, leftLAtk01VFXPosition, leftLAtk01VFXRotation);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rightLAtk01VFXstartDelay, rightLAtk01VFXPosition, rightLAtk01VFXRotation);
            }
        }

    }
}