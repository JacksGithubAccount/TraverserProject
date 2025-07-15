using UnityEngine;

namespace TraverserProject
{

    public class ToggleBlockingController : StateMachineBehaviour
    {
        PlayerManager player;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null)
                player = animator.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (player.characterNetworkManager.isBlocking.Value)
            {
                player.characterAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);
            }
            else
            {
                player.characterAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }
        }


        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null)
                player = animator.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (player.characterNetworkManager.isBlocking.Value)
            {
                player.characterAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);
            }
            else
            {
                player.characterAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }
        }

    }
}