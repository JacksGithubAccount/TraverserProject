using System.Globalization;
using UnityEngine;
using UnityEngine.AI;

namespace TraverserProject
{

    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string characterName = "";

        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

        [Header("Current State")]
        [SerializeField] protected AIState currentState;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;

        protected override void Awake()
        {
            base.Awake();

            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();


        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            //use copy of scriptable objects so the original is not modified
            if (IsOwner)
            {
                idle = Instantiate(idle);
                pursueTarget = Instantiate(pursueTarget);

                currentState = idle;
            }
        }

        protected override void Update()
        {
            base.Update();

            aiCharacterCombatManager.HandleActionRecovery(this);
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (IsOwner)
                ProcessStateMachine();
        }

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }

            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if (aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Singleton.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
            }

            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if (remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }

    }
}