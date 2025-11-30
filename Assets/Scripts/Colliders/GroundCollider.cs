using UnityEngine;

namespace TraverserProject
{
    public class GroundCollider : MonoBehaviour
    {
        [Header("Collider")]
        public Collider groundCollider;

        [Header("Collision")]
        private bool hasPenetratedSurface = false;
        public Rigidbody rigidBody;
        private CapsuleCollider capsuleCollider;

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CreateObjectOnGround(collision);
        }

        private void CreateObjectOnGround(Collision hit)
        {
            if (!hasPenetratedSurface)
            {
                hasPenetratedSurface = true;

                //contact point
                gameObject.transform.position = hit.GetContact(0).point;
                var emptyObject = new GameObject();
                emptyObject.transform.parent = hit.collider.transform;
                gameObject.transform.SetParent(emptyObject.transform, true);

                //how far the arrow penetrates
                transform.position += transform.forward * -.5f;

                //disables colliders and rigidbody
                rigidBody.isKinematic = true;
                capsuleCollider.enabled = false;

                //destroys collider and arrow after a time
                Destroy(GetComponent<GroundCollider>());
                Destroy(gameObject, 20);
            }
        }

        public virtual void EnableGroundCollider()
        {
            groundCollider.enabled = true;
        }
        public virtual void DisableGroundCollider()
        {
            groundCollider.enabled = false;
        }
    }
}