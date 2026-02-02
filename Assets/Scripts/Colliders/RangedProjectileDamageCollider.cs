using UnityEngine;

namespace TraverserProject
{

    public class RangedProjectileDamageCollider : DamageCollider
    {
        [Header("Marksmen")]
        public CharacterManager characterShootingProjectile;

        [Header("Collision")]
        private bool hasPenetratedSurface = false;
        public Rigidbody rigidBody;
        private CapsuleCollider capsuleCollider;

        [Header("Penetration")]
        [SerializeField] float minimumPenetration = 0.0f;
        [SerializeField] float maximumPenetration = 0.1f;

        [Header("VFX")]
        [SerializeField] ParticleSystem trailVFX;

        protected override void Awake()
        {
            base.Awake();

            rigidBody = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
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
            CreatePenetrationIntoObject(collision);

            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 3);

            CharacterManager potentialTarget = collision.transform.gameObject.GetComponent<CharacterManager>();


            if (characterShootingProjectile == null)
                return;

            Collider contactCollider = collision.gameObject.GetComponent<Collider>();

            if (contactCollider != null)
                contactPoint = contactCollider.ClosestPointOnBounds(transform.position);

            if (potentialTarget == null)
                return;

            if (WorldUtilityManager.Singleton.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
            {
                CheckForBlock(potentialTarget);
                DamageTarget(potentialTarget);
            }



        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            float angle = Vector3.Angle(damageTarget.transform.forward, transform.forward);

            if (damageTarget.characterNetworkManager.isBlocking.Value && angle > 145)
            {
                charactersDamaged.Add(damageTarget);
                TakeBlockedDamageEffect blockedDamageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeBlockedDamageEffect);

                if (characterShootingProjectile != null)
                    blockedDamageEffect.characterCausingDamage = characterShootingProjectile;

                blockedDamageEffect.physicalDamage = physicalDamage;
                blockedDamageEffect.magicDamage = magicDamage;
                blockedDamageEffect.fireDamage = fireDamage;
                blockedDamageEffect.lightningDamage = lightningDamage;
                blockedDamageEffect.holyDamage = holyDamage;
                blockedDamageEffect.poiseDamage = poiseDamage;
                blockedDamageEffect.staminaDamage = poiseDamage;
                blockedDamageEffect.contactPoint = contactPoint;

                damageTarget.characterEffectsManager.ProcessInstantEffect(blockedDamageEffect);
            }


        }

        private void CreatePenetrationIntoObject(Collision hit)
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
                transform.position += transform.forward * (Random.Range(minimumPenetration, maximumPenetration));

                //disables colliders and rigidbody
                rigidBody.isKinematic = true;
                capsuleCollider.enabled = false;

                //disables vfx trail
                if (trailVFX != null)
                {
                    trailVFX.Stop();
                }

                //destroys collider and arrow after a time
                Destroy(GetComponent<RangedProjectileDamageCollider>());
                Destroy(gameObject, 20);
            }
        }

    }
}