using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Status Buff Item")]
    public class StatusBuffItem : QuickSlotItem
    {
        [Header("Negation")]
        public float armorPhysicalDamageAbsorptionModifier;
        public float armorMagicDamageAbsorptionModifier;
        public float armorFireDamageAbsorptionModifier;
        public float armorLightningDamageAbsorptionModifier;
        public float armorHolyDamageAbsorptionModifier;

        [Header("Stamina Regeneration")]
        public float staminaRegenerationPercentageModifier = 15;

        [Header("Buff Duration")]
        public int buffDuration = 180;

        protected GameObject statusBuffVFX;

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


            if (armorPhysicalDamageAbsorptionModifier != 0 && armorMagicDamageAbsorptionModifier != 0 && armorFireDamageAbsorptionModifier != 0 &&
                armorLightningDamageAbsorptionModifier != 0 && armorHolyDamageAbsorptionModifier != 0)
            {
                ModifyArmorAbsorptionForATimeEffect absorptionBuff = Instantiate(WorldCharacterEffectsManager.Singleton.itemAbsorptionBuffEffect);
                absorptionBuff.armorPhysicalDamageAbsorptionModifer = armorPhysicalDamageAbsorptionModifier;
                absorptionBuff.armorFireDamageAbsorptionModifer = armorFireDamageAbsorptionModifier;
                absorptionBuff.armorMagicDamageAbsorptionModifer = armorMagicDamageAbsorptionModifier;
                absorptionBuff.armorLightningDamageAbsorptionModifer = armorLightningDamageAbsorptionModifier;
                absorptionBuff.armorHolyDamageAbsorptionModifer = armorHolyDamageAbsorptionModifier;
                absorptionBuff.defaultLengthOfEffect = buffDuration;

                player.playerEffectsManager.AddTimedEffect(absorptionBuff);

                player.playerStatsManager.CalculateTotalArmorAbsorption();
            }

            if(staminaRegenerationPercentageModifier != 0)
            {
                ModifyStaminaRegenerationForATimeEffect staminaBuff = Instantiate(WorldCharacterEffectsManager.Singleton.itemStaminaRegenerationEffect);
                staminaBuff.staminaRegenerationPercentageModifier = staminaRegenerationPercentageModifier;
                staminaBuff.defaultLengthOfEffect = buffDuration;

                player.playerEffectsManager.AddTimedEffect(staminaBuff);
            }

            statusBuffVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
            statusBuffVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            statusBuffVFX.transform.root.rotation = Quaternion.identity;




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
