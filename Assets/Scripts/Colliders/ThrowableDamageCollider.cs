using UnityEngine;
using System.Collections;

namespace TraverserProject
{
    public class ThrowableDamageCollider : DamageCollider
    {
        public CharacterManager itemThrower;
        private ThrowableManager throwableManager;
        bool hasPenetratedSurface;

        protected override void Awake()
        {
            base.Awake();
            throwableManager = GetComponentInParent<ThrowableManager>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            RaycastHit hit;

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

                switch (throwableManager.throwableType)
                {
                    case ThrowableType.Destructible:
                        throwableManager.WaitThenInstantiateDestructionFX(0.0f);
                        break;
                    case ThrowableType.Lingering:                        
                        if (Physics.Raycast(transform.position, transform.forward, out hit))
                        {
                            throwableManager.CreateObjectOnCollision(other, false, hit);
                        }                        
                        break;
                    case ThrowableType.Persistant:
                        if (Physics.Raycast(transform.position, transform.forward, out hit))
                        {
                            throwableManager.CreateObjectOnCollision(other, true, hit);
                        }
                        break;
                    default:
                        break;

                }
                
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
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(itemThrower.OwnerClientId, damageTarget.NetworkObjectId, itemThrower.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
            }
        }
    }
}