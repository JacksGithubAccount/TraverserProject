using UnityEngine;

namespace TraverserProject
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }
        private void GetVerticalAndHorizontalInputs()
        {
            verticalMovement = PlayerInputManager.Singleton.verticalInput;
            horizontalMovement = PlayerInputManager.Singleton.horizontalInput;

            //clamp the movements
        }
        private void HandleGroundedMovement()
        {
            GetVerticalAndHorizontalInputs();
            //movement is based in camer direction and move inputs
            moveDirection = PlayerCamera.Singleton.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.Singleton.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

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
        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Singleton.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.Singleton.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero){
                targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}
