using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "AI/States/Boss Sleep")]
    public class BossSleepState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return base.Tick(aiCharacter);
        }

    }
}