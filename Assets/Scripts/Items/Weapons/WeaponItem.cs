using UnityEngine;

namespace TraverserProject
{

    public class WeaponItem : EquipmentItem
    {
        [Header("Animations")]
        public AnimatorOverrideController weaponAnimator;

        [Header("Model Instantiation")]
        public WeaponModelType weaponModelType;

        [Header("VFX")]
        [HideInInspector] public GameObject weaponSwingVFX;

        [Header("Weapon Class")]
        public WeaponClass weaponClass;

        [Header("Upgrade Level")]
        public UpgradeLevel upgradeLevel;

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
        public float light_Attack_01_Modifier = 1.0f;
        public float light_Attack_02_Modifier = 1.1f;
        public float heavy_Attack_01_Modifier = 1.4f;
        public float heavy_Attack_02_Modifier = 1.5f;
        public float charge_Attack_01_Modifier = 2.0f;
        public float charge_Attack_02_Modifier = 2.1f;
        public float running_Light_Attack_01_Modifier = 1.1f;
        public float running_Heavy_Attack_01_Modifier = 1.5f;
        public float rolling_Light_Attack_01_Modifier = 0.9f;
        public float rolling_Heavy_Attack_01_Modifier = 1.3f;
        public float backstep_Light_Attack_01_Modifier = 1.0f;
        public float backstep_Heavy_Attack_01_Modifier = 1.4f;
        public float jumping_Light_Attack_01_Modifier = 1.0f;
        public float jumping_Heavy_Attack_01_Modifier = 1.5f;

        [Header("Dual Attack Modifiers")]
        public float dual_Light_Attack_01_Modifier = 0.77f;
        public float dual_Light_Attack_02_Modifier = 0.87f;
        public float dual_Heavy_Attack_01_Modifier = 0.94f;
        public float dual_Heavy_Attack_02_Modifier = 1.0f;
        public float dual_Charge_Attack_01_Modifier = 1.3f;
        public float dual_Charge_Attack_02_Modifier = 1.4f;
        public float dual_Running_Light_Attack_01_Modifier = 0.77f;
        public float dual_Running_Heavy_Attack_01_Modifier = 0.94f;
        public float dual_Rolling_Light_Attack_01_Modifier = 0.68f;
        public float dual_Rolling_Heavy_Attack_01_Modifier = 0.81f;
        public float dual_Backstep_Light_Attack_01_Modifier = 0.77f;
        public float dual_Backstep_Heavy_Attack_01_Modifier = 0.9f;
        public float dual_Jumping_Light_Attack_01_Modifier = 0.77f;
        public float dual_Jumping_Heavy_Attack_01_Modifier = 0.98f;

        [Header("Stamina Cost Modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        public float heavyAttackStaminaCostMultiplier = 1.3f;
        public float chargedAttackStaminaCostMultiplier = 1.5f;
        public float runningLightAttackStaminaCostMultiplier = 1.0f;
        public float runningHeavyAttackStaminaCostMultiplier = 0.4f;
        public float rollingLightAttackStaminaCostMultiplier = 0.9f;
        public float rollingHeavyAttackStaminaCostMultiplier = 1.4f;
        public float backstepLightAttackStaminaCostMultiplier = 0.9f;
        public float backstepHeavyAttackStaminaCostMultiplier = 1.3f;
        public float jumpingLightAttackStaminaCostMultiplier = 1.0f;
        public float jumpingHeavyAttackStaminaCostMultiplier = 2.0f;

        [Header("Weapon Blocking Absorption")]
        public float physicalBaseDamageAbsorption = 50;
        public float magicBaseDamageAbsorption = 50;
        public float fireBaseDamageAbsorption = 50;
        public float lightningBaseDamageAbsorption = 50;
        public float holyBaseDamageAbsorption = 50;
        public float stability = 50;

        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; //one hand right bumper
        public WeaponItemAction oh_RT_Action; //one hand right trigger
        public WeaponItemAction oh_LB_Action; //one hand left bumper
        public AshOfWar ashOfWarAction; //one hand left trigger

        [Header("SFX")]
        public AudioClip[] whooshes;
        public AudioClip[] blocking;

    }
}