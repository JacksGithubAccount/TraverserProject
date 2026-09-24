using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Accessory")]
    public class AccessoryEquipmentItem : EquipmentItem
    {
        [Header("Bar Modifier")]
        public int maxHealthModifier;
        public int maxFocusPointModifier;
        public int maxStaminaModifier;

        [Header("Negation")]
        public int armorPhysicalDamageAbsorptionModifier;
        public int armorMagicDamageAbsorptionModifier;
        public int armorFireDamageAbsorptionModifier;
        public int armorLightningDamageAbsorptionModifier;
        public int armorHolyDamageAbsorptionModifier;

        [Header("Damage")]
        public int physicalDamageModifier;
        public int magicDamageModifier;
        public int fireDamageModifier;
        public int lightningDamageModifier;
        public int holyDamageModifier;

        [Header("Stamina Regeneration")]
        public float staminaRegenerationPercentageModifier;

        [Header("Static Character Effect")]
        public StaticCharacterEffect staticEffect; //not implemented


        [Header("Item Effect Description")]
        public string itemEffectDescription;


    }
}