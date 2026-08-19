using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        [Header("Pursuit Mode")]
        [SerializeField] PursuitMode pursuitMode;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 0);
                return this;
            }

            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 1);

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            // option 1
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.aiCharacterCombatManager.minimumDistanceToEndPursuit)
                return SwitchState(aiCharacter, aiCharacter.combatStance);

            //option 2
            //if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
            //    return SwitchState(aiCharacter, aiCharacter.combatStance);

            // if the target is not reachable, and they are far away, return home

            switch (pursuitMode)
            {
                case PursuitMode.None:  //even if vertical is set to 0, when a destination is set the AI will still walk(as long as they have a target destination away from them)
                    break;
                case PursuitMode.Walk:
                    aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 0.5f);
                    break;
                case PursuitMode.Run:
                    aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 1);
                    break;
                case PursuitMode.Sprint:
                    aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 2);
                    break;
                default:
                    break;
            }

            //pursue the target
            //option 1
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            //option 2
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}