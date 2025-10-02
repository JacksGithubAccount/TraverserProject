using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Boss Sleep")]
    public class BossSleepState : AIState
    {
        [Header("Sleep Options")]
        private bool sleepAnimationSet = false;
        [SerializeField] string sleepingAnimation = "Sleep_01";
        [SerializeField] string wakingAnimation = "Wake_01";

        public bool hasBeenAwakened = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            aiCharacter.navMeshAgent.enabled = false;
            if (!hasBeenAwakened)
            {
                return HasNotBeenAwakened(aiCharacter);
            }
            else
            {
                return HasBeenAwakened(aiCharacter);
            }
        }

        private AIState HasBeenAwakened(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {

                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            return this;
        }

        private AIState HasNotBeenAwakened(AICharacterManager aiCharacter)
        {
            aiCharacter.navMeshAgent.enabled = false;

            if (aiCharacter.characterCombatManager.currentTarget != null)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            if (!sleepAnimationSet && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {
                sleepAnimationSet = true;
                aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value = sleepingAnimation;
                aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value = wakingAnimation;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterNetworkManager.sleepingAnimation.Value.ToString(), true);
            }

            if (aiCharacter.characterCombatManager.currentTarget != null && !aiCharacter.aiCharacterNetworkManager.isAwake.Value)
            {

                aiCharacter.aiCharacterNetworkManager.isAwake.Value = true;

                if (!aiCharacter.isPerformingAction && !aiCharacter.isDead.Value)
                    aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(aiCharacter.aiCharacterNetworkManager.wakingAnimation.Value.ToString(), true);

                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            return this;
        }

    }
}