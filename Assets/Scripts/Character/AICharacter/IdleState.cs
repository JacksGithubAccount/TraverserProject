using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.characterCombatManager.currentTarget != null)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }
            else
            {

                aiCharacter.aiCharacterCombatManager.FindATargetViaLineOfSight(aiCharacter);


                return this;
            }

        }



    }
}