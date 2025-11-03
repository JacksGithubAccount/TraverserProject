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

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                if (damageTarget == spellCaster)
                    return;

                if (!WorldUtilityManager.Singleton.CanIDamageThisTarget(spellCaster.characterGroup, damageTarget.characterGroup))
                    return;

                CheckForParry(damageTarget);

                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(damageTarget);

                fireBallManager.WaitThenInstantiateSpellDestructionFX(0.0f);


            }
        }

        protected override void CheckForParry(CharacterManager damageTarget)
        {


        }

        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = spellCaster.transform.position - damageTarget.transform.position;
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(spellCaster.transform.forward, damageTarget.transform.forward, Vector3.up);



            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (spellCaster.IsOwner)
            {
                damageTarget.characterNetworkManager.NofityTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, spellCaster.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
            }
        }
    }
}