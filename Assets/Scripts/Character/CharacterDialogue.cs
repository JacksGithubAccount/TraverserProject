using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/Dialogue")]

    public class CharacterDialogue : ScriptableObject
    {
        [Header("Dialogue Requirements")]
        public int requiredStageID = 0;

        [Header("Greeting Dialogue")]
        [TextArea] public List<string> greetingDialogueString = new List<string>();
        public List<AudioClip> greetingDialogueAudio = new List<AudioClip>();
        private bool greetingHasPlayed = false;

        [Header("Core Dialogue")]
        [TextArea] public List<string> dialogueString = new List<string>();
        public List<AudioClip> dialogueAudio = new List<AudioClip>();
        public int dialogueIndex = 0;

        [Header("Farewell Dialogue")]
        [TextArea] public List<string> farewellDialogueString = new List<string>();
        public List<AudioClip> farewellDialogueAudio = new List<AudioClip>();
        private bool farewellHasPlayed = false;

        [Header("End Triggers")]
        [SerializeField] bool setStageIndex = false;
        [SerializeField] int stageID = 0;
        [SerializeField] DialogueEndEvents endEvent;

        public void PlayDialogueEvent(AICharacterManager aiCharacter)
        {
            if (dialogueString.Count != dialogueAudio.Count)
            {
                Debug.Log("audio clip doesn't match subtitle count, missing files");
                return;
            }

            aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying = true;
            PlayerUIManager.Singleton.playerUIPopUpManager.SendDialoguePopUp(this, aiCharacter);
        }

        public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
        {
            if (greetingDialogueAudio.Count != 0 && !greetingHasPlayed)
            {
                greetingHasPlayed = true;
                int randomGreetingDialogueIndex = Random.Range(0, greetingDialogueAudio.Count);
                PlayerUIManager.Singleton.playerUIPopUpManager.SetDialoguePopUpSubtitles(greetingDialogueString[randomGreetingDialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(greetingDialogueAudio[randomGreetingDialogueIndex]);
                yield return new WaitForSeconds(greetingDialogueAudio[randomGreetingDialogueIndex].length + 1);
            }

            while (dialogueIndex < dialogueString.Count)
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(dialogueAudio[dialogueIndex]);
                yield return new WaitForSeconds(dialogueAudio[dialogueIndex].length + 1);
                dialogueIndex++;
            }

            if (farewellDialogueAudio.Count != 0 && !farewellHasPlayed)
            {
                farewellHasPlayed = true;
                int randomFarewellDialogueIndex = Random.Range(0, farewellDialogueAudio.Count);
                PlayerUIManager.Singleton.playerUIPopUpManager.SetDialoguePopUpSubtitles(farewellDialogueString[randomFarewellDialogueIndex]);
                aiCharacter.aiCharacterSoundFXManager.PlaySoundFX(farewellDialogueAudio[randomFarewellDialogueIndex]);
                yield return new WaitForSeconds(farewellDialogueAudio[randomFarewellDialogueIndex].length + 1);
            }

            OnDialogueEnded(aiCharacter);
            PlayerUIManager.Singleton.playerUIPopUpManager.EndDialoguePopUp();

            yield return null;
        }

        public void OnDialogueEnded(AICharacterManager aiCharacter)
        {
            greetingHasPlayed = false;
            dialogueIndex = 0;

            if (setStageIndex)
                WorldSaveGameManager.Singleton.SetStageOfDialogue(aiCharacter.aiCharacterSoundFXManager.characterDialogueID, stageID);

            aiCharacter.aiCharacterSoundFXManager.OnCurrentDialogueEnded();

            if (endEvent != DialogueEndEvents.None)
            {
                switch (endEvent)
                {
                    case DialogueEndEvents.None:
                        break;
                    case DialogueEndEvents.Blacksmith:
                        PlayerUIManager.Singleton.playerUIWeaponUpgradeManager.OpenMenuAfterFixedFrame();
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnDialogueCancelled(AICharacterManager aiCharacter)
        {
            switch (endEvent)
            {
                case DialogueEndEvents.None:
                    break;
                case DialogueEndEvents.Blacksmith:
                    PlayerUIManager.Singleton.playerUIWeaponUpgradeManager.CloseMenu();
                    break;
                default:
                    break;
            }
        }

    }
}