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
    }
}