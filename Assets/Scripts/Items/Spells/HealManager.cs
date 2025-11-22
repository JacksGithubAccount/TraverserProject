using System.Collections;
using UnityEngine;

namespace TraverserProject
{
    public class HealManager : SpellManager
    {
        [Header("Colliders")]
        public HealDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedSpellPart2FX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

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
            }
        }

        public void InitializeHeal(CharacterManager spellCaster)
        {
            damageCollider.spellCaster = spellCaster;

            //setup damage formula
            damageCollider.recoveryAmount = 50;

        }
    }
}