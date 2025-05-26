using UnityEngine;

namespace TraverserProject
{

    public class WeaponItem : BaseItem
    {
        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirement")]
        public int strengthREQ = 0;
        public int dexterityREQ = 0;
        public int intelligenceREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;



        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]
        public float light_Attack_01_Modifier = 1.1f;


        [Header("Stamina Cost Modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;

        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; //one hand right bumper

    }
}