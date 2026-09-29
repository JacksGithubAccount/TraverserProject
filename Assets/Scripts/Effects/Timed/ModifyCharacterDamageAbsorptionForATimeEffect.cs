using UnityEngine;

namespace TraverserProject
{

    public class ModifyCharacterDamageAbsorptionForATimeEffect : TimedCharacterEffect
    {
        [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Stat/Character Damage Absorption")]
        public class ModifyCharacterDamageForATimeEffect : TimedCharacterEffect
        {
            [Header("Effect Calculated")]
            private bool effectHasBeenInitialized = false;

            [Header("Damage Modified")]
            public int physicalDamageAbsorptionModified = 0;
            public int magicDamageAbsorptionModifed = 0;
            public int fireDamageAbsorptionModified = 0;
            public int lightningDamageAbsorptionModified = 0;
            public int holyDamageAbsorptionModified = 0;

            public override void ProcessEffect(CharacterManager character)
            {
                base.ProcessEffect(character);

                if (effectHasBeenInitialized)
                    return;

                effectHasBeenInitialized = true;

                if (!character.IsOwner)
                    return;

                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifier.Value += physicalDamageAbsorptionModified;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifier.Value += magicDamageAbsorptionModifed;
                character.characterNetworkManager.armorFireDamageAbsorptionModifier.Value += fireDamageAbsorptionModified;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifier.Value += lightningDamageAbsorptionModified;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifier.Value += holyDamageAbsorptionModified;
            }
            public override void RemoveEffect(CharacterManager character)
            {
                base.RemoveEffect(character);

                if (!effectHasBeenInitialized)
                    return;

                if (!character.IsOwner)
                    return;

                character.characterNetworkManager.armorPhysicalDamageAbsorptionModifier.Value -= physicalDamageAbsorptionModified;
                character.characterNetworkManager.armorMagicDamageAbsorptionModifier.Value -= magicDamageAbsorptionModifed;
                character.characterNetworkManager.armorFireDamageAbsorptionModifier.Value -= fireDamageAbsorptionModified;
                character.characterNetworkManager.armorLightningDamageAbsorptionModifier.Value -= lightningDamageAbsorptionModified;
                character.characterNetworkManager.armorHolyDamageAbsorptionModifier.Value -= holyDamageAbsorptionModified;


            }


        }

    }
}