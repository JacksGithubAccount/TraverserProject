using System.Collections;
using UnityEngine;

namespace TraverserProject
{
    public class HealActivationManager : SpellManager
    {
        [Header("Instantiated FX")]
        private GameObject instantiatedSpellActivatedFX;

        public bool isFullyCharged = false;
        private CharacterManager spellCaster;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();

            if (spellTarget != null)
                transform.LookAt(spellTarget.characterCombatManager.lockOnTransform.position);

        }

        public void InitializeHeal(CharacterManager spellCaster)
        {
            this.spellCaster = spellCaster;
        }

        public void InstantiateSpellActivatedFX()
        {
            if (impactParticle == null)
                return;

            instantiatedSpellActivatedFX = Instantiate(impactParticle, transform.position, Quaternion.identity);

            HealManager healManager = instantiatedSpellActivatedFX.GetComponent<HealManager>();
            healManager.InitializeHeal(spellCaster);

            WorldSoundFXManager.Singleton.AlertNearbyCharactersToSound(transform.position, 5);
        }

        public void WaitThenInstantiateSpellActivatedFX(float timeToWait)
        {
            if (destructionFXCoroutine != null)
                StopCoroutine(destructionFXCoroutine);

            destructionFXCoroutine = StartCoroutine(WaitThenInstantiateFX(timeToWait));
        }

        private IEnumerator WaitThenInstantiateFX(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            InstantiateSpellActivatedFX();
        }
    }
}