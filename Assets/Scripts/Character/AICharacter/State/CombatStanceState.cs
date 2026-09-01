using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Combat Stance")]
    public class CombatStanceState : AIState
    {
        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;
        public List<AICharacterAttackAction> potentialAttacks;
        public AICharacterAttackAction choosenAttack;
        public AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false;
        [SerializeField] protected int percentageOfTimeWillPerformCombo = 25;
        [SerializeField] public bool onlyPerformComboIfInitialAttackHits = false;
        protected bool hasRolledForComboChance = false;

        [Header("Circling")]
        [SerializeField] StrafeMode strafeMode = StrafeMode.None;
        [SerializeField] private float minimumDistanceNeededToAvoidStrafeBackwards = 5;
        private bool hasChosenStrafeDirection = false;
        private float strafeMoveAmount;

        [Header("Movement Values")]
        private float horizontalMovement;
        private float verticalMovement;

        [Header("Blocking")]
        [SerializeField] bool canBlock = false;
        [SerializeField] int percentageOfTimeWillBlock = 75;
        private bool hasRolledForBlockChance = false;
        private bool willBlockDuringThisCombatRotation;

        [Header("Evasion")]
        [SerializeField] bool canEvade = false;
        [SerializeField] int percentageOfTimeWillEvade = 75;
        private bool hasEvaded = false;
        private bool hasRolledForEvasionChance = false;
        private bool willEvadeDuringThisCombatRotation;

        [Header("Pursuit Mode")]
        [SerializeField] PursuitMode pursuitMode;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return this;

            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
                aiCharacter.aiCharacterCombatManager.SetTarget(null);

            //turns and face towards target when target is outside FOV
            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

            //rotate to face target
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

            switch (pursuitMode)
            {
                case PursuitMode.None:
                    horizontalMovement = 0;
                    verticalMovement = 0;
                    break;
                case PursuitMode.Walk:
                    horizontalMovement = 0;
                    verticalMovement = 0.5f;
                    break;
                case PursuitMode.Run:
                    horizontalMovement = 0;
                    verticalMovement = 1;
                    break;
                case PursuitMode.Sprint:
                    horizontalMovement = 0;
                    verticalMovement = 2;
                    break;
                default:
                    break;
            }

            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (strafeMode != StrafeMode.None)
                SetCirclePath(aiCharacter);

            if (canBlock && !hasRolledForBlockChance)
            {
                hasRolledForBlockChance = true;
                willBlockDuringThisCombatRotation = RollForOutcomeChance(percentageOfTimeWillBlock);
            }

            if (canEvade && !hasRolledForEvasionChance)
            {
                hasRolledForEvasionChance = true;
                willEvadeDuringThisCombatRotation = RollForOutcomeChance(percentageOfTimeWillEvade);
            }

            if (canPerformCombo && !hasRolledForComboChance)
            {
                hasRolledForComboChance = true;
                aiCharacter.attack.willPerformCombo = RollForOutcomeChance(percentageOfTimeWillPerformCombo);
            }

            if (willBlockDuringThisCombatRotation)
                aiCharacter.aiCharacterNetworkManager.isBlocking.Value = true;

            if (willEvadeDuringThisCombatRotation && aiCharacter.aiCharacterCombatManager.currentTarget.characterNetworkManager.isAttacking.Value && !hasEvaded)
            {
                hasEvaded = true;
                aiCharacter.aiCharacterCombatManager.PerformEvasion();
            }

            //if we do not have an attack, get one
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                //roll for combo chance
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            //if we are outside of the combat engagement distance, switch to pursue target state
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > aiCharacter.aiCharacterCombatManager.maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(horizontalMovement, verticalMovement);

            if (pursuitMode == PursuitMode.None)
            {
                aiCharacter.navMeshAgent.SetDestination(aiCharacter.transform.position);
                return this;
            }

            //calculates path towards target and moves towards it
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;

        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                return;

            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in aiCharacterAttacks)
            {
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;

                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                if (aiCharacter.aiCharacterCombatManager.currentTarget != null && potentialAttack.requireClearLineOfSight && !aiCharacter.aiCharacterCombatManager.HasLineOfSight())
                    continue;

                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if (randomWeightValue <= processedWeight)
                {
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
                outcomeWillBePerformed = true;

            return outcomeWillBePerformed;
        }

        protected virtual void SetCirclePath(AICharacterManager aiCharacter)
        {
            /*
            if (Physics.CheckSphere(aiCharacter.aiCharacterCombatManager.lockOnTransform.position, aiCharacter.characterController.radius + 0.25f, WorldUtilityManager.Singleton.GetEnviroLayers()))
            {
                //stop strafing as we hit something
                //aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, Mathf.Abs(strafeMoveAmount));
                horizontalMovement = 0;
                verticalMovement = Mathf.Abs(strafeMoveAmount);
                return;
            }*/


            switch (strafeMode)
            {
                case StrafeMode.None:
                    break;
                case StrafeMode.Standard:
                    horizontalMovement = strafeMoveAmount;
                    verticalMovement = 0;
                    break;
                case StrafeMode.Avoidance:
                    horizontalMovement = 0;
                    verticalMovement = strafeMoveAmount;
                    break;
                default:
                    break;
            }

            if (hasChosenStrafeDirection)
                return;

            hasChosenStrafeDirection = true;

            //strafe left or right
            int leftOrRightIndex = Random.Range(0, 100);

            switch (pursuitMode)
            {
                case PursuitMode.None:
                    strafeMoveAmount = 0;
                    break;
                case PursuitMode.Walk:
                    strafeMoveAmount = 0.5f;
                    break;
                case PursuitMode.Run:
                    strafeMoveAmount = 1;
                    break;
                case PursuitMode.Sprint:
                    strafeMoveAmount = 2;
                    break;
                default:
                    break;
            }

            //set value to negative if left movement or backwards
            if (leftOrRightIndex >= 50 || strafeMode == StrafeMode.Avoidance)
                strafeMoveAmount *= -1;

            //if target is far away enough and we are set to avoid, instead do not move
            if (strafeMode == StrafeMode.Avoidance && aiCharacter.aiCharacterCombatManager.distanceFromTarget > minimumDistanceNeededToAvoidStrafeBackwards)
                strafeMoveAmount = 0;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasRolledForEvasionChance = false;
            hasRolledForComboChance = false;
            hasRolledForBlockChance = false;
            willBlockDuringThisCombatRotation = false;
            hasChosenStrafeDirection = false;
            strafeMoveAmount = 0;
            hasAttack = false;
            hasEvaded = false;
        }

    }
}