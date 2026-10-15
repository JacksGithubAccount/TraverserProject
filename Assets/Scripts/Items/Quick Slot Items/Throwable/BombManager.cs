using UnityEngine;

namespace TraverserProject
{

    public class BombManager : MonoBehaviour
    {
        private CharacterManager bombThrower;

        [Header("Collision")]
        [SerializeField] Collider bombCollider;
        [SerializeField] Rigidbody bombRigidbody;

        [SerializeField] float forwardVelocity = 25;
        [SerializeField] float upwardVelocity = 5;

        public void ThrowBomb(CharacterManager characterThrowingBomb)
        {
            transform.parent = null;

            bombThrower = characterThrowingBomb;

            //stops bomb from potentially exploding in hands
            transform.forward = bombThrower.transform.forward;
            bombThrower.ToggleIgnoreCollisionWithOtherCollider(true, bombCollider);
            bombCollider.enabled = true;

            Vector3 velocityForward = transform.forward * forwardVelocity;
            Vector3 velocityUpward = transform.up * upwardVelocity;
            Vector3 totalVelocity = velocityForward + velocityUpward;

            bombRigidbody.isKinematic = false;
            bombRigidbody.AddForce(totalVelocity, ForceMode.Impulse);
        }
    }
}