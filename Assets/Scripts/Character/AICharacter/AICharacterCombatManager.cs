using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{

    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Target Information")]
        public float viewableAngle;
        public Vector3 targetsDirection;

        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        public float minimumFOV = -35;
        public float maximumFOV = 35;


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

                if (WorldUtilityManager.Singleton.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if (angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.Singleton.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.Log("Blocked");
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.Singleton.GetAngleOfTarget(transform, targetsDirection);

                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
            }
        }

        public void PivotTowardsTarget(AICharacterManager aiCharacter)
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
    }
}