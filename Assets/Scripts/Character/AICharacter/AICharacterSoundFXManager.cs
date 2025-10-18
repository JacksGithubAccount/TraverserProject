using UnityEngine;

namespace TraverserProject
{

    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        AICharacterManager aiCharacter;

        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public GameObject interactableDialogueCollider;
        public CharacterDialogue currentDialogue;
        public CharacterDialogue farewellDialogue;
        public bool dialogueIsPlaying = false;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void PlayBlockSoundFX()
        {
            if (blockingSFX.Length <= 0)
                return;

            PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(blockingSFX));
        }

        public void PlayCurrentDialogueEvent()
        {
            if (currentDialogue == null)
                return;

            if (!dialogueIsPlaying)
            {
                currentDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SendNextDialoguePopUpInIndex(currentDialogue, aiCharacter);
            }
        }

        public void PlayFarewellDialogueEvent()
        {
            if (farewellDialogue == null)
                return;

            if (!dialogueIsPlaying)
            {
                farewellDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SendNextDialoguePopUpInIndex(farewellDialogue, aiCharacter);
            }
        }

        public void CancelCurrentDialogueEvent()
        {
            if (dialogueIsPlaying)
            {
                dialogueIsPlaying = false;
                PlayerUIManager.Singleton.playerUIPopUpManager.CancelDialoguePopUp(aiCharacter);
            }
        }

        public void OnCurrentDialogueEnded()
        {

        }


    }
}