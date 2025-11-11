using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Build Up Effect")]
    public class TakeBuildUpEffect : InstantCharacterEffect
    {
        [Header("Build Up")]
        [SerializeField] BuildUp buildUpType;
        public int buildUpAmount = 10;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            character.characterEffectsManager.AddBuildUps(buildUpType, buildUpAmount);

            switch (buildUpType)
            {
                case BuildUp.Poison:
                    CheckForPoisonedStatus(character);
                    break;
                case BuildUp.Bleed:
                    CheckForBloodLossStatus(character);
                    break;
                default:
                    break;
            }
        }

        private void CheckForPoisonedStatus(CharacterManager character)
        {
            BuildUpEffect poisonBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.Singleton.degradePoisonBuildUpEffect.effectID) as BuildUpEffect;

            if (poisonBuildUp == null)
            {
                poisonBuildUp = Instantiate(WorldCharacterEffectsManager.Singleton.degradePoisonBuildUpEffect);
                character.characterEffectsManager.AddTimedEffect(poisonBuildUp);
                poisonBuildUp.ProcessEffect(character);
            }
        }

        private void CheckForBloodLossStatus(CharacterManager character)
        {
            BuildUpEffect bleedBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.Singleton.degradeBleedBuildUpEffect.effectID) as BuildUpEffect;

            if (bleedBuildUp == null)
            {
                bleedBuildUp = Instantiate(WorldCharacterEffectsManager.Singleton.degradeBleedBuildUpEffect);
                character.characterEffectsManager.AddTimedEffect(bleedBuildUp);
                bleedBuildUp.ProcessEffect(character);
            }
        }


    }
}