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

        [Header("Running Attacks")]
        [SerializeField] string running_Light_Attack_01 = "Main_Run_Light_Attack_01";

        [Header("Rolling Attacks")]
        [SerializeField] string rolling_Light_Attack_01 = "Main_Roll_Light_Attack_01";

        [Header("Backstep Attacks")]
        [SerializeField] string backstep_Light_Attack_01 = "Main_Backstep_Light_Attack_01";

        //twohand
        [Header("Light Attacks")]
        [SerializeField] string twoh_light_Attack_01 = "2H_Light_Attack_01";
        [SerializeField] string twoh_light_Attack_02 = "2H_Light_Attack_02";

        [Header("Running Attacks")]
        [SerializeField] string twoh_running_Light_Attack_01 = "2H_Run_Light_Attack_01";

        [Header("Rolling Attacks")]
        [SerializeField] string twoh_rolling_Light_Attack_01 = "2H_Roll_Light_Attack_01";

        [Header("Backstep Attacks")]
        [SerializeField] string twoh_backstep_Light_Attack_01 = "2H_Backstep_Light_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (!playerPerformingAction.characterLocomotionManager.isGrounded)
                return;

            if (playerPerformingAction.IsOwner)
                playerPerformingAction.playerNetworkManager.isAttacking.Value = true;


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

            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            PerformLightAttack(playerPerformingAction, weaponPerformingAction);


        }

        public void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if(playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandLightAttack(playerPerformingAction, weaponPerformingAction);
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
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
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
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, twoh_light_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, twoh_light_Attack_01, true);
            }
        }

        public void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningLightAttack01, twoh_running_Light_Attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningLightAttack01, running_Light_Attack_01, true);
            }
        }

        public void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingLightAttack01, twoh_rolling_Light_Attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingLightAttack01, rolling_Light_Attack_01, true);

            }

        }

        public void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepLightAttack01, twoh_backstep_Light_Attack_01, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepLightAttack01, backstep_Light_Attack_01, true);
            }
        }
    }
}