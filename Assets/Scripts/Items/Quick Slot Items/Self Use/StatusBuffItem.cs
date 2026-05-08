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

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

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
    } 
}
