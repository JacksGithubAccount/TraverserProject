using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class DoorInteractable : Interactable
    {
        [Header("Network Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> doorIsOpening = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> doorIsClosing = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [SerializeField] float networkPositionSmoothTime = 0.1f;

        [Header("Door Pivot Transform")]
        public GameObject doorPivotTransform;

        [Header("Lock")]
        public int doorID;
        public DoorState doorState = DoorState.Open;
        public KeyItem keyToUnlockDoor;
        [SerializeField] string unlockMessage = "Used KeyItemNameHere";
        [SerializeField] string lockedMessage = "It's locked";
        [SerializeField] string doesNotOpenFromThisSideMessage = "Does not open from this side";

        [Header("Destination")]
        [SerializeField] float moveSpeed = 2;
        [SerializeField] float moveMagnitude = 10;
        public Vector3 destinationOpen;
        public Vector3 destinationClose;

        [Header("Characters In Front Of Door")]
        [SerializeField] protected List<CharacterManager> charactersInFrontOfDoor = new List<CharacterManager>();

        [Header("Back of door Location")]
        [SerializeField] BackDoorInteractable backDoorInteractable;

        [Header("SFX")]
        private AudioSource doorAudioSource;
        [SerializeField] private AudioClip doorOpeningSFX;
        [SerializeField] private AudioClip[] doorClosingSFX;

        protected override void Awake()
        {
            base.Awake();

            doorAudioSource = GetComponent<AudioSource>();
        }

        public override void OnTriggerEnter(Collider other)
        {
            if (doorIsOpening.Value || doorIsClosing.Value)
                return;

            base.OnTriggerEnter(other);
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (player.IsOwner)
                ActivateDoorServerRpc();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                transform.localPosition = networkPosition.Value;
            }
            else
            {
                networkPosition.Value = transform.localPosition;
            }

            if (doorIsOpening.Value)
                ActivateDoor(true);

            if (doorIsClosing.Value)
                ActivateDoor(false);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        private void ActivateDoor(bool isOpening, bool isAtSideThatCantOpen = false)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            switch (doorState)
            {
                case DoorState.Open:
                    StartCoroutine(MoveDoorCoroutine(isOpening));
                    break;
                case DoorState.Locked:
                    if (player == null)
                        break;
                    if (keyToUnlockDoor == null)
                        break;

                    if (player.playerInventoryManager.itemsInInventory.Contains(keyToUnlockDoor))
                    {
                        unlockMessage = "Used " + keyToUnlockDoor.name;
                        PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp(unlockMessage);
                        doorState = DoorState.Open;
                        StartCoroutine(MoveDoorCoroutine(isOpening));
                    }
                    else
                    {
                        PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp(lockedMessage);
                        interactableCollider.enabled = true;
                    }

                    break;

                case DoorState.CantOpenFromThisSide:
                    if (isAtSideThatCantOpen)
                        PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp(doesNotOpenFromThisSideMessage);
                    else
                    {                        
                        doorState = DoorState.Open;
                        StartCoroutine(MoveDoorCoroutine(isOpening));
                    }
                    break;
            }
            
        }

        private IEnumerator MoveDoorCoroutine(bool isOpening)
        {
            interactableCollider.enabled = false;

            if (!isOpening)
            {
                if (IsOwner)
                    doorIsClosing.Value = true;
            }else
            {
                if (IsOwner)
                    doorIsOpening.Value = true;
            }

                //when elevator starts, remove it as an interable whilst it is going
                for (int i = 0; i < charactersInFrontOfDoor.Count; i++)
                {
                    if (charactersInFrontOfDoor[i] == null)
                        continue;

                    PlayerManager player = charactersInFrontOfDoor[i] as PlayerManager;

                    if (player == null)
                        continue;

                    player.playerInteractionManager.RemoveInteractionFromList(this);
                }

            //SFX
            doorAudioSource.clip = doorOpeningSFX;
            doorAudioSource.Play();

            //Determines destination
            Vector3 destination = destinationOpen;
            if (!isOpening)
                destination = destinationClose;

            backDoorInteractable.RemoveInteractionFromPlayers();

            //moves the elevator
            while (doorPivotTransform.transform.localPosition != destination)
            {
                doorPivotTransform.transform.localEulerAngles = Vector3.RotateTowards(doorPivotTransform.transform.localEulerAngles, destination, moveSpeed * Time.deltaTime, moveMagnitude * Time.deltaTime);
                Vector3 velocityOfMovement = Vector3.RotateTowards(doorPivotTransform.transform.eulerAngles, destination, moveSpeed * Time.deltaTime, moveMagnitude * Time.deltaTime);

                if (IsOwner)
                    networkPosition.Value = doorPivotTransform.transform.localEulerAngles;

                for (int i = 0; i < charactersInFrontOfDoor.Count; i++)
                {
                    if (charactersInFrontOfDoor[i] == null)
                        continue;

                    if (!charactersInFrontOfDoor[i].gameObject.activeInHierarchy)
                        RemoveCharacterFromListOfCharactersInFrontOfDoor(charactersInFrontOfDoor[i]);

                    //If using foot IK, disable here temporarily. it may cause weird artifacts with feet otherwise

                    if (!charactersInFrontOfDoor[i].characterNetworkManager.isJumping.Value)
                        charactersInFrontOfDoor[i].transform.position = new Vector3(charactersInFrontOfDoor[i].transform.position.x, velocityOfMovement.y, charactersInFrontOfDoor[i].transform.position.z);

                }
                yield return null;
            }

            //stops movement flags
            if (IsOwner)
            {
                doorIsOpening.Value = false;
                doorIsClosing.Value = false;
            }

            backDoorInteractable.ReturnInteractionToPlayers();

            //stops movement SFX
            doorAudioSource.Stop();
            //plays stopped SFX
            doorAudioSource.PlayOneShot(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(doorClosingSFX));

            //if animating elevator, stop animation here

            //re-enable interaction with elevator
            interactableCollider.enabled = true;

            yield return null;
        }

        
        public void AddCharacterToListOfCharactersInFrontOfDoor(CharacterManager character)
        {
            if (charactersInFrontOfDoor.Contains(character))
                return;

            charactersInFrontOfDoor.Add(character);
            character.characterLocomotionManager.isOpeningDoor = true;
        }

        public void RemoveCharacterFromListOfCharactersInFrontOfDoor(CharacterManager character)
        {
            if (!charactersInFrontOfDoor.Contains(character))
                return;

            charactersInFrontOfDoor.Remove(character);
            character.characterLocomotionManager.isOpeningDoor = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ActivateDoorServerRpc(bool isBackdoor = false)
        {
            if (IsServer)
                ActivateDoorClientRpc(isBackdoor);
        }

        [ClientRpc]
        private void ActivateDoorClientRpc(bool isBackdoor = false)
        {
            if (transform.localEulerAngles == destinationOpen)
            {
                

                ActivateDoor(false, isBackdoor);
            }
            else if (transform.localEulerAngles == destinationClose)
            {
                

                ActivateDoor(true, isBackdoor);
            }
        }
    }
}
