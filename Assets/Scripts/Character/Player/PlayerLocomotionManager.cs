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
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
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
            Vector3 targetDirection = Vector3.zero;
            targetDirection = PlayerCamera.Singleton.cameraObject.transform.forward * verticalMovement;
            targetDirection = targetDirection + PlayerCamera.Singleton.cameraObject.transform.right * horizontalMovement;
        }
    }
}
