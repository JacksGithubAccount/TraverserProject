using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Stat/Character Damage")]
    public class ModifyCharacterDamageForATimeEffect : TimedCharacterEffect
    {
        [Header("Effect Calculated")]
        private bool effectHasBeenInitialized = false;

        [Header("Damage Modified")]
        public int physicalDamageModified = 0;
        public int magicDamageModifed = 0;
        public int fireDamageModified = 0;
        public int lightningDamageModified = 0;
        public int holyDamageModified = 0;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (effectHasBeenInitialized)
                return;

            effectHasBeenInitialized = true;

            if (!character.IsOwner)
                return;

            character.characterNetworkManager.physicalDamageModifier.Value += physicalDamageModified;
            character.characterNetworkManager.magicDamageModifier.Value += magicDamageModifed;
            character.characterNetworkManager.fireDamageModifier.Value += fireDamageModified;
            character.characterNetworkManager.lightningDamageModifier.Value += lightningDamageModified;
            character.characterNetworkManager.holyDamageModifier.Value += holyDamageModified;
        }
        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (!effectHasBeenInitialized)
                return;

            if (!character.IsOwner)
                return;

            character.characterNetworkManager.physicalDamageModifier.Value -= physicalDamageModified;
            character.characterNetworkManager.magicDamageModifier.Value -= magicDamageModifed;
            character.characterNetworkManager.fireDamageModifier.Value -= fireDamageModified;
            character.characterNetworkManager.lightningDamageModifier.Value -= lightningDamageModified;
            character.characterNetworkManager.holyDamageModifier.Value -= holyDamageModified;


        }


    }
}