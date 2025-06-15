using UnityEngine;

namespace TraverserProject
{

    public class ToggleAttackType : StateMachineBehaviour
    {
        CharacterManager character;
        [SerializeField] AttackType attackType;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }

            character.characterCombatManager.currentAttackType = attackType;

        }

    }
}