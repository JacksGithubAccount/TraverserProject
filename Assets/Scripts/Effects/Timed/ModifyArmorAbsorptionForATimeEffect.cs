using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Stat/Armor Absorption")]
    public class ModifyArmorAbsorptionForATimeEffect : TimedCharacterEffect
    {
        [Header("Armor Absorption")]
        [SerializeField] float armorPhysicalDamageAbsorptionModifer = 0;
        [SerializeField] float armorMagicDamageAbsorptionModifer = 0;
        [SerializeField] float armorFireDamageAbsorptionModifer = 0;
        [SerializeField] float armorLightningDamageAbsorptionModifer = 0;
        [SerializeField] float armorHolyDamageAbsorptionModifer = 0;

        [Header("Effect Processed")]
        private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!effectHasBeenInitialized)
            {
                //toggle some UI icon on character HP bar or player hud if is owner
                if (!character.IsOwner)
                    return;

                effectHasBeenInitialized = true;
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifer.Value += armorPhysicalDamageAbsorptionModifer;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifer.Value += armorMagicDamageAbsorptionModifer;
                character.characterNetworkManager.armorFireDamageAbsorptionModifer.Value += armorFireDamageAbsorptionModifer;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifer.Value += armorLightningDamageAbsorptionModifer;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifer.Value += armorHolyDamageAbsorptionModifer;
    }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (effectHasBeenInitialized)
            {
                //remove ui icon if implemented
                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifer.Value -= armorPhysicalDamageAbsorptionModifer;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifer.Value -= armorMagicDamageAbsorptionModifer;
                character.characterNetworkManager.armorFireDamageAbsorptionModifer.Value -= armorFireDamageAbsorptionModifer;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifer.Value -= armorLightningDamageAbsorptionModifer;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifer.Value -= armorHolyDamageAbsorptionModifer;
            }
        }
    }
}
