using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]

    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        //main hand
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";

        //twohand
        [SerializeField] string twoh_heavy_Attack_01 = "2H_Heavy_Attack_01";
        [SerializeField] string twoh_heavy_Attack_02 = "2H_Heavy_Attack_02";

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

            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);


        }

        public void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
            }

        }

        public void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, heavy_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
            }
        }

        public void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == twoh_heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, twoh_heavy_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, twoh_heavy_Attack_01, true);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, twoh_heavy_Attack_01, true);
            }
        }

    }
}