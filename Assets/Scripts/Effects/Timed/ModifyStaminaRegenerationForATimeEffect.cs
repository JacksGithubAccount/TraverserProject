using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Stat/Stamina Regeneration")]
    public class ModifyStaminaRegenerationForATimeEffect : TimedCharacterEffect
    {
        [Header("Regeneration")]
        [SerializeField] public float staminaRegenerationPercentageModifier = 15;

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
                character.characterNetworkManager.staminaRegenerationModifier.Value += staminaRegenerationPercentageModifier;
            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (effectHasBeenInitialized)
            {
                //remove ui icon if implemented
                character.characterNetworkManager.staminaRegenerationModifier.Value -= staminaRegenerationPercentageModifier;
            }
        }

    }
}