using UnityEngine;

namespace TraverserProject
{

    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug")]
        [SerializeField] bool applyPoisonBuildUp = false;
        [SerializeField] bool applyBleedBuildUp = false;

        private void Update()
        {
            if (applyPoisonBuildUp)
            {
                applyPoisonBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.Singleton.poisonBuildUpEffect);
                buildUp.buildUpAmount = 25;
                character.characterEffectsManager.ProcessInstantEffect(buildUp);
            }

            if (applyBleedBuildUp)
            {
                applyBleedBuildUp = false;
                TakeBuildUpEffect buildUp = Instantiate(WorldCharacterEffectsManager.Singleton.bleedBuildUpEffect);
                buildUp.buildUpAmount = 25;
                character.characterEffectsManager.ProcessInstantEffect(buildUp);
            }
        }

    }
}