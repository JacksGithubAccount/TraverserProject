using UnityEngine;

namespace TraverserProject
{

    public class ToggleCanExitLadder : StateMachineBehaviour
    {
        CharacterManager character;

        [Header("Right Hand")]
        [SerializeField] bool canExitRight = true;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
                character = animator.GetComponent<CharacterManager>();

            if (character == null)
                return;

            character.characterLocomotionManager.canExitTopOfLadder = true;

            if (canExitRight)
                character.characterLocomotionManager.canExitLadderWithRightHand = true;
            if (!canExitRight)
                character.characterLocomotionManager.canExitLadderWithLeftHand = true;

        }


        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (character == null)
                return;

            character.characterLocomotionManager.canExitTopOfLadder = false;

            if (canExitRight)
                character.characterLocomotionManager.canExitLadderWithRightHand = false;
            if (!canExitRight)
                character.characterLocomotionManager.canExitLadderWithLeftHand = false;
        }
    }
}