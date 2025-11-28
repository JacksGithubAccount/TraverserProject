using System.Runtime.ExceptionServices;
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
                currentItemAmount--;
                player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].currentItemAmount--;
                PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

                //if out of items, remove from quickslot and current item
                if(currentItemAmount <= 0)
                {
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] = null;
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                }

            }
            Transform itemInstantiationLocation;
            GameObject instantiatedThrowableThrown = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            itemInstantiationLocation = player.playerEquipmentManager.rightHandWeaponSlot.transform;

            instantiatedThrowableThrown.transform.parent = itemInstantiationLocation.transform;
            instantiatedThrowableThrown.transform.localPosition = Vector3.zero;
            instantiatedThrowableThrown.transform.localRotation = Quaternion.identity;
            instantiatedThrowableThrown.transform.parent = null;

            ThrowableManager throwableManager = instantiatedThrowableThrown.GetComponent<ThrowableManager>();
            
            throwableManager.InitializeThrowable(player);
            Destroy(instantiatedThrowableInHand);

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedThrowableThrown.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                //gets rotation of camera and direction of player so throwable is aimable along up and down but not side to side
                Vector3 rotation = PlayerCamera.Singleton.cameraPivotTransform.eulerAngles;
                Quaternion throwRotation = Quaternion.Euler(rotation.x, player.transform.eulerAngles.y, rotation.z);
                instantiatedThrowableThrown.transform.rotation = throwRotation;
            }

            Rigidbody rigidBody = instantiatedThrowableThrown.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedThrowableThrown.transform.up * upwardVelocity;
            Vector3 forwardVelocityVector = instantiatedThrowableThrown.transform.forward * forwardVelocity;
            Vector3 totalVelocity = upwardVelocityVector + forwardVelocityVector;
            rigidBody.linearVelocity = totalVelocity;
        }

        public override bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isAttacking.Value)
                return false;

            if (player.playerCombatManager.isUsingItem)
                return false;

            return true;
        }

        public override int GetCurrentAmount(PlayerManager player)
        {
            int currentAmount = 0;

                currentAmount = currentItemAmount;

            return currentAmount;
        }
    }
}
