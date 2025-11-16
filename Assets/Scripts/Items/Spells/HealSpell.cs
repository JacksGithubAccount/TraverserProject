using UnityEngine;

namespace TraverserProject
{
    public class HealSpell : SpellItem
    {
        public override void AttemptToCastSpell(PlayerManager player)
        {
            base.AttemptToCastSpell(player);

            if (!CanICastThisSpell(player))
                return;

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);
            }
        }

        public override void InstantiateWarmUpSpellFX(PlayerManager player)
        {
            base.InstantiateWarmUpSpellFX(player);

            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellCastWarmUpFX);

            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();

            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }

            instantiatedWarmUpSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedWarmUpSpellFX.transform.localPosition = Vector3.zero;
            instantiatedWarmUpSpellFX.transform.localRotation = Quaternion.identity;

            player.playerEffectsManager.activeSpellWarmUpFX = instantiatedWarmUpSpellFX;
        }

        public override void InstantiateReleaseFX(PlayerManager player)
        {
            base.InstantiateReleaseFX(player);

        }

        public override void SuccessfullyCastSpell(PlayerManager player)
        {
            base.SuccessfullyCastSpell(player);

            if (player.IsOwner)
                player.playerCombatManager.DestroyAllCurrentActionFX();


            SpellInstantiationLocation spellInstantiationLocation;
            GameObject instantiatedReleasedSpellFX = Instantiate(spellCastReleaseFX);
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                spellInstantiationLocation = player.playerEquipmentManager.rightWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();

            }
            else
            {
                spellInstantiationLocation = player.playerEquipmentManager.leftWeaponManager.GetComponentInChildren<SpellInstantiationLocation>();
            }
            instantiatedReleasedSpellFX.transform.parent = spellInstantiationLocation.transform;
            instantiatedReleasedSpellFX.transform.localPosition = Vector3.zero;
            instantiatedReleasedSpellFX.transform.localRotation = Quaternion.identity;
            instantiatedReleasedSpellFX.transform.parent = null;



            HealManager healManager = instantiatedReleasedSpellFX.GetComponent<HealManager>();
            HealManager.InitializeHeal(player);



            //shows ignoring collision of caster, but that is already done elsewhere
            /*Collider[] characterCollider = player.GetComponentInChildren<Collider>();
			Collider characterCollisionCollider = player.GetComponent<Collider>();
			Physics.IgnoreCollision(characterCollisionCollider, fireBallManager.damageCollider.damageCollider)
			foreach(var collider in characterColliders)
			{
				Physics.IgnoreCollision(collider, fireBallManager.damageCollider.damageCollider, true);
			}*/

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedReleasedSpellFX.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                //gets rotation of camera and direction of player so throwable is aimable along up and down but not side to side
                Vector3 rotation = PlayerCamera.Singleton.cameraPivotTransform.eulerAngles;
                Quaternion throwRotation = Quaternion.Euler(rotation.x, player.transform.eulerAngles.y, rotation.z);
                instantiatedReleasedSpellFX.transform.rotation = throwRotation;
            }

            Rigidbody spellRigidBody = instantiatedReleasedSpellFX.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedReleasedSpellFX.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedReleasedSpellFX.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            spellRigidBody.linearVelocity = totalVelocity;
        }
    }
}