using System.Collections;
using UnityEngine;

namespace TraverserProject
{
    public class HealManager : SpellManager
    {
        [Header("Colliders")]
        public HealDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;
        private Rigidbody fireBallRigidBody;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

            fireBallRigidBody = GetComponent<Rigidbody>();
            damageCollider = GetComponentInChildren<HealDamageCollider>();
        }

        protected override void Update()
        {
            base.Update();

            if (spellTarget != null)
                transform.LookAt(spellTarget.characterCombatManager.lockOnTransform.position);

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                InstantiateSpellDestructionFX();
            }
        }

        public void InitializeHeal(CharacterManager spellCaster)
        {
            damageCollider.spellCaster = spellCaster;

            //setup damage formula
            damageCollider.recoveryAmount = 50;

        }

        public void InstantiateSpellDestructionFX()
        {
            instantiatedDestructionFX = Instantiate(impactParticle, transform.position, Quaternion.identity);
            
            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 5);
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