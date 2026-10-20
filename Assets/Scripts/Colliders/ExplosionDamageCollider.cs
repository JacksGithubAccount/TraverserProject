using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class ExplosionDamageCollider : DamageCollider
    {
        [Header("Character")]
        public CharacterManager characterCausingExplosion;

        [Header("Explosion")]
        [SerializeField] float explosionTime = 0.2f; //time it takes for explosion to reach it's full radius
        [SerializeField] bool ignoreFriendlyFireRestrictions = false; //if true, will damage character no matter the alignment
        [HideInInspector] public float startingRadius = 0.01f;
        private float maximumExplosionRadius = 0;

        [Header("Collision")]
        private SphereCollider explosionCollider;

        [Header("FX")]
        [SerializeField] ParticleSystem explosionParticles;

        protected override void Start()
        {
            base.Start();

            explosionCollider = damageCollider as SphereCollider;
            startingRadius = explosionCollider.radius;

        }

        public void Explode(CharacterManager characterCausingDamage, float maximumRadius)
        {
            if (characterCausingDamage != null)
                characterCausingExplosion = characterCausingDamage;

            maximumExplosionRadius = maximumRadius;
            damageCollider.enabled = true;

            StartCoroutine(WaitThenDisableExplosionCollider());

            if (explosionParticles == null)
                return;

            //make the explosion the same size as the passed explosion radius
            explosionParticles.transform.localScale = new Vector3(maximumRadius, maximumRadius, maximumRadius);
            explosionParticles.Play();
        }

        private IEnumerator WaitThenDisableExplosionCollider()
        {
            float timer = 0;

            while (timer < 1)
            {
                timer += Time.deltaTime / explosionTime;

                if (explosionCollider != null)
                    explosionCollider.radius = Mathf.Lerp(startingRadius, maximumExplosionRadius, timer);

                yield return null;
            }

            damageCollider.enabled = false;

            //check for breakables and optionally destroy them
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.transform.GetComponent<CharacterManager>();

            if (damageTarget == null)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (characterCausingExplosion != null && !WorldUtilityManager.Singleton.CanIDamageThisTarget(characterCausingExplosion.characterGroup, damageTarget.characterGroup) && !ignoreFriendlyFireRestrictions)
                return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            CheckForBlock(damageTarget);
            DamageTarget(damageTarget);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (damageTarget == null)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (damageTarget.characterNetworkManager.isInvulnerable.Value)
                return;

            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);


            //step 1, load up the damage effect
            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);
            damageEffect.characterCausingDamage = characterCausingExplosion;
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;


            //step 2, sent it to the server to play for everyone else
            if (characterCausingExplosion != null)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.OwnerClientId, damageTarget.NetworkObjectId, damageEffect.characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z, PhysicalDamageType.Regular, false);

            }
            else
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.OwnerClientId, damageTarget.NetworkObjectId, 0,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z, PhysicalDamageType.Regular, false);
            }
            //step 3, play it instantly
            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            if (damageTarget == null)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (charactersDamaged.Contains(damageTarget))
                return;

            GetBlockingDotValues(damageTarget);

            if (!damageTarget.characterNetworkManager.isBlocking.Value)
                return;

            if (dotValueFromAttackToDamageTarget <= 0.3f)
                return;

            charactersDamaged.Add(damageTarget);

            TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeBlockedDamageEffect);

            damageEffect.characterCausingDamage = characterCausingExplosion;
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.staminaDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;

            if (characterCausingExplosion != null)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.OwnerClientId, damageTarget.NetworkObjectId, damageEffect.characterCausingDamage.NetworkObjectId,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z, PhysicalDamageType.Regular, true);

            }
            else
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(damageTarget.OwnerClientId, damageTarget.NetworkObjectId, 0,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z, PhysicalDamageType.Regular, false);
            }

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        }


    }
}