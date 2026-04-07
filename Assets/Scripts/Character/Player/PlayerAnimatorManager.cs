using UnityEngine;

namespace TraverserProject
{

    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        private void OnAnimatorMove()
        {
            //if our character controller is disabled, we move our model with animations movement
            if (!player.characterController.enabled)
            {
                player.animator.ApplyBuiltinRootMotion();
                return;
            }
            if (player.characterAnimatorManager.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }





    }

}