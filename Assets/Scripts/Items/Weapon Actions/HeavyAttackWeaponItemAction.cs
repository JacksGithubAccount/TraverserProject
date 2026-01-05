using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]

    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        //main hand
        [SerializeField] string heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] string heavy_Attack_02 = "Main_Heavy_Attack_02";
        [SerializeField] string heavy_Jumping_Attack_01 = "Main_Jumping_Heavy_Attack_01";

        [SerializeField] float hAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 hAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 hAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float hAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 hAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 hAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float jumpHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpHAtk01VFXRotation = new Vector3(-35, -85, -40);

        //twohand
        [SerializeField] string twoh_heavy_Attack_01 = "2H_Heavy_Attack_01";
        [SerializeField] string twoh_heavy_Attack_02 = "2H_Heavy_Attack_02";
        [SerializeField] string twoh_Heavy_Jumping_Attack_01 = "2H_Jumping_Heavy_Attack_01";

        [SerializeField] float twohAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 twohAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 twohAtk01VFXRotation = new Vector3(-35, -85, -40);
        [SerializeField] float twohAtk02VFXstartDelay = 0.5f;
        [SerializeField] Vector3 twohAtk02VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 twohAtk02VFXRotation = new Vector3(33, 96, 60);

        [SerializeField] float jumpTwoHAtk01VFXstartDelay = 0.5f;
        [SerializeField] Vector3 jumpTwoHAtk01VFXPosition = new Vector3(2.5f, 1.2f, 3.8f);
        [SerializeField] Vector3 jumpTwoHAtk01VFXRotation = new Vector3(-35, -85, -40);

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
                return;

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (playerPerformingAction.playerCombatManager.isUsingItem)
                return;

            if (!playerPerformingAction.characterLocomotionManager.isGrounded)
            {
                PerformJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
                return;


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

        public void PerformJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
            {
                PerformTwoHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformMainHandJumpingHeavyAttack(playerPerformingAction, weaponPerformingAction);
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
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, hAtk02VFXstartDelay, hAtk02VFXPosition, hAtk02VFXRotation);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, hAtk01VFXstartDelay, hAtk01VFXPosition, hAtk01VFXRotation);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, hAtk01VFXstartDelay, hAtk01VFXPosition, hAtk01VFXRotation);
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
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk02VFXstartDelay, twohAtk02VFXPosition, twohAtk02VFXRotation);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, twoh_heavy_Attack_01, true);
                    PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk01VFXstartDelay, twohAtk01VFXPosition, twohAtk01VFXRotation);
                }
            }
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, twoh_heavy_Attack_01, true);
                PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, twohAtk01VFXstartDelay, twohAtk01VFXPosition, twohAtk01VFXRotation);
            }
        }

        public void PerformMainHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
                return;

            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, heavy_Jumping_Attack_01, true);
            PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpHAtk01VFXstartDelay, jumpHAtk01VFXPosition, jumpHAtk01VFXRotation);

        }

        public void PerformTwoHandJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
                return;

            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyJumpingAttack01, twoh_Heavy_Jumping_Attack_01, true);
            PlayWeaponSwingVFX(weaponPerformingAction, playerPerformingAction, jumpTwoHAtk01VFXstartDelay, jumpTwoHAtk01VFXPosition, jumpTwoHAtk01VFXRotation);

        }

    }
}