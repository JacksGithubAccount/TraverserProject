using UnityEngine;

namespace TraverserProject
{

    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        [SerializeField] float vertical;
        [SerializeField] float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
        {
            character.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
        }
        public virtual void PlayTargetActionAnimation(string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;
        }

    }

}