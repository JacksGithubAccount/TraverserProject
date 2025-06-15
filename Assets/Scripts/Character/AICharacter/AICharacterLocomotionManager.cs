using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }
}
