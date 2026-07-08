using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class DurkStompCollider : DamageCollider
    {
        [SerializeField] AIDurkCharacterManager durkManager;

        protected override void Awake()
        {
            base.Awake();

            durkManager = GetComponentInParent<AIDurkCharacterManager>();
        }
        public void StompAttack()
        {
            physicalDamage = durkManager.durkCombatManager.stompDamage;
            poiseDamage = durkManager.durkCombatManager.stompDamage;
            GameObject stompVFX = Instantiate(durkManager.durkCombatManager.durkImpactVFX, transform);

            Collider[] colliders = Physics.OverlapSphere(transform.position, durkManager.durkCombatManager.stompAttackAOERadius, WorldUtilityManager.Singleton.GetCharacterLayers());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>();

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager damageTarget = colliders[i].GetComponentInParent<CharacterManager>();

                if (damageTarget == null)
                    continue;

                if (!damageTarget.IsOwner)
                    continue;

                if (charactersDamaged.Contains(damageTarget))
                    continue;


                CheckForBlock(damageTarget);

                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(damageTarget);
            }

        }
    }
}