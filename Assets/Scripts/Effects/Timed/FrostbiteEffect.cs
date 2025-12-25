using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Frostbite Effect")]
    public class FrostbiteEffect : TimedCharacterEffect
    {
        [Header("HP Percentage Damage")]
        [SerializeField] float percentageOfMaxHealthAsDamageDealt = 10;
        [Header("Effect Processed")]
        private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (timeRemainingOnEffect <= 0 || character.isDead.Value)
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                character.characterEffectsManager.RemoveTimedEffect(WorldCharacterEffectsManager.Singleton.frostbiteStaminaRegenerationEffect.effectID);
                character.characterNetworkManager.isFrostbite.Value = false;
            }

            if (!character.characterNetworkManager.isFrostbite.Value)
            {
                character.characterEffectsManager.RemoveTimedEffect(effectID);
                character.characterEffectsManager.RemoveTimedEffect(WorldCharacterEffectsManager.Singleton.frostbiteStaminaRegenerationEffect.effectID);
            }

            if (!effectHasBeenInitialized)
            {
                effectHasBeenInitialized = true;
                InflictStaminaRegenerationDebuff(character);
            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.isFrostbite.Value = false;
                character.characterNetworkManager.isFrozen.Value = false;
            }

        }

        private void InflictStaminaRegenerationDebuff(CharacterManager character)
        {
            ModifyStaminaRegenerationForATimeEffect staminaDebuff = Instantiate(WorldCharacterEffectsManager.Singleton.frostbiteStaminaRegenerationEffect);
            staminaDebuff.defaultLengthOfEffect = this.defaultLengthOfEffect;
            character.characterEffectsManager.AddTimedEffect(staminaDebuff);

            if (!character.IsOwner)
                return;

            float damage = character.characterNetworkManager.maxHealth.Value * (percentageOfMaxHealthAsDamageDealt / 100);

            if (damage < 0)
                damage = 1;

            character.characterNetworkManager.currentStamina.Value = 0;
            character.characterEffectsManager.ProcessEffectDamage(Mathf.RoundToInt(damage));
            character.characterNetworkManager.isFrozen.Value = true;
        }
    }
}