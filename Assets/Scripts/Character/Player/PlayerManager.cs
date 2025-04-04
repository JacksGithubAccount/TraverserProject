using UnityEngine;

namespace TraverserProject
{
    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;


            playerLocomotionManager.HandleAllMovement();
        }
    }
}
