using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager Singleton;


        [Header("Debug")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool respawnCharacters = false;


        [Header("Characters")]
        [SerializeField] GameObject[] aiCharacters;
        [SerializeField] List<GameObject> spawnedInCharacters;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
            }
        }

        private void Update()
        {
            if (despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
            if (respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }

            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach (var character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedInCharacters.Add(instantiatedCharacter);
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void DisableAllCharacters()
        {

        }

    }
}