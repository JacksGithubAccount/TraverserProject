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



        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
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




        public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(aiCharacterSpawner);
                aiCharacterSpawner.AttemptToSpawnCharacter();
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