using UnityEngine;

namespace TraverserProject
{
    public class ArmorItem : EquipmentItem
    {
        [Header("Equipment Damage Absorption")]
        public float physicalDamageAbsorption;
        public float magicDamageAbsorption;
        public float fireDamageAbsorption;
        public float lightningDamageAbsorption;
        public float holyDamageAbsorption;

        [Header("Equipment Resistance Absorption")]
        public float immunity;   //rot and poison
        public float robustness; //bleed and frost
        public float focus;      //sleep and madness
        public float vitality;   //deathblight

        [Header("Poise")]
        public float poise;



    }
}
