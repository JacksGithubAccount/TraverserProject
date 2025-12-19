using TraverserProject;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Status Cure Item")]
public class StatusCureItem : QuickSlotItem
{
    [Header("Build Up Type to cure")]
    public BuildUp buildUp;

    protected GameObject statusCureVFX;

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
