using TraverserProject;
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Throwable")]
    public class ThrowableItem : QuickSlotItem
    {
        [Header("Projectile Velocity")]
        [SerializeField] float upwardVelocity = 3;
        [SerializeField] float forwardVelocity = 15;

        [Header("Throwable Model")]
        private GameObject instantiatedThrowableInHand = null;
        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            if (currentItemAmount < 1)
                return;

            player.playerCombatManager.isUsingItem = true;

            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc();
                instantiatedThrowableInHand = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            }
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (player.IsOwner)
            {
                //currentItemAmount--;

                PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

            }
            Transform itemInstantiationLocation;
            GameObject instantiatedThrowableThrown = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            itemInstantiationLocation = instantiatedThrowableInHand.transform;

            instantiatedThrowableThrown.transform.parent = itemInstantiationLocation.transform;
            instantiatedThrowableThrown.transform.localPosition = Vector3.zero;
            instantiatedThrowableThrown.transform.localRotation = Quaternion.identity;
            instantiatedThrowableThrown.transform.parent = null;

            Destroy(instantiatedThrowableInHand);


            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedThrowableThrown.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                Vector3 forwardDirection = PlayerCamera.Singleton.transform.forward;
                instantiatedThrowableThrown.transform.forward = forwardDirection;
            }

            Rigidbody spellRigidBody = instantiatedThrowableThrown.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedThrowableThrown.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedThrowableThrown.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            spellRigidBody.linearVelocity = totalVelocity;
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isAttacking.Value)
                return false;

            return true;
        }
    }
}
