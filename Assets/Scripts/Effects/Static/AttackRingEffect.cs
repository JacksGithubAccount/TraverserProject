using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Attack Ring Effect")]
    public class AttackRingEffect : StaticCharacterEffect
    {
        [Header("Stats")]
        public int physicalDamageGainedFromEffect;
        public int magicDamageGainedFromEffect;
        public int fireDamageGainedFromEffect;
        public int lightningDamageGainedFromEffect;
        public int holyDamageGainedFromEffect;

        [Header("Conditionals")]
        public float healthRemainingToTakeEffect;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.physicalDamageModifier.Value += physicalDamageGainedFromEffect;
                character.characterNetworkManager.magicDamageModifier.Value += magicDamageGainedFromEffect;
                character.characterNetworkManager.fireDamageModifier.Value += fireDamageGainedFromEffect;
                character.characterNetworkManager.lightningDamageModifier.Value += lightningDamageGainedFromEffect;
                character.characterNetworkManager.holyDamageModifier.Value += holyDamageGainedFromEffect;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.physicalDamageModifier.Value -= physicalDamageGainedFromEffect;
                character.characterNetworkManager.magicDamageModifier.Value -= magicDamageGainedFromEffect;
                character.characterNetworkManager.fireDamageModifier.Value -= fireDamageGainedFromEffect;
                character.characterNetworkManager.lightningDamageModifier.Value -= lightningDamageGainedFromEffect;
                character.characterNetworkManager.holyDamageModifier.Value -= holyDamageGainedFromEffect;
            }
        }
    }
}
