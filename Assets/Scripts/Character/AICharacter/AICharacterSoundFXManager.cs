using UnityEngine;

namespace TraverserProject
{

    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public GameObject interactableDialogueCollider;
        public bool dialogueIsPlaying = false;

        public override void PlayBlockSoundFX()
        {
            if (blockingSFX.Length <= 0)
                return;

            PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(blockingSFX));
        }

        public void PlayCurrentDialogueEvent()
        {

        }

        public void PlayFarewellDialogueEvent()
        {

        }

        public void CancelCurrentDialogueEvent()
        {

        }

        public void OnCurrentDialogueEnded()
        {

        }


    }
}