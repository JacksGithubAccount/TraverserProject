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
        [SerializeField] Transform maxTopExitHeightTransform; //max height you are allowed when exiting a ladder at the top(blends player to proper ground when exiting)
        private Vector3 refVelocity = Vector3.zero;
        [SerializeField] float smoothTime = 0.2f;

        private Coroutine enterLadderCoroutine;
        private Coroutine exitLadderCoroutine;
        private Coroutine delayPopUpCoroutine;
        private PlayerManager playerClimbingLadder = null;

        [Header("Animation")]
        [SerializeField] string climbDownLadderEnterAnimation = "Climb_Down_Ladder_Enter_01";
        [SerializeField] string climbUpLadderEnterAnimation = "Climb_Up_Ladder_Enter_01";
        [SerializeField] string climbUpLadderExitRightAnimation = "Climb_Up_Ladder_Exit_R_01";
        [SerializeField] string climbUpLadderExitLeftAnimation = "Climb_Up_Ladder_Exit_L_01";

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            playerClimbingLadder = player;

            if (!player.playerNetworkManager.isClimbingLadder.Value)
                player.playerInteractionManager.AddInteractionToList(this);

            if (player.playerNetworkManager.isClimbingLadder.Value)
            {
                //wait for stop climbing ladder to send interaction pop up
                if (delayPopUpCoroutine != null)
                    StopCoroutine(delayPopUpCoroutine);

                delayPopUpCoroutine = StartCoroutine(WaitForPlayerToConcludeClimbingBeforeSendingPopUp());
            }

            if (!player.playerNetworkManager.isClimbingLadder.Value)
                return;

            if (exitLadderCoroutine != null)
                StopCoroutine(exitLadderCoroutine);

            exitLadderCoroutine = StartCoroutine(CheckForExitCoroutine(player));
        }

        public override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            playerClimbingLadder = null;

            player.playerInteractionManager.RemoveInteractionFromList(this);

            if (exitLadderCoroutine != null)
                StopCoroutine(exitLadderCoroutine);
        }

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



        //Exiting ladder logic

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


                StartCoroutine(LimitExitHeightCoroutine(player));
                //force player to minimum height
                StartCoroutine(ForcePlayerToMinimumExitHeightCoroutine(player));

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

        private IEnumerator CheckForExitCoroutine(PlayerManager player)
        {
            while (player.playerNetworkManager.isClimbingLadder.Value)
            {
                CheckForExit(player);
                yield return null;
            }
            yield return null;
        }

        //Sync/lock player to proper exit height(failsafe for if animation root motion moves the player too high or low)
        private IEnumerator LimitExitHeightCoroutine(PlayerManager player)
        {
            while (player.playerLocomotionManager.isExitingLadder)
            {
                if (player.transform.position.y > maxTopExitHeightTransform.position.y)
                    player.transform.position = new Vector3(player.transform.position.x, maxTopExitHeightTransform.position.y, player.transform.position.z);

                yield return null;
            }
        }

        private IEnumerator ForcePlayerToMinimumExitHeightCoroutine(PlayerManager player)
        {
            //wait length of exit animation to get to its peak height
            yield return new WaitForSeconds(1);

            while (player.playerLocomotionManager.isExitingLadder)
            {
                if (player.transform.position.y < maxTopExitHeightTransform.position.y)
                    player.transform.position = new Vector3(player.transform.position.x, maxTopExitHeightTransform.position.y, player.transform.position.z);

                yield return null;
            }
        }

        //Do not send "Climb pop up" whilst already climbing
        private IEnumerator WaitForPlayerToConcludeClimbingBeforeSendingPopUp()
        {
            while (playerClimbingLadder != null && playerClimbingLadder.playerNetworkManager.isClimbingLadder.Value)
            {
                yield return new WaitForEndOfFrame();
            }

            if (playerClimbingLadder != null)
                playerClimbingLadder.playerInteractionManager.AddInteractionToList(this);
        }
    }
}