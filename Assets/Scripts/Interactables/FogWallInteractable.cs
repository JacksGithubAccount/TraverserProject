using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class FogWallInteractable : Interactable
    {
        [Header("fog")]
        [SerializeField] GameObject[] fogGameObjects;

        [Header("Collision")]
        [SerializeField] Collider fogWallCollider;

        [Header("I.D")]
        public int fogWallID;

        [Header("Sound")]
        private AudioSource fogWallAudioSource;
        [SerializeField] AudioClip fogWallSFX;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public List<CharacterManager> charactersThatHavePassedThroughThisFogwall = new List<CharacterManager>();

        protected override void Awake()
        {
            base.Awake();
            fogWallAudioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            PassThroughFogWall(player);
        }

        public void PassThroughFogWall(CharacterManager character)
        {
            //face fog wall
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
            character.transform.rotation = targetRotation * Quaternion.Euler(0, 90f, 0);

            character.characterCombatManager.lastFogWallEntered = this;

            //ignore fogwall collision
            AllowPlayerThroughFogWallCollidersServerRpc(character.NetworkObjectId);

            character.characterNetworkManager.isInvulnerable.Value = true;

            //walk through fog wall
            character.characterAnimatorManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);

            charactersThatHavePassedThroughThisFogwall.Add(character);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.Singleton.AddFogWallToList(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.Singleton.RemoveFogWallFromList(this);
        }


        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if (isActive.Value)
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(false);
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong characterObjectID)
        {
            if (IsServer)
            {
                AllowPlayerThroughFogWallCollidersClientRpc(characterObjectID);
            }
        }


        [ClientRpc]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong characterObjectID)
        {
            CharacterManager character = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterObjectID].GetComponent<CharacterManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX);

            if (character != null)
                StartCoroutine(DisableCollisionForTime(character));
        }

        private IEnumerator DisableCollisionForTime(CharacterManager character)
        {
            Physics.IgnoreCollision(character.characterController, fogWallCollider, true);

            yield return new WaitForSeconds(3);
            Physics.IgnoreCollision(character.characterController, fogWallCollider, false);

        }

    }
}