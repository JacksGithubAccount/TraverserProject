using UnityEngine;

namespace TraverserProject
{
    public class EventTriggerConstantStatusBuildUp : MonoBehaviour
    {
        [SerializeField] BuildUp buildUp;
        [SerializeField] float buildUpAmount = .1f;

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character == null)
                return;

            TakeBuildUpEffect buildUpEffect = null;
            switch (buildUp)
            {
                case BuildUp.Poison:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takePoisonBuildUpEffect);
                    break;
                case BuildUp.Bleed:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeBleedBuildUpEffect);
                    break;
                case BuildUp.Frost:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeFrostBuildUpEffect);
                    break;
                default:
                    break;
            }

            buildUpEffect.buildUpAmount = 3;
            character.characterEffectsManager.ProcessInstantEffect(buildUpEffect);
        }
        private void OnTriggerStay(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character == null)
                return;

            TakeBuildUpEffect buildUpEffect = null;
            switch (buildUp)
            {
                case BuildUp.Poison:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takePoisonBuildUpEffect);
                    break;
                case BuildUp.Bleed:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeBleedBuildUpEffect);
                    break;
                case BuildUp.Frost:
                    buildUpEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeFrostBuildUpEffect);
                    break;
                default:
                    break;
            }

            buildUpEffect.buildUpAmount = buildUpAmount;
            character.characterEffectsManager.ProcessInstantEffect(buildUpEffect);
        }
    }
}
