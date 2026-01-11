using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    public class BackDoorInteractable : Interactable
    {
        [SerializeField] DoorInteractable door;
        public List<PlayerManager> playersWithinInteractionTrigger = new List<PlayerManager>();

        private Coroutine waitForElevatorTravelCoroutine;



        public override void Interact(PlayerManager player)
        {
            door.ActivateDoorServerRpc();

        }

        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
                AddCharacterToListOfCharactersBehindDoor(player);
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
                RemoveCharacterFromListOfCharactersBehindDoor(player);
        }

        public void AddCharacterToListOfCharactersBehindDoor(PlayerManager player)
        {
            //null checker and removes
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    playersWithinInteractionTrigger.RemoveAt(i);
            }


            if (playersWithinInteractionTrigger.Contains(player))
                return;

            playersWithinInteractionTrigger.Add(player);

            if (waitForElevatorTravelCoroutine != null)
                StopCoroutine(waitForElevatorTravelCoroutine);

            waitForElevatorTravelCoroutine = StartCoroutine(CheckForCharactersInTrigger());
        }

        public void RemoveCharacterFromListOfCharactersBehindDoor(PlayerManager player)
        {
            if (!playersWithinInteractionTrigger.Contains(player))
                return;

            playersWithinInteractionTrigger.Remove(player);

            //null checker and removes
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    playersWithinInteractionTrigger.RemoveAt(i);
            }
        }

        private IEnumerator CheckForCharactersInTrigger()
        {
            while (door.doorIsOpening.Value || door.doorIsClosing.Value)
                yield return null;

            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }

        public void RemoveInteractionFromPlayers()
        {
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                if (!playersWithinInteractionTrigger[i].IsOwner)
                    continue;

                playersWithinInteractionTrigger[i].playerInteractionManager.RemoveInteractionFromList(this);
            }
        }

        public void ReturnInteractionToPlayers()
        {
            for (int i = 0; i < playersWithinInteractionTrigger.Count; i++)
            {
                if (playersWithinInteractionTrigger[i] == null)
                    continue;

                if (!playersWithinInteractionTrigger[i].IsOwner)
                    continue;

                playersWithinInteractionTrigger[i].playerInteractionManager.AddInteractionToList(this);
            }
        }
    }
}
