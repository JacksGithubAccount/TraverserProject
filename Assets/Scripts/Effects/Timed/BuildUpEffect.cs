using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Build Up Effect")]
    public class BuildUpEffect : TimedCharacterEffect
    {
        [Header("Type")]
        public BuildUp buildUpType;

        [Header("Degradation")]
        public float buildUpAmountDegradationWhenNotAfflicted = -1;
        public float buildUpAmountDegradationWhenAfflicted = -1;
        public float buildUpRemaining = 1;

        public override void ProcessEffect(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            switch(buildUpType)
            {
                case BuildUp.Poison:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterEffectsManager.RemoveTimedEffect(effectID);
                    }
                    break;
                case BuildUp.Bleed:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterEffectsManager.RemoveTimedEffect(effectID);
                    }
                    break;
                case BuildUp.Frost:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterEffectsManager.RemoveTimedEffect(effectID);
                    }
                    break;

            }           


            DegradeBuildUp(character);

            switch (buildUpType)
            {
                case BuildUp.Poison:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterNetworkManager.poisonBuildUp.Value = 0;
                        character.characterNetworkManager.isPoisoned.Value = false;
                    }
                    break;
                case BuildUp.Bleed:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterNetworkManager.bleedBuildUp.Value = 0;
                        character.characterNetworkManager.isBloodLoss.Value = false;
                    }
                    break;
                case BuildUp.Frost:
                    if (buildUpRemaining <= 0)
                    {
                        character.characterNetworkManager.frostBuildUp.Value = 0;
                        character.characterNetworkManager.isFrostbite.Value = false;
                    }
                    break;

            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);
        }

        private void DegradeBuildUp(CharacterManager character)
        {
            switch (buildUpType)
            {
                case BuildUp.Poison:
                    if (character.characterNetworkManager.isPoisoned.Value)
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenAfflicted, this);
                    else
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenNotAfflicted, this);
                    break;
                case BuildUp.Bleed:
                    if (character.characterNetworkManager.isBloodLoss.Value)
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenAfflicted, this);
                    else
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenNotAfflicted, this);
                    break;
                case BuildUp.Frost:
                    if (character.characterNetworkManager.isFrostbite.Value)
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenAfflicted, this);
                    else
                        character.characterStatsManager.DegradeBuildUps(buildUpType, buildUpAmountDegradationWhenNotAfflicted, this);
                    break;
                default:
                    break;

            }
            
        }

    }
}