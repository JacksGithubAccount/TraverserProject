using UnityEngine;

namespace TraverserProject
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;


            playerLocomotionManager.HandleAllMovement();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                PlayerCamera.Singleton.player = this;
                PlayerInputManager.Singleton.player = this;
            }
        }
        protected override void LateUpdate()
        {

            base.LateUpdate();
            PlayerCamera.Singleton.HandleAllCameraActions();
        }
    }
}