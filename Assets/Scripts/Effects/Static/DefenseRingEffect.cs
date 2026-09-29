using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Defense Ring Effect")]
    public class DefenseRingEffect : StaticCharacterEffect
    {
        public int physicalDamageAbsorptionGainedFromEffect;
        public int magicDamageAbsorptionGainedFromEffect;
        public int fireDamageAbsorptionGainedFromEffect;
        public int lightningDamageAbsorptionGainedFromEffect;
        public int holyDamageAbsorptionGainedFromEffect;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifier.Value += physicalDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifier.Value += magicDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorFireDamageAbsorptionModifier.Value += fireDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifier.Value += lightningDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifier.Value += holyDamageAbsorptionGainedFromEffect;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifier.Value -= physicalDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifier.Value -= magicDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorFireDamageAbsorptionModifier.Value -= fireDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifier.Value -= lightningDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifier.Value -= holyDamageAbsorptionGainedFromEffect;
            }
        }
    }
}
