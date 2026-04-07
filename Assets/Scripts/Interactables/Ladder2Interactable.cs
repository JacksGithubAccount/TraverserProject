using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class Ladder2Interactable : Interactable
    {
        [Header("Ladder Position")]
        [SerializeField] bool isTopOfLadder = true;
        [SerializeField] Transform ladderCenterPosition;

        [Header("Start Position")]
        [SerializeField] Transform startPosition;
        [SerializeField] Transform topExitStartPositionRightIdlePosition; //exit position if right hand is higher
        [SerializeField] Transform topExitStartPositionLeftIdlePosition;  //exit position if left hand is higher

        [Header("Exiting Top")]
        [SerializeField] Transform maxHeightTransform; //max height you are allowed when exiting a ladder at the top(blends player to proper ground when exiting)
        private Vector3 refVelocity = Vector3.zero;
        [SerializeField] float smoothTime = 0.2f;

        private Coroutine enterLadderCoroutine;
        private Coroutine exitLadderCoroutine;
        private PlayerManager playerClimbingLadder = null;

        [Header("Animation")]
        [SerializeField] string climbDownLadderEnterAnimation = "Climb_Down_Ladder_Enter_01";
        [SerializeField] string climbUpLadderEnterAnimation = "Climb_Up_Ladder_Enter_01";
        [SerializeField] string climbUpLadderExitRightAnimation = "Climb_Up_Ladder_Exit_R_01";
        [SerializeField] string climbUpLadderExitLeftAnimation = "Climb_Up_Ladder_Exit_L_01";

        public override void Interact(PlayerManager player)
        {
            if (player.playerNetworkManager.isClimbingLadder.Value)
                return;

            if (enterLadderCoroutine != null)
                StopCoroutine(enterLadderCoroutine);

            enterLadderCoroutine = StartCoroutine(WaitThenEnterLadder(player));
        }

        private IEnumerator WaitThenEnterLadder(PlayerManager player)
        {
            //disables any possibility of movement via character controller/animation root motion
            player.playerInteractionManager.RemoveInteractionFromList(this);
            player.playerLocomotionManager.DisableCanMove();
            player.playerLocomotionManager.DisableCanRotate();
            player.playerLocomotionManager.DisableCanRoll();
            player.isPerformingAction = true;
            player.characterController.enabled = false;

            //makes character face the ladder before climbing & hides their weapons
            Vector3 rotationDirection = startPosition.transform.forward;
            rotationDirection.y = 0;
            player.transform.position = startPosition.transform.position;
            player.transform.rotation = Quaternion.LookRotation(rotationDirection);
            player.playerNetworkManager.HideWeaponsServerRpc();

            if (isTopOfLadder)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(climbDownLadderEnterAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(climbUpLadderEnterAnimation, true);
            }

            while (!player.playerNetworkManager.isClimbingLadder.Value)
            {
                yield return null;

                //keeps the player centered on the ladder while maintaining their height while climbing
                if (!player.playerLocomotionManager.isExitingLadder && !isTopOfLadder)
                {
                    Vector3 newPosition = new Vector3(ladderCenterPosition.transform.position.x, player.transform.position.y, ladderCenterPosition.transform.position.z);
                    player.transform.position = Vector3.SmoothDamp(player.transform.position, newPosition, ref refVelocity, smoothTime * Time.deltaTime);
                }
            }

            while (player.playerNetworkManager.isClimbingLadder.Value)
            {
                player.characterController.enabled = true;

                if (!player.playerLocomotionManager.canExitTopOfLadder)
                    player.playerLocomotionManager.EnableCanMove();

                //if player is at exit height and they are attempting to go up further, disables movement and exist via animation movement
                if (player.transform.position.y >= topExitStartPositionLeftIdlePosition.position.y && PlayerInputManager.Singleton.verticalInput > 0)
                    player.playerLocomotionManager.DisableCanMove();

                if (player.transform.position.y >= topExitStartPositionRightIdlePosition.position.y && PlayerInputManager.Singleton.verticalInput > 0)
                    player.playerLocomotionManager.DisableCanMove();

                if (!player.playerLocomotionManager.isExitingLadder)
                {
                    Vector3 newPosition = new Vector3(ladderCenterPosition.transform.position.x, player.transform.position.y, ladderCenterPosition.transform.position.z);
                    player.transform.position = Vector3.SmoothDamp(player.transform.position, newPosition, ref refVelocity, smoothTime * Time.deltaTime);
                }

                player.transform.rotation = Quaternion.LookRotation(rotationDirection);
                yield return null;
            }
        }

        private void CheckForExit(PlayerManager player)
        {
            if (!player.playerNetworkManager.isClimbingLadder.Value)
                return;

            if (player.playerLocomotionManager.isExitingLadder)
                return;

            //exit top
            if (isTopOfLadder)
            {
                if (PlayerInputManager.Singleton.verticalInput <= 0)
                    return;

                //if the player can exit the ladder with their respective raised hand, but is not yet at the exit height, return	
                if (player.playerLocomotionManager.canExitLadderWithRightHand && player.transform.position.y < topExitStartPositionRightIdlePosition.position.y)
                    return;

                if (player.playerLocomotionManager.canExitLadderWithLeftHand && player.transform.position.y < topExitStartPositionLeftIdlePosition.position.y)
                    return;

                if (!player.playerLocomotionManager.canExitTopOfLadder)
                    return;

                player.playerLocomotionManager.isExitingLadder = true;
                player.playerLocomotionManager.DisableCanMove();
                player.playerLocomotionManager.DisableCanRotate();
                player.isPerformingAction = true;
                player.characterController.enabled = false;

                if (player.playerLocomotionManager.canExitLadderWithRightHand)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation(climbUpLadderExitRightAnimation, true);
                }
                else if (player.playerLocomotionManager.canExitLadderWithLeftHand)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation(climbUpLadderExitLeftAnimation, true);
                }

                Vector3 rotationDirection = startPosition.transform.forward;
                player.transform.rotation = Quaternion.LookRotation(rotationDirection);
                return;
            }

            //exit bottom
            if (PlayerInputManager.Singleton.verticalInput < 0 || player.playerNetworkManager.isSlidingDownLadder.Value)
            {
                player.playerLocomotionManager.isExitingLadder = true;
                player.playerLocomotionManager.DisableCanMove();
                player.playerLocomotionManager.DisableCanRotate();

                if (player.playerNetworkManager.isSlidingDownLadder.Value)
                    player.playerAnimatorManager.PlayTargetActionAnimation("Ladder Empty", true);

                if (player.playerLocomotionManager.canExitLadderWithRightHand)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimationInstantly(climbUpLadderExitRightAnimation, true);
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetActionAnimationInstantly(climbUpLadderExitLeftAnimation, true);
                }
            }

        }
    }
}