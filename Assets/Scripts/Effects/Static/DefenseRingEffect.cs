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
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifer.Value += physicalDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifer.Value += magicDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorFireDamageAbsorptionModifer.Value += fireDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifer.Value += lightningDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifer.Value += holyDamageAbsorptionGainedFromEffect;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifer.Value -= physicalDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifer.Value -= magicDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorFireDamageAbsorptionModifer.Value -= fireDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifer.Value -= lightningDamageAbsorptionGainedFromEffect;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifer.Value -= holyDamageAbsorptionGainedFromEffect;
            }
        }
    }
}
