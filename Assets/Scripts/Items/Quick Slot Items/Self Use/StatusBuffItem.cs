using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Status Buff Item")]
    public class StatusBuffItem : QuickSlotItem
    {
        [Header("Damage Modified")]
        public int physicalDamageModifier = 0;
        public int magicDamageModifer = 0;
        public int fireDamageModifier = 0;
        public int lightningDamageModifier = 0;
        public int holyDamageModifier = 0;

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

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject bag = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = bag;
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (armorPhysicalDamageAbsorptionModifier != 0 || armorMagicDamageAbsorptionModifier != 0 || armorFireDamageAbsorptionModifier != 0 ||
                armorLightningDamageAbsorptionModifier != 0 || armorHolyDamageAbsorptionModifier != 0)
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
            if (physicalDamageModifier != 0 || magicDamageModifer != 0 || fireDamageModifier != 0 ||
                lightningDamageModifier != 0 || holyDamageModifier != 0)
            {
                ModifyCharacterDamageForATimeEffect damageBuff = Instantiate(WorldCharacterEffectsManager.Singleton.itemDamageBuffEffect);
                damageBuff.physicalDamageModified = physicalDamageModifier;
                damageBuff.magicDamageModifed = magicDamageModifer;
                damageBuff.fireDamageModified = fireDamageModifier;
                damageBuff.lightningDamageModified = lightningDamageModifier;
                damageBuff.holyDamageModified = holyDamageModifier;
                damageBuff.defaultLengthOfEffect = buffDuration;

                player.playerEffectsManager.AddTimedEffect(damageBuff);
            }

            statusBuffVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
            statusBuffVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            statusBuffVFX.transform.root.rotation = Quaternion.identity;
        }
    } 
}
