using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class EventTriggerWakeNearbyCharacters : MonoBehaviour
    {
        [SerializeField] float awakenRadius = 8;

        private void OnTriggerEnter(Collider other)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            Collider[] creaturesInRadius = Physics.OverlapSphere(transform.position, awakenRadius, WorldUtilityManager.Singleton.GetCharacterLayers());
            List<AICharacterManager> creaturesToWake = new List<AICharacterManager>();

            for (int i = 0; i < creaturesInRadius.Length; i++)
            {
                AICharacterManager aiCharacter = creaturesInRadius[i].GetComponentInParent<AICharacterManager>();

                if (aiCharacter == null)
                    continue;

                if (aiCharacter.isDead.Value)
                    continue;

                if (aiCharacter.aiCharacterNetworkManager.isAwake.Value)
                    continue;

                if (!creaturesToWake.Contains(aiCharacter))
                    creaturesToWake.Add(aiCharacter);
            }

            for (int i = 0; i < creaturesToWake.Count; i++)
            {
                creaturesToWake[i].aiCharacterNetworkManager.isAwake.Value = true;
                creaturesToWake[i].characterAnimatorManager.PlayTargetActionAnimation(creaturesToWake[i].aiCharacterNetworkManager.wakingAnimation.Value.ToString(), true);
                creaturesToWake[i].aiCharacterCombatManager.SetTarget(player);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, awakenRadius);
        }

    }
}