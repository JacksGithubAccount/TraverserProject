using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TraverserProject;
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Throwable")]
    public class ThrowableItem : QuickSlotItem
    {
        [Header("Throwable Type")]
        [SerializeField] ThrowableType throwableType;

        [Header("Projectile Velocity")]
        [SerializeField] protected float upwardVelocity = 3;
        [SerializeField] protected float forwardVelocity = 15;

        [Header("Throwable Model")]
        protected GameObject instantiatedThrowableInHand = null;
        protected Material materialForRandomColor;

        [Header("Flags")]
        [SerializeField] bool randomColors = false;



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
                Renderer renderer = instantiatedThrowableInHand.GetComponentInChildren<Renderer>();

                if (randomColors)
                {
                    materialForRandomColor = WorldUtilityManager.Singleton.GetRandomMatFromRainbowMat();
                    renderer.material = materialForRandomColor;
                }
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
                if (currentItemAmount <= 0)
                {
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] = null;
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                }

            }

            Transform itemInstantiationLocation;
            GameObject instantiatedThrowableItem = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            itemInstantiationLocation = player.playerEquipmentManager.rightHandWeaponSlot.transform;

            instantiatedThrowableItem.transform.parent = itemInstantiationLocation.transform;
            instantiatedThrowableItem.transform.localPosition = Vector3.zero;
            instantiatedThrowableItem.transform.localRotation = Quaternion.identity;
            instantiatedThrowableItem.transform.parent = null;

            if (randomColors)
            {
                Renderer renderer = instantiatedThrowableItem.GetComponentInChildren<Renderer>();
                renderer.material = materialForRandomColor;

                Light light = instantiatedThrowableItem.GetComponentInChildren<Light>();
                if (light != null)
                    light.color = renderer.material.color;
            }

            ThrowableManager throwableManager = instantiatedThrowableItem.GetComponent<ThrowableManager>();

            throwableManager.throwableType = throwableType;

            throwableManager.InitializeThrowable(player);
            Destroy(instantiatedThrowableInHand);

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                instantiatedThrowableItem.transform.LookAt(player.playerCombatManager.currentTarget.transform.position);
            }
            else
            {
                //gets rotation of camera and direction of player so throwable is aimable along up and down but not side to side
                Vector3 rotation = PlayerCamera.Singleton.cameraPivotTransform.eulerAngles;
                Quaternion throwRotation = Quaternion.Euler(rotation.x, player.transform.eulerAngles.y, rotation.z);
                instantiatedThrowableItem.transform.rotation = throwRotation;
            }

            Rigidbody rigidBody = instantiatedThrowableItem.GetComponent<Rigidbody>();
            Vector3 upwardVelocityVector = instantiatedThrowableItem.transform.up;
            Vector3 forwardVelocityVector = instantiatedThrowableItem.transform.forward * forwardVelocity;
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
