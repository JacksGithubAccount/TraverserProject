using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class DialogInteractable : Interactable
    {
        AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponentInParent<AICharacterManager>();
        }

        public override void Interact(PlayerManager player)
        {
            if (PlayerUIManager.Singleton.menuWindowIsOpen)
                return;

            if (aiCharacter.isDead.Value)
            {
                interactableCollider.enabled = false;
                return;
            }

            if (NetworkManager.Singleton.IsServer)
            {
                WorldSaveGameManager.Singleton.SaveGame();
            }

            aiCharacter.aiCharacterSoundFXManager.PlayCurrentDialogueEvent();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (aiCharacter.isDead.Value)
            {
                interactableCollider.enabled = false;

                PlayerManager player = other.GetComponent<PlayerManager>();

                if (player != null && player.IsOwner)
                    aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
            }

            base.OnTriggerEnter(other);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
        }

    }
}