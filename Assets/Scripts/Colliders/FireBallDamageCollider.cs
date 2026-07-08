using UnityEngine;

namespace TraverserProject
{

    public class FireBallDamageCollider : SpellProjectileDamageCollider
    {
        private FireBallManager fireBallManager;


        protected override void Awake()
        {
            base.Awake();
            fireBallManager = GetComponentInParent<FireBallManager>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget == null)
                return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //we do not want to damage ourselves
            if (damageTarget == spellCaster)
                return;

            if (!spellCaster.IsOwner)
                return;

            //you could also reverse here, prioritizing the caster instead of damage target

            //check if we can damage this target based on friendly fire
            if (!WorldUtilityManager.Singleton.CanIDamageThisTarget(spellCaster.characterGroup, damageTarget.characterGroup))
                return;

            CheckForParry(damageTarget);

            CheckForBlock(damageTarget);

            if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                DamageTarget(damageTarget);

            if (fireBallManager != null)
                fireBallManager.WaitThenInstantiateSpellDestructionFX(0.0f);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            //if this character has already been damaged, do not proceed
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);


            //step 1, load up the damage effect
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;


            //step 2, sent it to the server to play for everyone else
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(spellCaster.OwnerClientId, damageTarget.NetworkObjectId, spellCaster.NetworkObjectId,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);

            //step 3, play it instantly
            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            //if the person is not being damaged by this collider locally on their end, do not process the damage step
            if (!damageTarget.IsOwner)
                return;

            GetBlockingDotValues(damageTarget);

            if (damageTarget.characterNetworkManager.isBlocking.Value && dotValueFromAttackToDamageTarget > 0.3f)
            {
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeBlockedDamageEffect);

                damageEffect.physicalDamage = physicalDamage;
                damageEffect.magicDamage = magicDamage;
                damageEffect.fireDamage = fireDamage;
                damageEffect.lightningDamage = lightningDamage;
                damageEffect.holyDamage = holyDamage;
                damageEffect.poiseDamage = poiseDamage;
                damageEffect.staminaDamage = poiseDamage;
                damageEffect.contactPoint = contactPoint;

                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(spellCaster.OwnerClientId, damageTarget.NetworkObjectId, spellCaster.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);


                damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
            }
        }
    }
}
    