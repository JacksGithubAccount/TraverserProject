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

    }
}