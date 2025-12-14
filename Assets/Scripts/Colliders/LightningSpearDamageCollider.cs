using UnityEngine;

namespace TraverserProject
{

    public class LightningSpearDamageCollider : SpellProjectileDamageCollider
    {
        private LightningSpearManager lightningSpearManager;


        protected override void Awake()
        {
            base.Awake();
            lightningSpearManager = GetComponentInParent<LightningSpearManager>();
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

                lightningSpearManager.WaitThenInstantiateSpellDestructionFX(0.0f);


            }
        }
    }
}