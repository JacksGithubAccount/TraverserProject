using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiatedGameObject;
        private AICharacterManager aiCharacter;

        [Header("Patrol")]
        [SerializeField] bool hasPatrolPath = false;
        [SerializeField] int patrolPathID = 0;

        [Header("Sleep")]
        [SerializeField] bool isSleeping = false;

        private void Awake()
        {

        }

        private void Start()
        {
            WorldAIManager.Singleton.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
                aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();

                if (aiCharacter == null)
                    return;

                WorldAIManager.Singleton.AddCharacterToSpawnedCharactersList(aiCharacter);

                if (hasPatrolPath)
                    aiCharacter.idle.aiPatrolPath = WorldAIManager.Singleton.GetAIPatrolPathByID(patrolPathID);

                if (isSleeping)
                    aiCharacter.aiCharacterNetworkManager.isAwake.Value = false;

                aiCharacter.aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void ResetCharacter()
        {
            if (instantiatedGameObject == null)
                return;

            if (aiCharacter == null)
                return;

            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.aiCharacterNetworkManager.currentHealth.Value = aiCharacter.aiCharacterNetworkManager.maxHealth.Value;
            aiCharacter.aiCharacterCombatManager.SetTarget(null);

            if (aiCharacter.isDead.Value)
            {
                aiCharacter.isDead.Value = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true, true, true);
                aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.idle);
            }

            aiCharacter.characterUIManager.ResetCharacterHPBar();


        }
    }
}