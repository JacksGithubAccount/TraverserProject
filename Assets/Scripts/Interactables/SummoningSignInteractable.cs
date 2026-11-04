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
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            summoningSignHasBeenTouched.OnValueChanged -= OnSummoningHasBeenTouchedChanged;
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);
            TouchSummoningSignServerRpc();
        }

        private IEnumerator WaitThenSpawnFriendlyPhantomCoroutine()
        {
            yield return new WaitForSeconds(spawnDelay);

            if (IsServer)
                spawner.AttemptToSpawnCharacter();
        }

        [ServerRpc]
        private void TouchSummoningSignServerRpc()
        {
            if (IsServer)
                summoningSignHasBeenTouched.Value = true;

        }

        private void OnSummoningHasBeenTouchedChanged(bool oldStatus, bool newStatus)
        {
            if (!summoningSignHasBeenTouched.Value)
                return;

            if (waitThenSpawnPhantomCoroutine != null)
                StopCoroutine(waitThenSpawnPhantomCoroutine);

            waitThenSpawnPhantomCoroutine = StartCoroutine(WaitThenSpawnFriendlyPhantomCoroutine());

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