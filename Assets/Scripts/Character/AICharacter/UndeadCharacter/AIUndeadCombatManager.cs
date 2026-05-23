using UnityEngine;

namespace TraverserProject
{

    public class AIUndeadCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] ManualDamageCollider rightHandDamageCollider;
        [SerializeField] ManualDamageCollider leftHandDamageCollider;

        [Header("Damage Modifiers")]
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] PhysicalDamageType attack01PhysicalDamageType = PhysicalDamageType.Regular;
        [SerializeField] float attack02DamageModifier = 1.2f;
        [SerializeField] PhysicalDamageType attack02PhysicalDamageType = PhysicalDamageType.Regular;

        public void SetAttack01Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            rightHandDamageCollider.physicalDamageType = attack01PhysicalDamageType;
            leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            leftHandDamageCollider.physicalDamageType = attack01PhysicalDamageType;
        }

        public void SetAttack02Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            rightHandDamageCollider.physicalDamageType = attack02PhysicalDamageType;
            leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            leftHandDamageCollider.physicalDamageType = attack02PhysicalDamageType;
        }

        public void OpenRightHandDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntFX();
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGruntFX();
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

        public override void CloseAllDamageColliders()
        {
            base.CloseAllDamageColliders();
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }

    }
}