using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class Door2Interactable : Interactable
    {
        [Header("Status")]
        public NetworkVariable<bool> isOpen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] private string doorID;

        [Header("Requirements")]
        [SerializeField] private bool requiresItem = false;
        [SerializeField] private Item itemRequiredToOpen;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string openDoorAnimation;
        [SerializeField] private string openedDoorAnimation;
        [SerializeField] private string closeDoorAnimation;

        [Header("SFX")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] private AudioClip doorOpeningSFX;

        [Header("Levers & Buttons")]
        [SerializeField] ActivateOtherInteractableInteractable[] leversAndButtons;

        [Header("Cannot Open From This Side")]
        [SerializeField]
        MessageInteractable cannotOpenFromThisSideInteractable;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //providing your doors are not named the same, this will generate a unique ID for them
            doorID = gameObject.scene.buildIndex + " " + gameObject.name;

            //checks if host has already opened doorID
            if (NetworkManager.Singleton.IsServer)
            {
                for (int i = 0; i < WorldSaveGameManager.Singleton.currentCharacterData.doorsOpenedAlt.Count; i++)
                {
                    if (WorldSaveGameManager.Singleton.currentCharacterData.doorsOpenedAlt[i] == null)
                        continue;

                    if (WorldSaveGameManager.Singleton.currentCharacterData.doorsOpenedAlt[i] == doorID)
                        isOpen.Value = true;

                }
            }

            isOpen.OnValueChanged += OnIsOpenChanged;

            CheckIfDoorIsAlreadyOpened();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isOpen.OnValueChanged -= OnIsOpenChanged;
        }

        private void DisableDoorInteractions()
        {
            if (interactableCollider != null)
                interactableCollider.enabled = false;

            for (int i = 0; i > leversAndButtons.Length; i++)
            {
                if (leversAndButtons[i] == null)
                    continue;

                leversAndButtons[i].interactableCollider.enabled = false;
                PlayerUIManager.Singleton.localPlayer.playerInteractionManager.RemoveInteractionFromList(leversAndButtons[i]);
            }

            if (cannotOpenFromThisSideInteractable != null)
            {
                cannotOpenFromThisSideInteractable.interactableCollider.enabled = false;
                PlayerUIManager.Singleton.localPlayer.playerInteractionManager.RemoveInteractionFromList(cannotOpenFromThisSideInteractable);
            }
        }

        private void OnIsOpenChanged(bool oldStatus, bool newStatus)
        {
            if (isOpen.Value)
            {
                DisableDoorInteractions();
            }
        }

        public override void Interact(PlayerManager player)
        {
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            WorldSaveGameManager.Singleton.SaveGame();

            if (requiresItem && PlayerHasKey(player))
            {
                OpenDoorServerRpc();
                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp("Used " + itemRequiredToOpen.itemName + ".");
                player.playerInventoryManager.RemoveItemFromInventory(itemRequiredToOpen);
                return;
            }
            else if (requiresItem && !PlayerHasKey(player))
            {
                PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp("It's locked");
                return;
            }

            OpenDoorServerRpc();
            player.playerInteractionManager.RemoveInteractionFromList(this);

        }

        private void CheckIfDoorIsAlreadyOpened()
        {
            if (isOpen.Value)
            {
                animator.Play(openedDoorAnimation);
                DisableDoorInteractions();
            }

        }

        private bool PlayerHasKey(PlayerManager player)
        {
            bool hasKey = false;

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                if (player.playerInventoryManager.itemsInInventory[i] == null)
                    continue;

                if (player.playerInventoryManager.itemsInInventory[i].itemID == itemRequiredToOpen.itemID)
                    hasKey = true;
            }

            return hasKey;
        }

        [ServerRpc(RequireOwnership = false)]
        private void OpenDoorServerRpc()
        {
            if (IsServer)
            {
                isOpen.Value = true;

                if (!WorldSaveGameManager.Singleton.currentCharacterData.doorsOpenedAlt.Contains(doorID))
                    WorldSaveGameManager.Singleton.currentCharacterData.doorsOpenedAlt.Add(doorID);

                OpenDoorClientRpc();
            }
        }

        [ClientRpc]
        private void OpenDoorClientRpc()
        {
            animator.Play(openDoorAnimation);
            audioSource.PlayOneShot(doorOpeningSFX);

            if (interactableCollider != null)
                interactableCollider.enabled = false;
        }
    }
}