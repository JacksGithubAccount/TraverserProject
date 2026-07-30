using System.Linq;
using UnityEngine;

namespace TraverserProject
{

    public class QuickSlotItem : Item
    {
        [Header("Description")]
        [TextArea] public string itemEffect;

        [Header("Item Model")]
        [SerializeField] protected GameObject itemModel;

        [Header("Animation")]
        [SerializeField] protected string useItemAnimation;

        [Header("Flags")]
        public bool isConsumable = true;
        public bool dealsDamage = false;
        

        [Header("Batch Item Use")]
        public int numberOfItemsToUse = 1;

        [Header("Costs")]
        public int FPCost = 0;

        [Header("Scaling")]
        public int strengthScaling = 0;
        public int dexterityScaling = 0;
        public int intelligenceScaling = 0;
        public int faithScaling = 0;

        public virtual void AttemptToUseItem(PlayerManager player)
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


        public virtual void SuccessfullyUseItem(PlayerManager player)
        {
            if (player.IsOwner)
            {
                QuickSlotItem qsItem = player.playerInventoryManager.quickSlotItemsInQuickSlots.SingleOrDefault(x => x.itemID == this.itemID);

                if (!isConsumable)
                    return;

                if (numberOfItemsToUse == 1)
                {
                    currentItemAmount--;
                    if (qsItem != null)
                        qsItem.currentItemAmount--;
                }
                else if (numberOfItemsToUse > 1)
                {
                    currentItemAmount -= numberOfItemsToUse;
                    if (qsItem != null)
                        qsItem.currentItemAmount -= numberOfItemsToUse;
                }


                PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

                //if out of items, remove from quickslot and current item
                if (currentItemAmount <= 0)
                {
                    player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] = null;
                    player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
                }

            }
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            if (!player.playerCombatManager.isUsingItem && player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isAttacking.Value)
                return false;

            if (player.playerCombatManager.isUsingItem)
                return false;

            return true;
        }

        public virtual int GetCurrentAmount(PlayerManager player)
        {
            int currentAmount = 0;

            currentAmount = currentItemAmount;

            return currentAmount;
        }

    }
}