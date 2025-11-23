using UnityEngine;

namespace TraverserProject
{
    public class HealDamageCollider : SpellPointBlankDamageCollider
    {
        private HealActivatedManager healManager;
        public int recoveryAmount;        

        protected override void Awake()
        {
            base.Awake();
            healManager = GetComponentInParent<HealActivatedManager>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager recoveryTarget = other.GetComponentInParent<CharacterManager>();

            if (recoveryTarget != null)
            {
                if (!WorldUtilityManager.Singleton.CanIHealThisTarget(spellCaster.characterGroup, recoveryTarget.characterGroup))
                    return;


                if (!recoveryTarget.characterNetworkManager.isInvulnerable.Value)
                    DamageTarget(recoveryTarget);
            }
        }

        protected override void DamageTarget(CharacterManager recoveryTarget)
        {
            if (charactersDamaged.Contains(recoveryTarget))
                return;

            charactersDamaged.Add(recoveryTarget);

            TakeRecoveryEffect recoveryEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeRecoveryEffect);
            recoveryEffect.recoveryAmount = recoveryAmount;


            recoveryTarget.characterEffectsManager.ProcessInstantEffect(recoveryEffect);
        }
    }
}