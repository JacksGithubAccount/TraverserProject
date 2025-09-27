using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        AICharacterManager aiCharacter;



        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }

        protected override void Update()
        {
            base.Update();

            if (aiCharacter.IsOwner)
            {
                aiCharacter.characterNetworkManager.verticalMovement.Value = aiCharacter.animator.GetFloat("Vertical");
                aiCharacter.characterNetworkManager.horizontalMovement.Value = aiCharacter.animator.GetFloat("Horizontal"); ;
            }
            else
            {
                aiCharacter.animator.SetFloat("Vertical", aiCharacter.characterNetworkManager.verticalMovement.Value, 0.1f, Time.deltaTime);
                aiCharacter.animator.SetFloat("Horizontal", aiCharacter.characterNetworkManager.horizontalMovement.Value, 0.1f, Time.deltaTime);


            }
        }
    }
}