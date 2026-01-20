using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        //main hand
        [Header("Light Attacks")]
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";
        [SerializeField] string light_Jumping_Attack_01 = "Main_Jumping_Light_Attack_01";

        [SerializeField] float lAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 lAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 lAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float lAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 lAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 lAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float jumpLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Running Attacks")]
        [SerializeField] string running_Light_Attack_01 = "Main_Run_Light_Attack_01";
        [SerializeField] float runLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 runLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 runLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Rolling Attacks")]
        [SerializeField] string rolling_Light_Attack_01 = "Main_Roll_Light_Attack_01";
        [SerializeField] float rollLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rollLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rollLAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Backstep Attacks")]
        [SerializeField] string backstep_Light_Attack_01 = "Main_Backstep_Light_Attack_01";
        [SerializeField] float bsLAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 bsLAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 bsLAtk01VFXRotation = new Vector3(-35, -85, -40);

        //twohand
        [Header("Light Attacks")]
        [SerializeField] string twoh_light_Attack_01 = "2H_Light_Attack_01";
        [SerializeField] string twoh_light_Attack_02 = "2H_Light_Attack_02";
        [SerializeField] string twoh_light_Jumping_Attack_01 = "2H_Jumping_Light_Attack_01";

        [SerializeField] float twohAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 twohAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 twohAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float twohAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 twohAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 twohAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float jumpHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpTwoHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpTwoHAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Running Attacks")]
        [SerializeField] string twoh_running_Light_Attack_01 = "2H_Run_Light_Attack_01";
        [SerializeField] float runTwoHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 runTwoHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 runTwoHAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Rolling Attacks")]
        [SerializeField] string twoh_rolling_Light_Attack_01 = "2H_Roll_Light_Attack_01";
        [SerializeField] float rollTwoHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 rollTwoHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 rollTwoHAtk01VFXRotation = new Vector3(-35, -85, -40);

        [Header("Backstep Attacks")]
        [SerializeField] string twoh_backstep_Light_Attack_01 = "2H_Backstep_Light_Attack_01";
        [SerializeField] float bsTwoHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 bsTwoHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 bsTwoHAtk01VFXRotation = new Vector3(-35, -85, -40);

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerCombatManager.isUsingItem)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.characterLocomotionManager.isGrounded)
            {
                PerformJumpingLightAttack(playerPerformingAction, weaponPerformingAction);
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;

            if (playerPerformingAction.characterNetworkManager.isSprinting.Value)
            {
                PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            if (playerPerformingAction.characterCombatManager.canPerformRollingAttack)
            {
                PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            if (playerPerformingAction.characterCombatManager.canPerformBackstepAttack)
            {
                PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            playerPerformingAction.characterCombatManager.AttemptCriticalAttack();

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);


        }

        public void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandLightAttack(playerPerformingAction, weaponPerformingAction);
            }

        }

        public void PerformJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandJumpingLightAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandJumpingLightAttack(playerPerformingAction, weaponPerformingAction);
            }

        }

        private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, lAtk02VFXstartDelay, lAtk02VFXPosition, lAtk02VFXRotation);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, lAtk01VFXstartDelay, lAtk01VFXPosition, lAtk01VFXRotation);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, lAtk01VFXstartDelay, lAtk01VFXPosition, lAtk01VFXRotation);
            }
        }

        private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == twoh_light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, twoh_light_Attack_02, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk02VFXstartDelay, twohAtk02VFXPosition, twohAtk02VFXRotation);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, twoh_light_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk01VFXstartDelay, twohAtk01VFXPosition, twohAtk01VFXRotation);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, twoh_light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk01VFXstartDelay, twohAtk01VFXPosition, twohAtk01VFXRotation);
            }
        }

        public void PerformMainHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
                return;

            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightJumpingAttack01, light_Jumping_Attack_01, true);
            PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpLAtk01VFXstartDelay, jumpLAtk01VFXPosition, jumpLAtk01VFXRotation);

        }

        public void PerformTwoHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
                return;

            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightJumpingAttack01, twoh_light_Jumping_Attack_01, true);
            PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpHAtk01VFXstartDelay, jumpTwoHAtk01VFXPosition, jumpTwoHAtk01VFXRotation);

        }

        public void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningLightAttack01, twoh_running_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, runTwoHAtk01VFXstartDelay, runTwoHAtk01VFXPosition, runTwoHAtk01VFXRotation);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningLightAttack01, running_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, runLAtk01VFXstartDelay, runLAtk01VFXPosition, runLAtk01VFXRotation);
            }
        }

        public void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingLightAttack01, twoh_rolling_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rollTwoHAtk01VFXstartDelay, rollTwoHAtk01VFXPosition, rollTwoHAtk01VFXRotation);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingLightAttack01, rolling_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, rollLAtk01VFXstartDelay, rollLAtk01VFXPosition, rollLAtk01VFXRotation);
            }

        }

        public void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepLightAttack01, twoh_backstep_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, bsTwoHAtk01VFXstartDelay, bsTwoHAtk01VFXPosition, bsTwoHAtk01VFXRotation);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepLightAttack01, backstep_Light_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, bsLAtk01VFXstartDelay, bsLAtk01VFXPosition, bsLAtk01VFXRotation);
            }
        }
    }
}