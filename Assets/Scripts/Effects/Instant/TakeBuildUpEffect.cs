using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Build Up Effect")]
    public class TakeBuildUpEffect : InstantCharacterEffect
    {
        [Header("Build Up")]
        [SerializeField] BuildUp buildUpType;
        public float buildUpAmount = 10;

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
                case BuildUp.Frost:
                    CheckForFrostbiteStatus(character);
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

            if (character.characterNetworkManager.isPoisoned.Value)
                return;

            if (character.characterNetworkManager.poisonBuildUp.Value > character.characterNetworkManager.immunityBuildUpCapacity.Value)
            {
                character.characterNetworkManager.poisonBuildUp.Value = character.characterNetworkManager.immunityBuildUpCapacity.Value;
                character.characterNetworkManager.isPoisoned.Value = true;

                PoisonedEffect poison = Instantiate(WorldCharacterEffectsManager.Singleton.poisonedEffect);
                poison.defaultLengthOfEffect = character.characterNetworkManager.immunityBuildUpCapacity.Value;
                character.characterEffectsManager.AddTimedEffect(poison);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


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

            if (character.characterNetworkManager.bleedBuildUp.Value > character.characterNetworkManager.robustnessBuildUpCapacity.Value)
            {
                //character.characterNetworkManager.bleedBuildUp.Value = character.characterNetworkManager.bleedBuildUpCapacity.Value;
                character.characterNetworkManager.isBloodLoss.Value = true;

                BloodLossEffect bloodLoss = Instantiate(WorldCharacterEffectsManager.Singleton.bloodLossEffect);
                character.characterEffectsManager.ProcessInstantEffect(bloodLoss);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


            }
        }

        private void CheckForFrostbiteStatus(CharacterManager character)
        {
            if (character.characterNetworkManager.isFrostbite.Value)
                return;

            BuildUpEffect frostBuildUp = character.characterEffectsManager.CheckForTimedEffect(WorldCharacterEffectsManager.Singleton.degradeFrostBuildUpEffect.effectID) as BuildUpEffect;

            if (frostBuildUp == null)
            {
                frostBuildUp = Instantiate(WorldCharacterEffectsManager.Singleton.degradeFrostBuildUpEffect);
                character.characterEffectsManager.AddTimedEffect(frostBuildUp);
                frostBuildUp.ProcessEffect(character);
            }

            if (character.characterNetworkManager.frostBuildUp.Value > character.characterNetworkManager.robustnessBuildUpCapacity.Value)
            {
                character.characterNetworkManager.frostBuildUp.Value = character.characterNetworkManager.robustnessBuildUpCapacity.Value;
                character.characterNetworkManager.isFrostbite.Value = true;

                FrostbiteEffect frost = Instantiate(WorldCharacterEffectsManager.Singleton.frostbiteEffect);
                frost.defaultLengthOfEffect = character.characterNetworkManager.robustnessBuildUpCapacity.Value;
                character.characterEffectsManager.AddTimedEffect(frost);

                PlayerManager player = character as PlayerManager;

                if (player == null)
                    return;

                if (!player.IsOwner)
                    return;


            }
        }


    }
}