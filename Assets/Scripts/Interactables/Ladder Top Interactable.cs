using UnityEngine;

namespace TraverserProject
{
    public class LadderTopInteractable : Interactable
    {
        [Header("Transforms")]
        public Transform playerStandingPositionAtTop;
        public Transform[] playerClimbingPositions;
        public override void Interact(PlayerManager player)
        {
            //interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            //turns player to face interactable object
            Vector3 rotationDirection = transform.position - player.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
            player.transform.rotation = targetRotation;

            player.transform.position = playerStandingPositionAtTop.transform.position;
            player.playerLocomotionManager.interactedLadderClimbPositions = playerClimbingPositions;
            player.playerLocomotionManager.currentLadderClimbPosition = playerClimbingPositions.Length - 1;
            player.playerLocomotionManager.isOnLadder = true;
            player.playerLocomotionManager.isGrounded = false;
            player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Climb_From_Top_01", true);
        }
    }
}
