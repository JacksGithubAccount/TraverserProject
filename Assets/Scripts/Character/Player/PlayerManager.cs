using TravserserProject;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;


            playerLocomotionManager.HandleAllMovement();

            playerStatsManager.RegenerateStamina();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.Singleton.player = this;
                PlayerInputManager.Singleton.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Singleton.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                //moved with implement save/load
                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.Singleton.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

            }
        }
        protected override void LateUpdate()
        {

            base.LateUpdate();
            PlayerCamera.Singleton.HandleAllCameraActions();
        }
    }
}