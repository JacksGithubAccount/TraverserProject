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
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;

        [Header("Bumper Input")]
        [SerializeField] bool RB_Input = false;

        [Header("Trigger Input")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool Hold_RT_Input = false;




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


                //actions
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;

                //bumpers
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;


                //Triggers
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

                playerControls.PlayerActions.LockOn.performed += i => lock_On_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

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
            HandlePlayerMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleRBInput();
            HandleRTInput();
            HandleHoldRTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
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

        private void HandlePlayerMovementInput()
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

            if (moveAmount > 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

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

        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleHoldRTInput()
        {
            if (player.isPerformingAction)
            {
                if (player.playerNetworkManager.isUsingRightHand.Value)
                {
                    player.playerNetworkManager.isChargingAttack.Value = Hold_RT_Input;
                }
            }
        }

        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }
    }
}