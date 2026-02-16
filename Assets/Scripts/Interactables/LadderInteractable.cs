using UnityEngine;

namespace TraverserProject
{
    public class LadderInteractable : Interactable
    {
        public Transform playerStandingPosition;
        public override void Interact(PlayerManager player)
        {
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
            
            //turns player to face interactable object
            Vector3 rotationDirection = transform.position - player.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
            player.transform.rotation = targetRotation;

            player.transform.position = playerStandingPosition.transform.position;
            player.playerLocomotionManager.isOnLadder = true;
            player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Start_Climbing_From_Bottom_01", true); 
        }
    }
}
