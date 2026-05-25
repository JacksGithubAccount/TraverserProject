using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Accessory")]
    public class AccessoryEquipmentItem : EquipmentItem
    {        
        [Header("Bar Modifier")]
        public float maxHealthModifier;
        public float maxFocusPointModifier;
        public float maxStaminaModifier;

        [Header("Negation")]
        public float armorPhysicalDamageAbsorptionModifier;
        public float armorMagicDamageAbsorptionModifier;
        public float armorFireDamageAbsorptionModifier;
        public float armorLightningDamageAbsorptionModifier;
        public float armorHolyDamageAbsorptionModifier;

        [Header("Stamina Regeneration")]
        public float staminaRegenerationPercentageModifier;

        [Header("Item Effect")]
        public string itemEffect;


    }
}