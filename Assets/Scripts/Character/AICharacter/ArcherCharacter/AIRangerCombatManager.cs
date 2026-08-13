using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class AIRangerCombatManager : AICharacterCombatManager
    {
        AIRangerManager ranger;

        protected override void Awake()
        {
            base.Awake();

            ranger = GetComponent<AIRangerManager>();
        }

        public virtual void DrawProjectile()
        {
            Animator bowAnimator = ranger.aiRangerEquipmentManager.bowAnimator;
            RangedProjectileItem projectile = ranger.aiRangerEquipmentManager.projectile;
            Transform drawHand = ranger.aiRangerEquipmentManager.drawHand;

            if (bowAnimator == null)
                return;

            if (projectile == null)
                return;

            if (drawHand == null)
                return;

            ranger.characterAnimatorManager.PlayTargetActionAnimation("Bow_Draw_01", true);

            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Draw_01");

            GameObject arrow = Instantiate(projectile.drawProjectileModel, drawHand);
            ranger.characterEffectsManager.activeDrawnProjectileFX = arrow;


            ranger.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.notchArrowSFX));
        }

        public override void ReleaseArrow()
        {
            if (ranger.IsOwner)
                ranger.aiCharacterNetworkManager.hasArrowNotched.Value = false;

            if (ranger.characterEffectsManager.activeDrawnProjectileFX != null)
                Destroy(ranger.characterEffectsManager.activeDrawnProjectileFX);

            ranger.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.releaseArrowSFX));

            Animator bowAnimator = ranger.aiRangerEquipmentManager.bowAnimator;

            if (bowAnimator == null)
                return;

            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("Bow_Fire_01");

            RangedProjectileItem projectileItem = ranger.aiRangerEquipmentManager.projectile;

            if (projectileItem == null)
                return;

            Transform projectileInstantiationLocation;
            GameObject projectileGameObject;
            Rigidbody projectileRigidbody;
            RangedProjectileDamageCollider projectileDamageCollider;

            projectileInstantiationLocation = ranger.aiCharacterCombatManager.lockOnTransform; //you can change depending on creature firing projectile
            projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
            projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            projectileDamageCollider.physicalDamage = 100;
            projectileDamageCollider.characterShootingProjectile = ranger;

            //AI should always have a target when firing unless they disconnect whilst ai is waiting to release arrow
            if (ranger.aiCharacterCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(ranger.aiCharacterCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - projectileGameObject.transform.position);
                projectileGameObject.transform.rotation = arrowRotation;
            }
            else
            {
                Quaternion arrowRotation = Quaternion.LookRotation(ranger.transform.forward);
                projectileGameObject.transform.rotation = arrowRotation;
            }

            Collider[] characterColliders = ranger.GetComponentsInChildren<Collider>();
            List<Collider> collidersArrowWillIgnore = new List<Collider>();

            foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);

            foreach (Collider hitBox in collidersArrowWillIgnore)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;

        }

    }
}