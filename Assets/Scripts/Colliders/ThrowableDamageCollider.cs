using UnityEngine;
using System.Collections;

namespace TraverserProject
{
    public class ThrowableDamageCollider : DamageCollider
    {
        public CharacterManager itemThrower;
        private ThrowableManager throwableManager;

        protected override void Awake()
        {
            base.Awake();
            throwableManager = GetComponentInParent<ThrowableManager>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                if (itemThrower == null)
                    return;

                if (damageTarget == itemThrower)
                    return;

                if (!WorldUtilityManager.Singleton.CanIDamageThisTarget(itemThrower.characterGroup, damageTarget.characterGroup))
                    return;

                CheckForParry(damageTarget);

                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(damageTarget);

                throwableManager.WaitThenInstantiateDestructionFX(0.0f);
            }
            

        }

        protected override void CheckForParry(CharacterManager damageTarget)
        {


        }

        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = itemThrower.transform.position - damageTarget.transform.position;
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(itemThrower.transform.forward, damageTarget.transform.forward, Vector3.up);



            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (itemThrower.IsOwner)
            {
                damageTarget.characterNetworkManager.NofityTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, itemThrower.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
            }
        }
    }
}