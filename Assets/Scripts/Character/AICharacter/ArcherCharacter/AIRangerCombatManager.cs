using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TraverserProject
{

    public class AIRangerCombatManager : AICharacterCombatManager
    {
        AIRangerManager ranger;

        [Header("Aim Time")]
        [SerializeField] float minimumTimeToAim = 1;
        [SerializeField] float maximumTimeToAim = 4;
        private Coroutine aimCoroutine;

        protected override void Awake()
        {
            base.Awake();

            ranger = GetComponent<AIRangerManager>();
        }

        public override void DrawProjectile()
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

            if (ranger.IsOwner)
            {
                if (aimCoroutine != null)
                    StopCoroutine(aimCoroutine);

                aimCoroutine = StartCoroutine(HoldArrowForATime(Random.Range(minimumTimeToAim, maximumTimeToAim)));



            }

            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_Draw_01");

            GameObject arrow = Instantiate(projectile.drawProjectileModel, drawHand);
            ranger.characterEffectsManager.activeDrawnProjectileFX = arrow;


            ranger.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.notchArrowSFX));
        }

        private IEnumerator HoldArrowForATime(float time)
        {
            ranger.aiCharacterNetworkManager.hasArrowNotched.Value = true;
            ranger.aiCharacterNetworkManager.isHoldingArrow.Value = true;
            ranger.aiCharacterLocomotionManager.canRotate = true;
            yield return new WaitForSeconds(time);

            bool canFire = false;
            //while we arent looking at our target, do not fire
            while (!canFire)
            {
                if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                    canFire = true;
                if (viewableAngle < 5 && viewableAngle > -5 && distanceFromTarget > 1 && HasLineOfSight())
                    canFire = true;

                yield return null;
            }
            ranger.aiCharacterLocomotionManager.canRotate = false;
            ranger.aiCharacterNetworkManager.isHoldingArrow.Value = false;
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