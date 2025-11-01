using UnityEngine;
using System.Collections;

namespace TraverserProject
{
    public class ThrowableManager : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] protected CharacterManager target;

        [Header("Colliders")]
        public ThrowableDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        private Rigidbody throwableRigidBody;
        private Coroutine destructionFXCoroutine;

        [Header("VFX")]
        [SerializeField] protected GameObject impactParticle;

        protected virtual void Awake()
        {
            throwableRigidBody = GetComponent<Rigidbody>();
            damageCollider = GetComponentInChildren<ThrowableDamageCollider>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            if (target != null)
                transform.LookAt(target.characterCombatManager.lockOnTransform.position);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
                return;

            if (!hasCollided)
            {
                hasCollided = true;
                InstantiateDestructionFX();
            }
        }

        public void InitializeThrowable(CharacterManager thrower)
        {
            damageCollider.itemThrower = thrower;

            //setup damage formula
            damageCollider.fireDamage = 150;

        }

        public void InstantiateDestructionFX()
        {
            instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);
            
            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 5);
            Destroy(gameObject);
        }

        public void WaitThenInstantiateDestructionFX(float timeToWait)
        {
            if (destructionFXCoroutine != null)
                StopCoroutine(destructionFXCoroutine);

            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));

            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }

        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            InstantiateDestructionFX();
        }
    }

}