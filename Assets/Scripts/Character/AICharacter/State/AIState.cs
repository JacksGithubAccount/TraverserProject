using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{

    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {

            return this;
        }

        public virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
        {
            ResetStateFlags(aiCharacter);
            return newState;
        }

        //called from outside a state, triggered by other scripts or functions (such as investigate sound)
        public virtual AIState ManuallySwitchState(AICharacterManager aiCharacter, AIState newState)
        {
            aiCharacter.hasManuallySwitchedState = true;
            ResetStateFlags(aiCharacter);
            return newState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aICharacter)
        {

        }

        public virtual bool IsDestinationReachable(AICharacterManager aiCharacter, Vector3 destination)
        {
            aiCharacter.navMeshAgent.enabled = true;

            NavMeshPath navMeshPath = new NavMeshPath();

            if (aiCharacter.navMeshAgent.CalculatePath(destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool IsDestinationOccupied(Vector3 destination, float radius)
        {
            float squareRadius = radius * radius;

            //checks every active AI in world to make sure they are not occupying the same place we are attempting to check
            for (int i = 0; i < WorldAIManager.Singleton.spawnedInCharacters.Count; i++)
            {
                if (WorldAIManager.Singleton.spawnedInCharacters[i] == null)
                    continue;

                if (!WorldAIManager.Singleton.spawnedInCharacters[i].gameObject.activeInHierarchy)
                    continue;

                if (!WorldAIManager.Singleton.spawnedInCharacters[i].navMeshAgent.enabled)
                    continue;

                if (!WorldAIManager.Singleton.spawnedInCharacters[i].navMeshAgent.isOnNavMesh)
                    continue;

                if ((WorldAIManager.Singleton.spawnedInCharacters[i].transform.position - destination).sqrMagnitude <= squareRadius)
                    return true;
            }

            return false;
        }

        public bool IsACharacterInFrontOfMyPath(AICharacterManager aiCharacter)
        {
            Vector3 origin = aiCharacter.transform.position + Vector3.up;
            float maxDistanceCheck = 3;

            if (Physics.SphereCast(origin, aiCharacter.characterController.radius, aiCharacter.transform.forward, out RaycastHit hit, maxDistanceCheck, WorldUtilityManager.Singleton.GetCharacterLayers(), QueryTriggerInteraction.Ignore))
            {
                CharacterController controller = hit.collider.GetComponent<CharacterController>();

                if (controller == null)
                    return false;

                if (controller.transform == aiCharacter.transform)
                    return false;

                return true;
            }

            return false;
        }

        public bool IsMySummonerInABossBattleThatIHaveNotJoined(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.followTarget.characterCombatManager.lastFogWallEntered != null
                   && aiCharacter.aiCharacterCombatManager.followTarget.characterCombatManager.lastFogWallEntered.isActive.Value
                   && !aiCharacter.aiCharacterCombatManager.followTarget.characterCombatManager.lastFogWallEntered.charactersThatHavePassedThroughThisFogwall.Contains(aiCharacter))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void MoveToBossFight(AICharacterManager aiCharacter, FogWallInteractable fogwall)
        {
            aiCharacter.navMeshAgent.enabled = true;

            Vector3 targetPosition = aiCharacter.aiCharacterCombatManager.followTarget.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fogwall.transform.position, out hit, 5, NavMesh.AllAreas))
                targetPosition = hit.position;

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(targetPosition, path);
            aiCharacter.navMeshAgent.SetPath(path);
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 1);
            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            Collider[] fogWallColliders = Physics.OverlapSphere(aiCharacter.transform.position,
            aiCharacter.navMeshAgent.stoppingDistance * 1.1f, WorldUtilityManager.Singleton.GetInteractableLayers());

            FogWallInteractable potentialFogWall;

            for (int i = 0; i < fogWallColliders.Length; i++)
            {
                potentialFogWall = fogWallColliders[i].GetComponentInParent<FogWallInteractable>();

                if (potentialFogWall == null)
                    continue;

                if (potentialFogWall.fogWallID != fogwall.fogWallID)
                    continue;

                potentialFogWall.PassThroughFogWall(aiCharacter);
                break;
            }
        }

    }
}