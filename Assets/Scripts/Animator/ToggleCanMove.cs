using UnityEngine;

namespace TraverserProject
{

    public class ToggleCanMove : StateMachineBehaviour
    {
        CharacterManager character;

        [Header("On State Enter")]
        [SerializeField] bool activateOnStateEnter = false;

        [Header("Can Move Status")]
        [SerializeField] bool canMove = true;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!activateOnStateEnter)
                return;

            if (character == null)
                character = animator.GetComponent<CharacterManager>();

            if (character == null)
                return;

            if (canMove)
                character.characterLocomotionManager.EnableCanMove();

            if (!canMove)
                character.characterLocomotionManager.DisableCanMove();
        }


        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!activateOnStateEnter)
                return;

            if (character == null)
                return;

            if (canMove)
                character.characterLocomotionManager.EnableCanMove();

            if (!canMove)
                character.characterLocomotionManager.DisableCanMove();
        }

    }
}