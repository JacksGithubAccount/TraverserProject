using UnityEngine;

namespace TraverserProject
{

    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager characterShootingProjectile;

        [Header("Collision")]
        private bool hasCollided = false;
        public Rigidbody rigidBody;

        protected override void Awake()
        {
            base.Awake();

            rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (rigidBody.linearVelocity != Vector3.zero)
            {
                rigidBody.rotation = Quaternion.LookRotation(rigidBody.linearVelocity);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                //hasCollided = true;

                CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();


                if (characterShootingProjectile == null)
                    return;

                if (potentialTarget == null)
                    return;

                if (WorldUtilityManager.Singleton.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
                {
                    DamageTarget(potentialTarget);
                }

                Destroy(gameObject);

            }
        }

    }
}