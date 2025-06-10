using TraverserProject;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TraverserProject
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Singleton;
        PlayerControls playerControls;
        public PlayerManager player;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Lock On Input")]
        [SerializeField] bool lock_On_Input;
        [SerializeField] bool lockOn_Left_Input;
        [SerializeField] bool lockOn_Right_Input;
        [SerializeField] Vector2 lockOn_Seek_Input;
        private Coroutine lockOnCoroutine;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool RB_Input = false;




        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;

            Singleton.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //enables controls if loading into our world scene
            if (newScene.buildIndex == WorldSaveGameManager.Singleton.GetWorldSceneIndex())
            {
                Singleton.enabled = true;
                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                Singleton.enabled = false;
                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                playerControls.PlayerActions.LockOn.performed += i => lock_On_Input = true;
                //playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                //playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;
                playerControls.PlayerActions.SeekLockOnTarget.performed += i => lockOn_Seek_Input = i.ReadValue<Vector2>();

                //hold input sprints, release stops sprint
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }
            playerControls.Enable();

        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void HandleAllInputs()
        {
            HandleLockOnInput();
            HandleLockOnSwitchInput();
            HandleCameraMovementInput();
            HandleMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
        }

        private void HandleLockOnInput()
        {
            if(player.playerNetworkManager.isLockedOn.Value)
			{
                if (player.playerCombatManager.currentTarget == null)
                    return;

                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    player.playerNetworkManager.isLockedOn.Value = false;
                }

                if (lockOnCoroutine != null)
                    StopCoroutine(lockOnCoroutine);

                lockOnCoroutine = StartCoroutine(PlayerCamera.Singleton.WaitThenFindNewTarget());


            }
            if (lock_On_Input && player.playerNetworkManager.isLockedOn.Value)
            {
                lock_On_Input = false;
                PlayerCamera.Singleton.ClearLockOnTargets();
                player.playerNetworkManager.isLockedOn.Value = false;

                return;
            }

            if (lock_On_Input && !player.playerNetworkManager.isLockedOn.Value)
            {
                lock_On_Input = false;

                PlayerCamera.Singleton.HandleLocatingLockOnTargets();


                if (PlayerCamera.Singleton.nearestLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Singleton.nearestLockOnTarget);
                    player.playerNetworkManager.isLockedOn.Value = true;
                }
            }
        }

        private void HandleLockOnSwitchInput()
        {
            if (lockOn_Seek_Input.x > cameraInput.x)
                lockOn_Right_Input = true;
            else if(lockOn_Seek_Input.x < cameraInput.x)
                lockOn_Left_Input = true;

            if (lockOn_Left_Input)
            {
                lockOn_Left_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.Singleton.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Singleton.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.Singleton.leftLockOnTarget);
                    }
                }
            }

            if (lockOn_Right_Input)
            {
                lockOn_Right_Input = false;

                if (player.playerNetworkManager.isLockedOn.Value)
                {
                    PlayerCamera.Singleton.HandleLocatingLockOnTargets();

                    if (PlayerCamera.Singleton.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.Singleton.rightLockOnTarget);
                    }
                }
            }
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            //clamps movement values
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
            if (player == null)
                return;

            if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
            {

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
            }
        }
        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;

        }
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                player.playerLocomotionManager.AttemptToPerformDodge();
            }

        }
        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }
    }
}