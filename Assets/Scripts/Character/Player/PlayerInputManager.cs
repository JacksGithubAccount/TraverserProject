using TraverserProject;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

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
        [SerializeField] bool sneakInput = false;
        [SerializeField] bool switch_Right_Weapon_Input = false;
        [SerializeField] bool switch_Left_Weapon_Input = false;
        [SerializeField] bool switch_Quick_Slot_Spell_Input = false;
        [SerializeField] bool switch_Quick_Slot_Item_Input = false;
        [SerializeField] bool interaction_Input = false;
        [SerializeField] bool use_Item_Input = false;

        [Header("Bumper Input")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool hold_RB_Input = false;
        [SerializeField] bool LB_Input = false;
        [SerializeField] bool hold_LB_Input = false;

        [Header("Trigger Input")]
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool hold_RT_Input = false;
        [SerializeField] bool LT_Input = false;

        [Header("Two Hand Input")]
        [SerializeField] bool two_Hand_Input = false;
        [SerializeField] bool two_Hand_Right_Weapon_Input = false;
        [SerializeField] bool two_Hand_Left_Weapon_Input = false;

        [Header("Que Inputs")]
        private bool input_Que_Is_Active = false;
        [SerializeField] float default_Que_Input_Time = 0.35f;
        [SerializeField] float que_Input_Timer = 0;
        [SerializeField] bool que_RB_Input = false;
        [SerializeField] bool que_RT_Input = false;

        [Header("UI Inputs")]
        [SerializeField] bool openCharacterMenuInput = false;
        [SerializeField] bool closeMenuInput = false;


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
                playerControls.PlayerActions.Sneak.performed += i => sneakInput = true;
                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switch_Right_Weapon_Input = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switch_Left_Weapon_Input = true;
                playerControls.PlayerActions.SwitchQuickSlotItem.performed += i => switch_Quick_Slot_Item_Input = true;
                playerControls.PlayerActions.SwitchQuickSlotSpell.performed += i => switch_Quick_Slot_Spell_Input = true;
                playerControls.PlayerActions.Interact.performed += i => interaction_Input = true;
                playerControls.PlayerActions.X.performed += i => use_Item_Input = true;

                //bumpers
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;
                playerControls.PlayerActions.HoldRB.performed += i => hold_RB_Input = true;
                playerControls.PlayerActions.HoldRB.canceled += i => hold_RB_Input = false;
                playerControls.PlayerActions.LB.performed += i => LB_Input = true;
                playerControls.PlayerActions.HoldLB.performed += i => hold_LB_Input = true;
                playerControls.PlayerActions.HoldLB.canceled += i => hold_LB_Input = false;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isBlocking.Value = false;
                playerControls.PlayerActions.LB.canceled += i => player.playerNetworkManager.isAiming.Value = false;

                //Triggers
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;
                playerControls.PlayerActions.HoldRT.performed += i => hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => hold_RT_Input = false;
                playerControls.PlayerActions.LT.performed += i => LT_Input = true;

                //two hand
                playerControls.PlayerActions.TwoHandWeapon.performed += i => two_Hand_Input = true;
                playerControls.PlayerActions.TwoHandWeapon.canceled += i => two_Hand_Input = false;
                playerControls.PlayerActions.TwoHandRightWeapon.performed += i => two_Hand_Right_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandRightWeapon.canceled += i => two_Hand_Right_Weapon_Input = false;
                playerControls.PlayerActions.TwoHandLeftWeapon.performed += i => two_Hand_Left_Weapon_Input = true;
                playerControls.PlayerActions.TwoHandLeftWeapon.canceled += i => two_Hand_Left_Weapon_Input = false;


                //lock on
                playerControls.PlayerActions.LockOn.performed += i => lock_On_Input = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOn_Left_Input = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOn_Right_Input = true;

                //hold input sprints, release stops sprint
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

                //Queued Inputs
                playerControls.PlayerActions.QueRB.performed += i => QueInputs(ref que_RB_Input);
                playerControls.PlayerActions.QueRT.performed += i => QueInputs(ref que_RT_Input);

                //UI Inputs
                playerControls.PlayerActions.Dodge.performed += i => closeMenuInput = true;
                playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;
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
            HandleUseItemInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleLockOnSwitchInput();
            HandleCameraMovementInput();
            HandlePlayerMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleSneakInput();
            HandleRBInput();
            HandleHoldRBInput();
            HandleLBInput();
            HandleHoldLBInput();
            HandleRTInput();
            HandleHoldRTInput();
            HandleLTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
            HandleSwitchQuickSlotSpellInput();
            HandleSwitchQuickSlotItemInput();
            HandleQueuedInputs();
            HandleInteractionInput();
            HandleOpenCharacterMenuInput();
            HandleCloseUIInput();

        }

        private void HandleUseItemInput()
        {
            if (use_Item_Input)
            {
                use_Item_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (player.playerInventoryManager.currentQuickSlotItem != null)
                {
                    player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);

                    player.playerNetworkManager.NotifyTheServerOfQuickSlotItemActionServerRpc(NetworkManager.Singleton.LocalClientId, player.playerInventoryManager.currentQuickSlotItem.itemID);
                }
            }
        }

        private void HandleTwoHandInput()
        {
            if (!two_Hand_Input)
                return;

            if (two_Hand_Right_Weapon_Input)
            {
                RB_Input = false;
                two_Hand_Right_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    player.playerNetworkManager.isTwoHandingRightWeapon.Value = true;
                    return;
                }
            }
            else if (two_Hand_Left_Weapon_Input)
            {
                LB_Input = false;
                two_Hand_Left_Weapon_Input = false;
                player.playerNetworkManager.isBlocking.Value = false;

                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    player.playerNetworkManager.isTwoHandingWeapon.Value = false;
                    return;
                }
                else
                {
                    player.playerNetworkManager.isTwoHandingLeftWeapon.Value = true;
                    return;
                }
            }
        }


        private void HandleLockOnInput()
        {
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;

                Vector3 targetLockOnTransform = PlayerCamera.Singleton.nearestLockOnTarget.characterCombatManager.lockOnTransform.transform.position;
                Vector2 lockOnCrosshairPosition = RectTransformUtility.WorldToScreenPoint(PlayerCamera.Singleton.cameraObject, targetLockOnTransform);
                PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.transform.position = lockOnCrosshairPosition;
                PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.SetActive(true);

                if (player.playerCombatManager.currentTarget.isDead.Value)
                {
                    PlayerCamera.Singleton.ClearLockOnTargets();
                    PlayerCamera.Singleton.HandleLocatingLockOnTargets();
                    if (PlayerCamera.Singleton.nearestLockOnTarget == null)
                    {
                        PlayerCamera.Singleton.ClearLockOnTargets();
                        player.playerCombatManager.SetTarget(null);
                        player.playerNetworkManager.isLockedOn.Value = false;
                    }
                    else
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.Singleton.nearestLockOnTarget);
                    }
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

            if (!player.playerLocomotionManager.canMove)
            {
                player.playerNetworkManager.isMoving.Value = false;
                return;
            }

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

            if (moveAmount != 0)
            {
                player.playerNetworkManager.isMoving.Value = true;
            }
            else
            {
                player.playerNetworkManager.isMoving.Value = false;
            }

            if (!player.playerLocomotionManager.canRun)
            {
                if (moveAmount > 0.5f)
                    moveAmount = 0.5f;

                if (verticalInput > 0.5f)
                    verticalInput = 0.5f;

                if (horizontalInput > 0.5f)
                    horizontalInput = 0.5f;
            }

            if (!player.characterController.enabled)
            {
                player.playerNetworkManager.isMoving.Value = false;
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);
                return;
            }

            if (player.playerNetworkManager.isClimbingLadder.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, false);
                return;
            }

            if (player.playerNetworkManager.isLockedOn.Value && !player.playerNetworkManager.isSprinting.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
                return;
            }

            if (player.playerNetworkManager.isAiming.Value)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
                return;
            }

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

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

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

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
                player.playerNetworkManager.isSlidingDownLadder.Value = false;
            }
        }
        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void HandleSneakInput()
        {
            if (sneakInput)
            {
                sneakInput = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                player.playerNetworkManager.isSneaking.Value = !player.playerNetworkManager.isSneaking.Value;
                player.playerNetworkManager.isBlocking.Value = false;

                player.playerCombatManager.CheckForHiddenStatus();
            }
        }

        private void HandleRBInput()
        {
            if (two_Hand_Input)
                return;

            if (RB_Input)
            {
                RB_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                player.playerNetworkManager.SetCharacterActionHand(true);

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_RB_Input)
            {
                player.playerNetworkManager.isChargingRightSpell.Value = true;
                player.playerNetworkManager.isHoldingArrow.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingRightSpell.Value = false;
                player.playerNetworkManager.isHoldingArrow.Value = false;
            }
        }

        private void HandleLBInput()
        {
            if (LB_Input)
            {
                LB_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                player.playerNetworkManager.SetCharacterActionHand(false);

                if (player.playerNetworkManager.isTwoHandingRightWeapon.Value)
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_LB_Action, player.playerInventoryManager.currentRightHandWeapon);

                }
                else
                {
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.oh_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);

                }
            }
        }

        private void HandleHoldLBInput()
        {
            if (hold_LB_Input)
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = true;
            }
            else
            {
                player.playerNetworkManager.isChargingLeftSpell.Value = false;
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
                    player.playerNetworkManager.isChargingAttack.Value = hold_RT_Input;
                }
            }
        }

        private void HandleLTInput()
        {
            if (LT_Input)
            {
                LT_Input = false;

                WeaponItem weaponPerformingAshOfWar = player.playerCombatManager.SelectWeaponToPerformAshOfWar();

                weaponPerformingAshOfWar.ashOfWarAction.AttemptToPerformAction(player);

            }
        }

        private void HandleSwitchRightWeaponInput()
        {
            if (switch_Right_Weapon_Input)
            {
                switch_Right_Weapon_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switch_Left_Weapon_Input)
            {
                switch_Left_Weapon_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }

        private void HandleSwitchQuickSlotItemInput()
        {
            if (switch_Quick_Slot_Spell_Input)
            {
                switch_Quick_Slot_Spell_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchQuickSlotSpell();
            }
        }

        private void HandleSwitchQuickSlotSpellInput()
        {
            if (switch_Quick_Slot_Item_Input)
            {
                switch_Quick_Slot_Item_Input = false;

                if (PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (player.isPerformingAction)
                    return;

                if (player.playerCombatManager.isUsingItem)
                    return;

                player.playerEquipmentManager.SwitchQuickSlotItem();
            }
        }

        private void HandleInteractionInput()
        {
            if (interaction_Input)
            {
                interaction_Input = false;

                player.playerInteractionManager.Interact();
            }
        }

        private void ResetQueInputs()
        {
            que_RB_Input = false;
            que_RT_Input = false;
        }

        private void QueInputs(ref bool queInput)
        {

            ResetQueInputs();
            if (player.isPerformingAction || player.playerNetworkManager.isJumping.Value)
            {
                queInput = true;
                que_Input_Timer = default_Que_Input_Time;
                input_Que_Is_Active = true;
            }
        }

        private void ProcessQuedInput()
        {
            if (player.isDead.Value)
                return;

            if (que_RB_Input)
                RB_Input = true;

            if (que_RT_Input)
                RT_Input = true;
        }

        private void HandleQueuedInputs()
        {
            if (input_Que_Is_Active)
            {
                if (que_Input_Timer > 0)
                {
                    que_Input_Timer -= Time.deltaTime;
                    ProcessQuedInput();
                }
                else
                {
                    ResetQueInputs();
                    input_Que_Is_Active = false;
                    que_Input_Timer = 0;
                }
            }
        }

        private void HandleOpenCharacterMenuInput()
        {
            if (openCharacterMenuInput)
            {
                openCharacterMenuInput = false;

                if (!PlayerUIManager.Singleton.menuWindowIsOpen)
                {
                    PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
                    PlayerUIManager.Singleton.CloseAllMenuWindows();
                    PlayerUIManager.Singleton.playerUICharacterMenuManager.OpenMenu();
                }
                else
                {

                    PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
                    PlayerUIManager.Singleton.CloseAllMenuWindows();

                }
            }

        }

        private void HandleCloseUIInput()
        {
            if (closeMenuInput)
            {
                closeMenuInput = false;

                if (!PlayerUIManager.Singleton.menuWindowIsOpen)
                    return;

                if (PlayerUIManager.Singleton.openSubmenus.Count > 0)
                {
                    var currentMenu = PlayerUIManager.Singleton.openMenus.Peek();
                    currentMenu.CloseSubMenu();
                }
                else
                {
                        var currentMenu = PlayerUIManager.Singleton.openMenus.Peek();
                        currentMenu.CloseMenu();
                        if (PlayerUIManager.Singleton.openMenus.Count > 0)
                        {
                            var nextMenu = PlayerUIManager.Singleton.openMenus.Peek();
                            nextMenu.OpenMenu();
                        }
                        else
                        {
                            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
                            PlayerUIManager.Singleton.CloseAllMenuWindows();
                        }
                    
                }

            }
        }
    }
}