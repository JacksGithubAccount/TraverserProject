using UnityEngine;
using Unity.Netcode;
using System.Collections;

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



        protected override void Awake()
        {
            base.Awake();
            fogWallAudioSource = GetComponent<AudioSource>();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            //face fog wall
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.right);
            player.transform.rotation = targetRotation;

            //ignore fogwall collision
            AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);

            //walk through fog wall
            player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Through_Fog_01", true);
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
        private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)
        {
            if (IsServer)
            {
                AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
            }
        }


        [ClientRpc]
        private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX);

            if (player != null)
                StartCoroutine(DisableCollisionForTime(player));
        }

        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

            yield return new WaitForSeconds(3);
            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);

        }

    }
}