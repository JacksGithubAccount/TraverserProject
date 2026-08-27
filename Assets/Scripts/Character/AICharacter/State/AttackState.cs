using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Attack")]
    public class AttackState : AIState
    {
        [Header("Current Attack")]
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo = false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;

        [Header("PivotAfterAttack")]
        [SerializeField] protected bool pivotAfterAttack = false;

        [Header("Retreat")]
        [SerializeField] protected bool retreatAfterAttack = false;
        protected bool hasRetreatPosition = false;
        [SerializeField] PursuitMode retreatSpeed = PursuitMode.Sprint;
        [SerializeField] protected float minimumDistanceNeededToPerformRetreat = 5;
        private float distanceFromRetreatPosition = Mathf.Infinity;
        private Vector3 retreatPosition = Vector3.zero;


        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                return SwitchState(aiCharacter, aiCharacter.idle);

            aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilstAttacking(aiCharacter);

            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            PerformCombo(aiCharacter);

            if (aiCharacter.isPerformingAction)
                return this;


            if (!hasPerformedAttack)
            {
                PerformAttack(aiCharacter);

                return this;
            }

            if (pivotAfterAttack)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);

            if (retreatAfterAttack && !hasRetreatPosition && aiCharacter.aiCharacterCombatManager.distanceFromTarget < minimumDistanceNeededToPerformRetreat)
            {
                hasRetreatPosition = true;
                retreatPosition = aiCharacter.aiCharacterCombatManager.GetRetreatPosition();
            }

            //optional, add a check for is being damaged, if so, return to combat stance state
            if (hasRetreatPosition && distanceFromRetreatPosition > aiCharacter.navMeshAgent.stoppingDistance)
            {
                if (aiCharacter.isPerformingAction)
                    return this;

                distanceFromRetreatPosition = Vector3.Distance(aiCharacter.transform.position, retreatPosition);
                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

                switch (retreatSpeed)
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

                NavMeshPath path = new NavMeshPath();
                aiCharacter.navMeshAgent.CalculatePath(retreatPosition, path);
                aiCharacter.navMeshAgent.SetPath(path);

                return this;
            }

            return SwitchState(aiCharacter, aiCharacter.combatStance);

        }

        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }

        protected virtual void PerformCombo(AICharacterManager aiCharacter)
        {
            bool canPerformTheCombo = false;

            if (!willPerformCombo)
                return;

            if (hasPerformedCombo)
                return;

            if (currentAttack.comboAction == null)
                return;


            //if dont need to hit target, perform combo
            if (aiCharacter.aiCharacterCombatManager.canPerformCombo && !aiCharacter.combatStance.onlyPerformComboIfInitialAttackHits)
                canPerformTheCombo = true;

            //if do need to hit target and target is hit, perform combo
            if (aiCharacter.aiCharacterCombatManager.canPerformCombo && aiCharacter.combatStance.onlyPerformComboIfInitialAttackHits && aiCharacter.aiCharacterCombatManager.hasHitTargetDuringCombo)
                canPerformTheCombo = true;



            if (canPerformTheCombo)
            {
                hasPerformedCombo = true;
                currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            distanceFromRetreatPosition = Mathf.Infinity;
            retreatPosition = Vector3.zero;

            hasRetreatPosition = false;
            hasPerformedAttack = false;
            hasPerformedCombo = false;
            willPerformCombo = false;
            currentAttack = null;
        }
    }
}