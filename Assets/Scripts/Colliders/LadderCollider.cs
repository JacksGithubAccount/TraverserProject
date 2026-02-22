using UnityEngine;

namespace TraverserProject
{
    public class LadderCollider : MonoBehaviour
    {
        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponentInParent<CharacterManager>();

            if (character == null)
                return;

            if(character.characterLocomotionManager.isOnLadder)
            {
                character.characterNetworkManager.isExitingLadder.Value = true;
                
            }

        }

        protected virtual void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponentInParent<CharacterManager>();

            if (character == null)
                return;

            character.characterNetworkManager.isExitingLadder.Value = false;
        }
    }
}
