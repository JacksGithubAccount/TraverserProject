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
            GameObject stompVFX = Instantiate(durkManager.durkCombatManager.durkImpactVFX, transform);

            Collider[] colliders = Physics.OverlapSphere(transform.position, durkManager.durkCombatManager.stompAttackAOERadius, WorldUtilityManager.Singleton.GetCharacterLayers());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>();

            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character))
                        continue;

                    if (character == durkManager)
                        continue;

                    charactersDamaged.Add(character);


                    if (character.IsOwner)
                    {


                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);
                        damageEffect.physicalDamage = durkManager.durkCombatManager.stompDamage;
                        damageEffect.poiseDamage = durkManager.durkCombatManager.stompDamage;

                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }


            }
        }
    }
}