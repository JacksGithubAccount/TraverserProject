using UnityEngine;

namespace TraverserProject
{

    public class BombManager : MonoBehaviour
    {
        private CharacterManager bombThrower;
        private CharacterManager bombTarget;

        [Header("Collision")]
        [SerializeField] Collider bombCollider;
        [SerializeField] Rigidbody bombRigidbody;
        private bool hasCollided = false;

        [SerializeField] float forwardVelocity = 25;
        [SerializeField] float upwardVelocity = 5;
        [SerializeField] float smoothFactorWhenClose = 0.1f;
        private Vector3 refVelocity = Vector3.zero;
		
		[Header("Explosion")]
        [SerializeField] float radius = 2;
        [SerializeField] GameObject explosionVFX;

        private void Update()
        {
            if (bombTarget == null)
                return;

            smoothFactorWhenClose += Time.deltaTime;

            //slightly curves the bomb into a target provided it's not alreadt passed by the target
            Vector3 targetDirection = bombTarget.characterCombatManager.lockOnTransform.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, targetDirection);
            targetDirection.Normalize();

            if (dotProduct < 0.0f)
                return;

            Vector3 newVelocity = targetDirection * bombRigidbody.linearVelocity.magnitude;
            bombRigidbody.linearVelocity = Vector3.SmoothDamp(bombRigidbody.linearVelocity, newVelocity, ref refVelocity, smoothFactorWhenClose);
            transform.rotation = Quaternion.LookRotation(bombRigidbody.linearVelocity);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        public void ThrowBomb(CharacterManager characterThrowingBomb)
        {
            transform.parent = null;

            bombThrower = characterThrowingBomb;

            if (characterThrowingBomb.characterCombatManager.currentTarget != null)
                bombTarget = characterThrowingBomb.characterCombatManager.currentTarget;

            //stops bomb from potentially exploding in hands
            transform.forward = bombThrower.transform.forward;

            PlayerManager playerThrowingBomb = characterThrowingBomb as PlayerManager;

            if (playerThrowingBomb != null)
                transform.rotation = Quaternion.Euler(playerThrowingBomb.playerNetworkManager.cameraLookAngle.Value, playerThrowingBomb.transform.localEulerAngles.y, 0);

            bombThrower.ToggleIgnoreCollisionWithOtherCollider(true, bombCollider);
            bombCollider.enabled = true;

            Vector3 velocityForward = transform.forward * forwardVelocity;
            Vector3 velocityUpward = transform.up * upwardVelocity;
            Vector3 totalVelocity = velocityForward + velocityUpward;

            bombRigidbody.isKinematic = false;
            bombRigidbody.AddForce(totalVelocity, ForceMode.Impulse);
        }

        private void Explode()
        {
            if (hasCollided)
                return;

            hasCollided = true;
            GameObject explosion = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            ExplosionDamageCollider explosionDamageCollider = explosion.GetComponent<ExplosionDamageCollider>();
            explosionDamageCollider.Explode(bombThrower, radius);
            Destroy(gameObject);
        }
    }
}