using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;
        public WeaponItem currentWeaponBeingUsed;
        public ProjectileSlot currentProjectileBeingUsed;

        [Header("Projectile")]
        private Vector3 projectileAimDirection;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;
        public bool canComboWithOffHandWeapon = false;
        public bool isUsingItem = false;



        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
            }



        }

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();

            player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }

        public override void AttemptRiposte(RaycastHit hit)
        {
            Debug.Log("Riposting Target");
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (!targetCharacter.characterNetworkManager.isRipostable.Value)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                riposteWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            //animation will change based on weapon's animator controller so the animation can be chosen there, name is always the same
            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeCriticalDamageEffect);

            damageEffect.physicalDamage = riposteCollider.physicalDamage;
            damageEffect.magicDamage = riposteCollider.magicDamage;
            damageEffect.fireDamage = riposteCollider.fireDamage;
            damageEffect.lightningDamage = riposteCollider.lightningDamage;
            damageEffect.holyDamage = riposteCollider.holyDamage;
            damageEffect.poiseDamage = riposteCollider.poiseDamage;

            damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

            targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRpc(targetCharacter.NetworkObjectId, character.NetworkObjectId, "Riposted_01", riposteWeapon.itemID,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage);
        }

        public override void AttemptBackstab(RaycastHit hit)
        {
            Debug.Log("Riposting Target");
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            if (targetCharacter == null)
                return;

            if (!targetCharacter.characterCombatManager.canBeBackstabbed)
                return;

            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            MeleeWeaponItem backstabWeapon;
            MeleeWeaponDamageCollider backstabCollider;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                backstabWeapon = player.playerInventoryManager.currentLeftHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.leftWeaponManager.meleeDamageCollider;
            }
            else
            {
                backstabWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
                backstabCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;
            }

            //animation will change based on weapon's animator controller so the animation can be chosen there, name is always the same
            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Backstab_01", true);

            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeCriticalDamageEffect);

            damageEffect.physicalDamage = backstabCollider.physicalDamage;
            damageEffect.magicDamage = backstabCollider.magicDamage;
            damageEffect.fireDamage = backstabCollider.fireDamage;
            damageEffect.lightningDamage = backstabCollider.lightningDamage;
            damageEffect.holyDamage = backstabCollider.holyDamage;
            damageEffect.poiseDamage = backstabCollider.poiseDamage;

            damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.fireDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.lightningDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.holyDamage *= backstabWeapon.backstab_Attack_01_Modifier;
            damageEffect.poiseDamage *= backstabWeapon.backstab_Attack_01_Modifier;

            targetCharacter.characterNetworkManager.NotifyTheServerOfBackstabServerRpc(targetCharacter.NetworkObjectId, character.NetworkObjectId, "Backstabbed_01", backstabWeapon.itemID,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
                return;
            if (currentWeaponBeingUsed == null)
                return;

            float staminaDeducted = 0;

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.LightJumpingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.jumpingLightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyJumpingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.jumpingHeavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningLightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningLightAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningHeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningHeavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingLightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingLightAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingHeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingHeavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepLightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepLightAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepHeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepHeavyAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            Debug.Log("Stamina drain: " + Mathf.RoundToInt(staminaDeducted));
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (player.IsOwner)
            {
                PlayerCamera.Singleton.SetLockCameraHeight();
            }
        }

        //animation event calls
        public override void EnableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {

            }
        }

        public override void DisableCanDoCombo()
        {

            player.playerCombatManager.canComboWithMainHandWeapon = false;
            player.playerCombatManager.canComboWithOffHandWeapon = false;

        }

        public void ReleaseArrow()
        {
            if (player.IsOwner)
                player.playerNetworkManager.hasArrowNotched.Value = false;

            if (player.playerEffectsManager.activeDrawnProjectileFX != null)
                Destroy(player.playerEffectsManager.activeDrawnProjectileFX);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.releaseArrowSFX));

            Animator bowAnimator;

            if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
            {
                bowAnimator = player.playerEquipmentManager.leftHandWeaponModel.GetComponentInChildren<Animator>();
            }
            else
            {
                bowAnimator = player.playerEquipmentManager.rightHandWeaponModel.GetComponentInChildren<Animator>();
            }

            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            if (!player.IsOwner)
                return;

            RangedProjectileItem projectileItem = null;

            switch (currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = player.playerInventoryManager.mainProjectile;
                    break;
                case ProjectileSlot.Secondary:
                    projectileItem = player.playerInventoryManager.secondaryProjectile;
                    break;
                default:
                    break;
            }

            if (projectileItem == null)
                return;

            if (projectileItem.currentAmmoAmount <= 0)
                return;

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            projectileItem.currentAmmoAmount -= 1;

            switch (currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    PlayerUIManager.Singleton.playerUIHudManager.SetMainProjectileQuickSlotIcon(projectileItem);
                    break;
                case ProjectileSlot.Secondary:
                    PlayerUIManager.Singleton.playerUIHudManager.SetMainProjectileQuickSlotIcon(projectileItem);
                    break;
                default:
                    break;
            }

            projectileInstantiationLocation = player.playerCombatManager.lockOnTransform;
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = player;

            float yRotationDuringFire = player.transform.localEulerAngles.y;

            //aiming
            if (player.playerNetworkManager.isAiming.Value)
            {
                Ray newRay = new Ray(lockOnTransform.position, PlayerCamera.Singleton.aimDirection);
                projectileAimDirection = newRay.GetPoint(5000);
                projectileGameObject.transform.LookAt(projectileAimDirection);
            }
            else
            {
                //locked onto target
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - projectileGameObject.transform.position);
                    projectileGameObject.transform.rotation = arrowRotation;
                }
                //unlocked and not aiming
                else
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                    projectileGameObject.transform.rotation = arrowRotation;
                }
            }


            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnore = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);

            foreach (Collider hitBox in collidersArrowWillIgnore)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;

            player.playerNetworkManager.NotifyTheServerOfReleasedProjectileServerRpc(player.OwnerClientId, projectileItem.itemID, projectileAimDirection.x, projectileAimDirection.y, projectileAimDirection.z, yRotationDuringFire);
        }

        public void InstantiateSpellWarmUpFX()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);
        }

        public void InstantiateSpellReleaseFX()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.InstantiateReleaseFX(player);
        }

        public void SuccessfullyCastSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpell(player);
        }

        public void SuccessfullyChargeSpell()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyChargeSpell(player);
        }

        public void SuccessfullyCastSpellFullCharge()
        {
            if (player.playerInventoryManager.currentSpell == null)
                return;

            player.playerInventoryManager.currentSpell.SuccessfullyCastSpellFullCharge(player);
        }

        public void SuccessfullyUseQuickSlotItem()
        {
            if (player.playerInventoryManager.currentQuickSlotItem != null)
                player.playerInventoryManager.currentQuickSlotItem.SuccessfullyUseItem(player);
        }

        public WeaponItem SelectWeaponToPerformAshOfWar()
        {
            WeaponItem selectedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
            player.playerNetworkManager.SetCharacterActionHand(false);
            player.playerNetworkManager.currentWeaponBeingUsed.Value = selectedWeapon.itemID;
            return selectedWeapon;
        }

    }
}