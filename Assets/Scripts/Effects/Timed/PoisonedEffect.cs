using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Poison Effect")]
    public class PoisonedEffect : TimedCharacterEffect
    {
        private int poisonDamage = 1;
        private bool poisonDamageHasBeenCalculated = false;

        [Header("Poison Damage")]
        [SerializeField] int poisonDamagePerTick = 10;

        public override void ProcessEffect(CharacterManager character)
        {
            timeRemainingOnEffect -= 1;

            if (timeRemainingOnEffect <= 0 || character.isDead.Value)
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                character.characterNetworkManager.isPoisoned.Value = false;
            }

            if (!poisonDamageHasBeenCalculated)
            {
                poisonDamageHasBeenCalculated = true;
                CalculatePoisonDamage(character);
            }

            if (!character.characterNetworkManager.isPoisoned.Value)
                character.characterEffectsManager.RemoveTimedEffect(effectID);

            ProcessPoisonDamage(character);
        }

        private void CalculatePoisonDamage(CharacterManager character)
        {
            poisonDamage = poisonDamagePerTick;
        }

        private void ProcessPoisonDamage(CharacterManager character)
        {
            character.characterEffectsManager.ProcessPoisonDamage(poisonDamage);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (character.IsOwner)
                character.characterNetworkManager.isPoisoned.Value = false;
        }

    }
}