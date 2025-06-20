using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            if(aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
                return SwitchState(aiCharacter, aiCharacter.combatStance);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}