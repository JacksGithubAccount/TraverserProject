using UnityEngine;

namespace TraverserProject
{

    public class AIUndeadCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
        [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.2f;

        public void SetAttack01Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenRightHandDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }



    }
}