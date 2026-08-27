using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {

        [Header("Attack")]
        [SerializeField] private string attackAnimation;
        [SerializeField] bool isParryable = true;

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction;

        [Header("Action Values")]
        [SerializeField] AttackType attackType;
        public int attackWeight = 50;

        public float actionRecoveryTime = 1.5f;

        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;
        public bool requireClearLineOfSight = false;

        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            //if AI act like player(like invader) use this
            //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackType, attackAnimation, true);

            //if AI use simple attacks that are purely animation based (not equipment/item based) use this
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);
            aiCharacter.aiCharacterNetworkManager.isParryable.Value = isParryable;
        }

    }
}