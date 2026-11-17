using UnityEngine;
using System.Collections;
using Unity.Netcode;

namespace TraverserProject
{

    public class SummoningSignInteractable : Interactable
    {
        [Header("Status")]
        public NetworkVariable<bool> summoningSignHasBeenTouched = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Settings")]
        [SerializeField] float spawnDelay = 5;

        [Header("Character")]
        [SerializeField] AICharacterSpawner spawner;

        private Coroutine waitThenSpawnPhantomCoroutine;

        [Header("Sign")]
        [SerializeField] GameObject[] mesh;
        [SerializeField] ParticleSystem[] particles;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            summoningSignHasBeenTouched.OnValueChanged += OnSummoningHasBeenTouchedChanged;

            OnSummoningHasBeenTouchedChanged(false, summoningSignHasBeenTouched.Value);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            summoningSignHasBeenTouched.OnValueChanged -= OnSummoningHasBeenTouchedChanged;
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            TouchSummoningSignServerRpc(player.NetworkObjectId);
        }

        private IEnumerator WaitThenSpawnFriendlyPhantomCoroutine(ulong playerNetworkObjectID)
        {
            //gets name
            /*AICharacterManager aiCharacter = spawner.characterGameObject.GetComponent<AICharacterManager>();
			string phantomName = "";
			
			if(aiCharacter != null)
				phantomName = aiCharacter.characterName;*/

            PlayerUIManager.Singleton.playerUIPopUpManager.SendTemporaryPopUp("Summoning Friendly Phantom");

            yield return new WaitForSeconds(spawnDelay);

            PlayerManager summoningPlayer = null;

            if (NetworkManager.SpawnManager.SpawnedObjects.ContainsKey(playerNetworkObjectID))
                summoningPlayer = NetworkManager.SpawnManager.SpawnedObjects[playerNetworkObjectID].GetComponent<PlayerManager>();

            if (summoningPlayer == null)
            {
                for (int i = 0; i < WorldGameSessionManager.Singleton.players.Count; i++)
                {
                    if (WorldGameSessionManager.Singleton.players[i] == null)
                        continue;

                    if (WorldGameSessionManager.Singleton.players[i].IsHost)
                        summoningPlayer = WorldGameSessionManager.Singleton.players[i];
                }
            }

            if (IsServer)
                spawner.AttemptToSpawnCharacter();

            while (!spawner.CharacterIsSpawned())
            {
                yield return new WaitForEndOfFrame();
            }

            spawner.aiCharacter.aiCharacterCombatManager.followTarget = summoningPlayer;
        }

        [ServerRpc]
        private void TouchSummoningSignServerRpc(ulong playerNetworkObjectID)
        {
            if (IsServer)
            {
                summoningSignHasBeenTouched.Value = true;
                TouchSummoningSignClientRpc(playerNetworkObjectID);
            }

        }
        [ClientRpc]
		private void TouchSummoningSignClientRpc(ulong playerNetworkObjectID)
        {
            if (waitThenSpawnPhantomCoroutine != null)
                StopCoroutine(waitThenSpawnPhantomCoroutine);

            waitThenSpawnPhantomCoroutine = StartCoroutine(WaitThenSpawnFriendlyPhantomCoroutine(playerNetworkObjectID));
        }

        private void OnSummoningHasBeenTouchedChanged(bool oldStatus, bool newStatus)
        {
            if (!summoningSignHasBeenTouched.Value)
                return;



            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i] == null)
                    continue;

                particles[i].Stop();
            }

            for (int i = 0; i < mesh.Length; i++)
            {
                if (mesh[i] == null)
                    continue;

                mesh[i].SetActive(false);
            }
        }



    }
}