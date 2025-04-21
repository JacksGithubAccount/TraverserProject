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

    }

}