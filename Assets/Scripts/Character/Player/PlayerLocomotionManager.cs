using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        [Header("Player Movements")]
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float runningBackwardSpeed = 4;
        [SerializeField] float sprintingSpeed = 7;
        [SerializeField] float sneakingWalkSpeed = 1.1f;
        [SerializeField] float sneakingRunSpeed = 3f;
        [SerializeField] float sneakingRunBackwardSpeed = 2.8f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 25;
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDirection;

        [Header("Ladder")]
        [SerializeField] Vector3 ladderDirection;
        [SerializeField] float ladderClimbSpeed = 2;

        [Header("Dodge")]
        private Vector3 RollDirection;
        [SerializeField] float dodgeStaminaCost = 25;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Update()
        {
            base.Update();
            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
                {

                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
                }
                else
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalMovement, verticalMovement, player.playerNetworkManager.isSprinting.Value);
                }
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Singleton.verticalInput;
            horizontalMovement = PlayerInputManager.Singleton.horizontalInput;
            moveAmount = PlayerInputManager.Singleton.moveAmount;
            //clamp the movements
        }
        private void HandleGroundedMovement()
        {
            if (player.characterLocomotionManager.canMove || player.playerLocomotionManager.canRotate)
            {
                GetMovementValues();
            }

            if (!player.characterLocomotionManager.canMove)
                return;

            //movement is based in camer direction and move inputs
            if (player.playerNetworkManager.isAiming.Value)
            {
                moveDirection = transform.forward * verticalMovement;
                moveDirection = moveDirection + transform.right * horizontalMovement;
                moveDirection.Normalize();
                moveDirection.y = 0;
            }
            else
            {
                moveDirection = PlayerCamera.Singleton.transform.forward * verticalMovement;
                moveDirection = moveDirection + PlayerCamera.Singleton.transform.right * horizontalMovement;
                moveDirection.Normalize();
                moveDirection.y = 0;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                MoveAtSprintingSpeed();
                return;
            }

            if (player.playerNetworkManager.isSneaking.Value)
            {
                MoveAtSneakingSpeed();
                return;
            }

            MoveAtRegularSpeed();

        }

        private void HandleJumpingMovement()
        {
            if (player.playerNetworkManager.isJumping.Value)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleLadderMovementUp()
        {
            if (player.playerLocomotionManager.isOnLadder)
            {
                if (currentLadderClimbPosition == interactedLadderClimbPositions.Length - 1)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Climb_To_Top_01", true);
                    player.playerNetworkManager.isExitingLadder.Value = true;
                    return;
                }

                currentLadderClimbPosition++;

                if (currentLadderClimbPosition < 0)
                    currentLadderClimbPosition = 0;

                if (currentLadderClimbPosition >= interactedLadderClimbPositions.Length)
                    currentLadderClimbPosition = interactedLadderClimbPositions.Length - 1;

                player.transform.position = interactedLadderClimbPositions[currentLadderClimbPosition].position;
            }
        }
        private void HandleLadderMovementDown()
        {
            if (player.playerLocomotionManager.isOnLadder)
            {
                if (currentLadderClimbPosition == 0)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Start_Climbing_To_Bottom_01", true);
                    player.playerNetworkManager.isExitingLadder.Value = true;
                    return;
                }

                currentLadderClimbPosition--;

                if (currentLadderClimbPosition < 0)
                    currentLadderClimbPosition = 0;

                if (currentLadderClimbPosition >= interactedLadderClimbPositions.Length)
                    currentLadderClimbPosition = interactedLadderClimbPositions.Length - 1;

                player.transform.position = interactedLadderClimbPositions[currentLadderClimbPosition].position;
            }
        }
        private void DisableIsOnLadder()
        {
            player.playerLocomotionManager.isOnLadder = false;
        }

        private void HandleFreeFallMovement()
        {
            if (player.playerLocomotionManager.isOnLadder)
                return;

            if (!player.characterLocomotionManager.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.Singleton.transform.forward * PlayerInputManager.Singleton.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.Singleton.transform.right * PlayerInputManager.Singleton.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
        private void HandleRotation()
        {
            if (player.isDead.Value)
                return;

            if (!player.characterLocomotionManager.canRotate)
                return;

            if (player.playerNetworkManager.isAiming.Value && !player.playerNetworkManager.isHoldingArrow.Value)
            {
                HandleAimRotation();
            }
            else if (player.playerNetworkManager.isAiming.Value && player.playerNetworkManager.isHoldingArrow.Value)
            {
                HandleAimAndHoldingRotation();
            }
            else
            {
                HandleStandardRotation();
            }


        }

        private void HandleAimRotation()
        {
            Vector3 targetDirection;
            targetDirection = PlayerCamera.Singleton.cameraObject.transform.forward;
            targetDirection.y = 0;
            targetDirection.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = finalRotation;
        }

        private void HandleAimAndHoldingRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Singleton.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.Singleton.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        private void HandleStandardRotation()
        {

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerNetworkManager.isSprinting.Value || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.Singleton.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.Singleton.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                        targetDirection = transform.forward;

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;

                }
                else
                {
                    if (player.playerCombatManager.currentTarget == null)
                        return;

                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                targetRotationDirection = Vector3.zero;
                targetRotationDirection = PlayerCamera.Singleton.cameraObject.transform.forward * verticalMovement;
                targetRotationDirection = targetRotationDirection + PlayerCamera.Singleton.cameraObject.transform.right * horizontalMovement;
                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                if (targetRotationDirection == Vector3.zero)
                {
                    targetRotationDirection = transform.forward;
                }
                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }
        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
                player.playerNetworkManager.isSneaking.Value = false;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }
        public void AttemptToPerformDodge()
        {
            if (!player.playerLocomotionManager.canRoll)
                return;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (player.IsOwner)
                player.playerNetworkManager.isRolling.Value = true;

            if (PlayerInputManager.Singleton.moveAmount > 0) //roll
            {
                RollDirection = PlayerCamera.Singleton.cameraObject.transform.forward * PlayerInputManager.Singleton.verticalInput;
                RollDirection += PlayerCamera.Singleton.cameraObject.transform.right * PlayerInputManager.Singleton.horizontalInput;

                RollDirection.y = 0;
                RollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(RollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);
                player.playerLocomotionManager.isRolling = true;
            }
            else //backstep
            {
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }
            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
            player.playerNetworkManager.DestroyAllCurrentActionFXServerRpc();
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;


            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;

            if (player.playerNetworkManager.isJumping.Value)
                return;

            if (!player.characterLocomotionManager.isGrounded)
                return;

            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);
            player.playerNetworkManager.isJumping.Value = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.Singleton.cameraObject.transform.forward * PlayerInputManager.Singleton.verticalInput;
            jumpDirection += PlayerCamera.Singleton.cameraObject.transform.right * PlayerInputManager.Singleton.horizontalInput;

            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                else if (PlayerInputManager.Singleton.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                else if (PlayerInputManager.Singleton.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }

        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

        private void MoveAtRegularSpeed()
        {
            if (!player.characterController.enabled)
                return;

            //if you want to make it so that running backwards whilst locked on is at a different speed vs running forward
            if (player.playerNetworkManager.isLockedOn.Value && verticalMovement < -0.5f)
            {
                player.characterController.Move(moveDirection * runningBackwardSpeed * Time.deltaTime);
                return;
            }

            if (PlayerInputManager.Singleton.moveAmount > 0.5f)
            {
                //running speed
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Singleton.moveAmount <= 0.5f)
            {
                //walk speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }

        }

        private void MoveAtSprintingSpeed()
        {
            if (!player.characterController.enabled)
                return;

            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }

        private void MoveAtSneakingSpeed()
        {
            if (!player.characterController.enabled)
                return;

            //if you want to make it so that running backwards whilst locked on is at a different speed vs running forward
            if (player.playerNetworkManager.isLockedOn.Value && verticalMovement < -0.5f)
            {
                player.characterController.Move(moveDirection * sneakingRunBackwardSpeed * Time.deltaTime);
                return;
            }

            if (PlayerInputManager.Singleton.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * sneakingRunSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Singleton.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * sneakingWalkSpeed * Time.deltaTime);
            }
        }
    }
}