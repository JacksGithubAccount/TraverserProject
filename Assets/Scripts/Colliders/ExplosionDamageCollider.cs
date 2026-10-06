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

            startingRadius = explosionCollider.radius;

        }

        public void Explode(float maximumRadius)
        {
            maximumExplosionRadius = maximumRadius;
            damageCollider.enabled = true;

            StartCoroutine(WaitThenDisableExplosionCollider());

            if (explosionParticles == null)
                return;

            //make the explosion the same size as the passed explosion radius
            var shape = explosionParticles.shape;
            shape.radius = maximumRadius;
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
            base.OnTriggerEnter(other);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            base.DamageTarget(damageTarget);
        }

        protected override void CheckForBlock(CharacterManager damageTarget)
        {
            base.CheckForBlock(damageTarget);
        }

    }
}