using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class FlameFistManager : SpellManager
    {
        [Header("Colliders")]
        public FireBallDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;

        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponentInChildren<FireBallDamageCollider>();
        }

        protected override void Update()
        {
            base.Update();

            if (spellTarget != null)
                transform.LookAt(spellTarget.characterCombatManager.lockOnTransform.position);

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
                return;

            if (!hasCollided)
            {
                hasCollided = true;
                InstantiateSpellDestructionFX();
            }
        }

        public void InitializeFireBall(CharacterManager spellCaster)
        {
            damageCollider.spellCaster = spellCaster;

            //setup damage formula
            damageCollider.fireDamage = 150;

            if (isFullyCharged)
                damageCollider.fireDamage *= 1.4f;
        }

        public void InstantiateSpellDestructionFX()
        {
            if (isFullyCharged)
            {
                instantiatedDestructionFX = Instantiate(impactParticleFullCharge, transform.position, Quaternion.identity);
            }
            else
            {
                instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);
            }
            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 5);
            Destroy(gameObject);
        }

        public void WaitThenInstantiateSpellDestructionFX(float timeToWait)
        {
            if (destructionFXCoroutine != null)
                StopCoroutine(destructionFXCoroutine);

            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));

            StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }

        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            InstantiateSpellDestructionFX();
        }

    }
}