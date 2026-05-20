using UnityEngine;

namespace TraverserProject
{

    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

        [Header("Weapon Attack Modifier")]
        public float light_Attack_01_Modifier;
        public PhysicalDamageType light_Attack_01_PhysicalDamageType;
        public float light_Attack_02_Modifier;
        public PhysicalDamageType light_Attack_02_PhysicalDamageType;
        public float heavy_Attack_01_Modifier;
        public float heavy_Attack_02_Modifier;
        public float charge_Attack_01_Modifier;
        public float charge_Attack_02_Modifier;
        public float running_Light_Attack_01_Modifier;
        public float running_Heavy_Attack_01_Modifier;
        public float rolling_Light_Attack_01_Modifier;
        public float rolling_Heavy_Attack_01_Modifier;
        public float backstep_Light_Attack_01_Modifier;
        public float backstep_Heavy_Attack_01_Modifier;
        public float jumping_Light_Attack_01_Modifier;
        public float jumping_Heavy_Attack_01_Modifier;

        [Header("Dual Attack Modifiers")]
        public float dual_Light_Attack_01_Modifier;
        public float dual_Light_Attack_02_Modifier;
        public float dual_Heavy_Attack_01_Modifier;
        public float dual_Heavy_Attack_02_Modifier;
        public float dual_Charge_Attack_01_Modifier;
        public float dual_Charge_Attack_02_Modifier;
        public float dual_Running_Light_Attack_01_Modifier;
        public float dual_Running_Heavy_Attack_01_Modifier;
        public float dual_Rolling_Light_Attack_01_Modifier;
        public float dual_Rolling_Heavy_Attack_01_Modifier;
        public float dual_Backstep_Light_Attack_01_Modifier;
        public float dual_Backstep_Heavy_Attack_01_Modifier;
        public float dual_Jumping_Light_Attack_01_Modifier;
        public float dual_Jumping_Heavy_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                if (damageTarget == characterCausingDamage)
                    return;

                if (!WorldUtilityManager.Singleton.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
                    return;

                CheckForParry(damageTarget);

                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(damageTarget);

            }
        }

        protected override void CheckForParry(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            if (!characterCausingDamage.characterNetworkManager.isParryable.Value)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (damageTarget.characterNetworkManager.isParrying.Value)
            {
                charactersDamaged.Add(damageTarget);
                damageTarget.characterNetworkManager.NotifyTheServerOfParryServerRpc(characterCausingDamage.NetworkObjectId);
                damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
            }
        }

        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = characterCausingDamage.transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect, light_Attack_01_PhysicalDamageType);
                    break;
                case AttackType.LightAttack02:
                    ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect, light_Attack_02_PhysicalDamageType);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.LightJumpingAttack01:
                    ApplyAttackDamageModifiers(jumping_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyJumpingAttack01:
                    ApplyAttackDamageModifiers(jumping_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApplyAttackDamageModifiers(charge_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.RunningLightAttack01:
                    ApplyAttackDamageModifiers(running_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RunningHeavyAttack01:
                    ApplyAttackDamageModifiers(running_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RollingLightAttack01:
                    ApplyAttackDamageModifiers(rolling_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RollingHeavyAttack01:
                    ApplyAttackDamageModifiers(rolling_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.BackstepLightAttack01:
                    ApplyAttackDamageModifiers(backstep_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.BackstepHeavyAttack01:
                    ApplyAttackDamageModifiers(backstep_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualLightAttack01:
                    ApplyAttackDamageModifiers(dual_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualLightAttack02:
                    ApplyAttackDamageModifiers(dual_Light_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.DualHeavyAttack01:
                    ApplyAttackDamageModifiers(dual_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualHeavyAttack02:
                    ApplyAttackDamageModifiers(dual_Heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.DualChargedAttack01:
                    ApplyAttackDamageModifiers(dual_Charge_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualChargedAttack02:
                    ApplyAttackDamageModifiers(dual_Charge_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.DualRunningLightAttack01:
                    ApplyAttackDamageModifiers(dual_Running_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualRunningHeavyAttack01:
                    ApplyAttackDamageModifiers(dual_Running_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualRollingLightAttack01:
                    ApplyAttackDamageModifiers(dual_Rolling_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualRollingHeavyAttack01:
                    ApplyAttackDamageModifiers(dual_Rolling_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualBackstepLightAttack01:
                    ApplyAttackDamageModifiers(dual_Backstep_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualBackstepHeavyAttack01:
                    ApplyAttackDamageModifiers(dual_Backstep_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualJumpingLightAttack01:
                    ApplyAttackDamageModifiers(dual_Jumping_Light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.DualJumpingHeavyAttack01:
                    ApplyAttackDamageModifiers(dual_Jumping_Heavy_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NofityTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z, damageEffect.physicalDamageType);
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;
            damage.physicalDamageType = PhysicalDamageType.Regular;


        }
        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage, PhysicalDamageType physicalDamageType)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;
            damage.physicalDamageType = physicalDamageType;


        }
    }
}