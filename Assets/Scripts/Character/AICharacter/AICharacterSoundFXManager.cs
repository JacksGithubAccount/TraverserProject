using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        AICharacterManager aiCharacter;

        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        [Header("Dialogue")]
        public CharacterDialogueID characterDialogueID;
        public CharacterMenuDialogueID characterMenuDialogueID;
        public GameObject interactableDialogueCollider;
        public CharacterDialogue currentDialogue;
        public CharacterDialogue menuDialogue;
        public GameObject interactableDialogueObject;
        public bool dialogueIsPlaying = false;
        public bool menuDialogueIsPlaying = false;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        protected override void Start()
        {
            base.Start();

            if (characterDialogueID != CharacterDialogueID.NoDialogueID)
            {
                currentDialogue = WorldSaveGameManager.Singleton.GetCharacterDialogueByEnum(characterDialogueID);
                interactableDialogueObject = Instantiate(WorldAIManager.Singleton.dialogueInteractable, transform);
                NetworkObject networkObject = interactableDialogueObject.GetComponent<NetworkObject>();
                networkObject.Spawn();
                networkObject.TrySetParent(gameObject, true);
            }
            if(characterMenuDialogueID != CharacterMenuDialogueID.NoDialogueID)
            {
                menuDialogue = WorldSaveGameManager.Singleton.GetCharacterMenuDialogueByEnum(characterMenuDialogueID);
            }
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

        public void PlayCurrentMenuDialogueEvent()
        {
            if (menuDialogue == null)
                return;

            menuDialogueIsPlaying = true;
            if (!dialogueIsPlaying)
            {
                menuDialogue.PlayDialogueEvent(aiCharacter);
            }
            else
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SendNextDialoguePopUpInIndex(menuDialogue, aiCharacter);
            }
        }



        public void CancelCurrentDialogueEvent()
        {
            if (dialogueIsPlaying)
            {
                dialogueIsPlaying = false;
                menuDialogueIsPlaying = false;
                PlayerUIManager.Singleton.playerUIPopUpManager.CancelDialoguePopUp(aiCharacter);
            }
        }

        public void OnCurrentDialogueEnded()
        {
            currentDialogue = WorldSaveGameManager.Singleton.GetCharacterDialogueByEnum(characterDialogueID);
        }


    }
}