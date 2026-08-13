using System.Collections.Generic;
using TraverserProject;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{

    public class AICharacterCombatManager : CharacterCombatManager
    {

        protected AICharacterManager aiCharacter;

        [Header("Damage")]
        [SerializeField] protected int baseDamage = 25;
        [SerializeField] protected int basePoiseDamage = 25;

        [Header("Action Recovery")]
        public float actionRecoveryTimer = 0f;

        [Header("Pivot")]
        public bool enablePivot = true;

        [Header("Combo")]
        public bool canPerformCombo = false;

        [Header("Hit Check")]
        public bool hasHitTargetDuringCombo = false;

        [Header("Target Information")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetsDirection;

        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        public float minimumFOV = -35;
        public float maximumFOV = 35;

        [Header("Attack Rotation Speed")]
        public float attackRotationSpeed = 25;

        [Header("Stance Settings")]
        public float maxStance = 150;
        [SerializeField] float stanceRegeneratedPerSecond = 15;
        [SerializeField] bool ignoreStanceBreak = false;

        [Header("Stance Timer")]
        [SerializeField] float stanceRegenerationTimer = 0;
        private float stanceTickTimer = 0;
        [SerializeField] float defaultTimeUntilStanceRegenerationBegins = 15;

        [Header("Activation Range")]
        public List<PlayerManager> playersWithinActivationRange = new List<PlayerManager>();


        //is on world utility manager, but can be placed here if want for different values for different ai
        //public float hiddenTargetDetectionRadiusPenalty = 0.5f;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        private void Update()
        {
            HandleStanceBreak();

        }

        public void AddPlayerToPlayersWithinRange(PlayerManager player)
        {
            if (playersWithinActivationRange.Contains(player))
                return;

            playersWithinActivationRange.Add(player);

            for (int i = 0; i < playersWithinActivationRange.Count; i++)
            {
                if (playersWithinActivationRange[i] == null)
                    playersWithinActivationRange.RemoveAt(i);
            }
        }

        public void RemovePlayerFromPlayersWithinRange(PlayerManager player)
        {
            if (!playersWithinActivationRange.Contains(player))
                return;

            playersWithinActivationRange.Remove(player);

            for (int i = 0; i < playersWithinActivationRange.Count; i++)
            {
                if (playersWithinActivationRange[i] == null)
                    playersWithinActivationRange.RemoveAt(i);
            }
        }

        public void AwardRunesOnDeath(PlayerManager player)
        {
            //checks if player is friendly to host (not an invader)
            if (player.characterGroup == CharacterGroup.Team02)
                return;

            //if want to give different rune amount to client vs host, is here
            //if(NetworkManager.Singleton.IsHost)
            //{

            //}

            //add runes, also if multipliers to runes is applicable, put here
            player.playerStatsManager.AddBubbles(aiCharacter.characterStatsManager.runesDroppedOnDeath);
        }

        private void HandleStanceBreak()
        {
            if (!aiCharacter.IsOwner)
                return;

            if (aiCharacter.isDead.Value)
                return;

            if (stanceRegenerationTimer > 0)
            {
                stanceRegenerationTimer -= Time.deltaTime;
            }
            else
            {
                stanceRegenerationTimer = 0;

                if (aiCharacter.aiCharacterNetworkManager.currentStance.Value < maxStance)
                {
                    stanceTickTimer += Time.deltaTime;
                    if (stanceTickTimer >= 1)
                    {
                        stanceTickTimer = 0;
                        aiCharacter.aiCharacterNetworkManager.currentStance.Value += stanceRegeneratedPerSecond;
                    }
                }
                else
                {
                    aiCharacter.aiCharacterNetworkManager.currentStance.Value = maxStance;
                }
            }

            if (aiCharacter.aiCharacterNetworkManager.currentStance.Value <= 0)
            {
                DamageIntensity previousDamageIntensity = WorldUtilityManager.Singleton.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

                if (previousDamageIntensity == DamageIntensity.Colossal)
                {
                    aiCharacter.aiCharacterNetworkManager.currentStance.Value = 1;
                    return;
                }

                aiCharacter.aiCharacterNetworkManager.currentStance.Value = maxStance;

                if (ignoreStanceBreak)
                    return;

                aiCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Stance_Break_01", true);
            }
        }

        public void DamageStance(int stanceDamage)
        {
            //Don't allow stance break animations to play if they are being riposted/backstabbed
            if (aiCharacter.aiCharacterNetworkManager.isBeingCriticallyDamaged.Value)
            {
                if (aiCharacter.IsOwner)
                    aiCharacter.aiCharacterNetworkManager.currentStance.Value = maxStance;
                return;
            }

            //timer is reset when stance is damaged
            stanceRegenerationTimer = defaultTimeUntilStanceRegenerationBegins;

            float projectedStance = aiCharacter.aiCharacterNetworkManager.currentStance.Value - stanceDamage;

            if (aiCharacter.IsOwner)
                aiCharacter.aiCharacterNetworkManager.currentStance.Value -= stanceDamage;


            if (projectedStance <= 0)
            {
                //Optional: if in a very high intensity damage animation(like launched in air), do not play stance break animation
                DamageIntensity previousDamageIntensity = WorldUtilityManager.Singleton.GetDamageIntensityBasedOnPoiseDamage(previousPoiseDamageTaken);

                if (previousDamageIntensity == DamageIntensity.Colossal)
                {
                    projectedStance = 1;
                    aiCharacter.aiCharacterNetworkManager.currentStance.Value = projectedStance;
                    return;
                }

                aiCharacter.aiCharacterNetworkManager.currentStance.Value = maxStance;


                projectedStance = maxStance;

                if (aiCharacter.IsOwner)
                    aiCharacter.aiCharacterNetworkManager.currentStance.Value = projectedStance;

                if (ignoreStanceBreak)
                    return;

                if (aiCharacter.isDead.Value || aiCharacter.isDeadLocal)
                    return;

                aiCharacter.characterAnimatorManager.PlayTargetLocalAnimationInstantly("Stance_Break_01", true);
            }
        }

        public virtual void AlertCharacterToSound(Vector3 positionOfSound)
        {
            if (!aiCharacter.IsOwner)
                return;

            if (aiCharacter.isDead.Value)
                return;

            if (aiCharacter.idle == null)
                return;

            if (aiCharacter.investigateSound == null)
                return;

            if (!aiCharacter.idle.willInvestigateSound)
                return;

            if (aiCharacter.idle.idleStateMode == IdleStateMode.Sleep && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value.ToString(), true);
            }

            aiCharacter.investigateSound.positionOfSound = positionOfSound;
            aiCharacter.currentState = aiCharacter.currentState.ManuallySwitchState(aiCharacter, aiCharacter.investigateSound);
        }

        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Singleton.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;

                if (targetCharacter == aiCharacter)
                    continue;

                if (targetCharacter.isDead.Value)
                    continue;

                if (targetCharacter.characterNetworkManager.isSneaking.Value && targetCharacter.characterNetworkManager.isHidden.Value)
                {
                    if (targetCharacter.characterCombatManager.stealthObjectsCurrentlyStandingIn.Count > 0)
                        continue;

                    float distanceFromPotentialTarget = Vector3.Distance(aiCharacter.transform.position, targetCharacter.transform.position);
                    float maxDistance = detectionRadius * WorldUtilityManager.Singleton.hiddenTargetDetectionRadiusPenalty;

                    //optionally, make them investigate instead of ignore
                    AlertCharacterToSound(targetCharacter.transform.position);

                    if (distanceFromPotentialTarget > maxDistance)
                        continue;
                }

                if (WorldUtilityManager.Singleton.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.Singleton.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.Singleton.GetAngleOfTarget(transform, targetsDirection);

                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                            if (enablePivot)
                                PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
            }
        }

        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= 1 - 46 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }
        }

        public virtual void PivotTowardsPosition(AICharacterManager aiCharacter, Vector3 position)
        {
            if (aiCharacter.isPerformingAction)
                return;

            Vector3 targetsDirection = position = aiCharacter.transform.position;
            float viewableAngle = WorldUtilityManager.Singleton.GetAngleOfTarget(aiCharacter.transform, targetsDirection);

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_45", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_45", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            else if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_135", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_135", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= 1 - 46 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }
        }

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

        public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
        {
            if (currentTarget == null)
                return;

            if (!aiCharacter.aiCharacterLocomotionManager.canRotate)
                return;

            if (!aiCharacter.isPerformingAction)
                return;

            Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection == Vector3.zero)
                targetDirection = aiCharacter.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime); ;

        }

        public void HandleActionRecovery(AICharacterManager aiCharacter)
        {
            if (actionRecoveryTimer > 0)
            {
                if (!aiCharacter.isPerformingAction)
                {
                    actionRecoveryTimer -= Time.deltaTime;
                }
            }
        }

        public override void EnableCanDoCombo()
        {
            canPerformCombo = true;
        }

        public override void DisableCanDoCombo()
        {
            canPerformCombo = false;
            hasHitTargetDuringCombo = false;
        }

        public virtual void PerformEvasion()
        {
            if (currentTarget == null)
                return;

            if (distanceFromTarget > 5)
                return;

            //	METHOD 1: simply plays animation
            //aiCharacter.aiCharacterNetworkManager.isInvulnerable.Value = true;
            //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Evade_01", true);

            //	METHOD 2: rolls away from target
            //aiCharacter.aiCharacterNetworkManager.isInvulnerable.Value = true;
            //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Evade_01", true);
            //Vector3 directionToDodge = -aiCharacter.transform.forward;
            //directionToDodge.y = 0;
            //directionToDodge.Normalize();
            //	optional to coroutine to smooth rotation
            //aiCharacter.transform.rotation = Quaternion.LookRotation(directionToDodge);

            //	METHOD 3: rolls random direction
            aiCharacter.aiCharacterNetworkManager.isInvulnerable.Value = true;
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Evade_01", true);
            Vector3 directionToDodge = Random.insideUnitSphere.normalized;
            directionToDodge.y = 0;
            //	optional to coroutine to smooth rotation
            aiCharacter.transform.rotation = Quaternion.LookRotation(directionToDodge);

            //	METHOD 4: use blend tree
            //1 select values and update network vert and horz
            //2 play animation


        }

        //Ranged Combat
        public virtual void DrawProjectile()
        {

        }
    }
}