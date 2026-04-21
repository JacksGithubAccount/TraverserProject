using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;

        [SerializeField] List<Interactable> currentInteractableActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            currentInteractableActions = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
                return;

            if (!PlayerUIManager.Singleton.menuWindowIsOpen && !PlayerUIManager.Singleton.popUpWindowIsOpen)
                CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] == null)
            {
                currentInteractableActions.RemoveAt(0);
                return;
            }

            if (currentInteractableActions[0] != null)
                PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }

        private void RefreshInteractionList()
        {
            for (int i = currentInteractableActions.Count - 1; i > -1; i--)
            {
                if (currentInteractableActions[i] == null)
                    currentInteractableActions.RemoveAt(i);
            }
        }

        public void AddInteractionToList(Interactable interactableObject)
        {
            RefreshInteractionList();

            if (!currentInteractableActions.Contains(interactableObject))
                currentInteractableActions.Add(interactableObject);
        }

        public void RemoveInteractionFromList(Interactable interactableObject)
        {
            bool hasOnlyThisInteraction = false;
            if (currentInteractableActions.Contains(interactableObject) && currentInteractableActions.Count == 1)
            {
                currentInteractableActions.Remove(interactableObject);
                hasOnlyThisInteraction = true;
            }


            RefreshInteractionList();

            if (hasOnlyThisInteraction)
                PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
        }

        public void Interact()
        {
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            if (currentInteractableActions.Count == 0)
                return;

            if (currentInteractableActions[0] != null)
            {
                currentInteractableActions[0].Interact(player);
                RefreshInteractionList();
            }
        }

    }
}