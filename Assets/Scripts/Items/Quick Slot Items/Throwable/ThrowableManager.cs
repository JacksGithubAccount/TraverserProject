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
        public GroundCollider groundCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        [Header("Collision")]
        private bool hasCollided = false;
        public Rigidbody throwableRigidBody;
        private Coroutine destructionFXCoroutine;
        private bool hasPenetratedSurface = false;

        [Header("VFX")]
        [SerializeField] protected GameObject impactParticle;

        [Header("Flags")]
        public ThrowableType throwableType;




        protected virtual void Awake()
        {
            throwableRigidBody = GetComponent<Rigidbody>();
            damageCollider = GetComponentInChildren<ThrowableDamageCollider>();
            groundCollider = GetComponentInChildren<GroundCollider>();
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

                switch(throwableType)
                {
                    case ThrowableType.Destructible:
                        InstantiateDestructionFX();
                        break;
                    case ThrowableType.Lingering:
                        CreateObjectOnCollision(collision, false);
                        break;
                    case ThrowableType.Persistant:
                        CreateObjectOnCollision(collision, true);
                        break;
                    default:
                        break;
                }
                    

                
            }
        }

        public void InitializeThrowable(CharacterManager thrower)
        {
            if (damageCollider == null)
                return;

            damageCollider.itemThrower = thrower;

            //setup damage formula
            //damageCollider.fireDamage = 150;

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

        public void CreateObjectOnCollision(Collision hit, bool isPersistant)
        {
            float penetrationDepth = 0;
            float upwardDepth = 0;

            if (!isPersistant)
            {
                penetrationDepth = -.5f;
                upwardDepth = 0;
            }
            else
            {
                penetrationDepth = 0;
                upwardDepth = .1f;
            }

            if (!hasPenetratedSurface)
            {
                hasPenetratedSurface = true;

                //contact point
                gameObject.transform.position = hit.GetContact(0).point;
                var emptyObject = new GameObject();
                emptyObject.transform.parent = hit.collider.transform;
                gameObject.transform.SetParent(emptyObject.transform, true);

                //how far the arrow penetrates
                transform.position += transform.forward * penetrationDepth;
                transform.position += transform.up * upwardDepth;

                //disables colliders and rigidbody
                throwableRigidBody.isKinematic = true;

                //destroys colliders and throwable after a time
                Destroy(GetComponentInChildren<ThrowableDamageCollider>().damageCollider);
                Destroy(GetComponentInChildren<ThrowableDamageCollider>());
                Destroy(GetComponent<Collider>());

                if (!isPersistant)
                    Destroy(gameObject, 20);
            }
        }
        public void CreateObjectOnCollision(Collider collider, bool isPersistant, RaycastHit hit)
        {
            float penetrationDepth = 0;
            float upwardDepth = 0;

            if (!isPersistant)
            {
                penetrationDepth = -2.0f;
                upwardDepth = 0;
            }

            if (!hasPenetratedSurface)
            {
                hasPenetratedSurface = true;

                //contact point
                gameObject.transform.position = hit.point;
                var emptyObject = new GameObject();
                emptyObject.transform.parent = collider.transform;
                gameObject.transform.SetParent(emptyObject.transform, true);

                //how far the arrow penetrates
                gameObject.transform.position += transform.forward * penetrationDepth;
                gameObject.transform.position += transform.up * upwardDepth;

                //disables colliders and rigidbody
                throwableRigidBody.isKinematic = true;

                //destroys colliders
                Destroy(GetComponentInChildren<ThrowableDamageCollider>().damageCollider);
                Destroy(GetComponentInChildren<ThrowableDamageCollider>());
                Destroy(GetComponent<Collider>());

                if (!isPersistant)
                    Destroy(gameObject, 20);
            }
        }
    }

}