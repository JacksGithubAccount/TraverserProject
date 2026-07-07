using NUnit.Framework;
using System.Collections.Generic;
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

        [Header("Weapon Timed Effect")]
        public ModifyWeaponDamageForATimeEffect weaponTimedEffect;

        [Header("Weapon Class")]
        public WeaponClass weaponClass;

        [Header("Upgrade Level")]
        public UpgradeLevel upgradeLevel;

        [Header("Weapon Upgrade Amount")]
        public float physicalUpgradeAmount = 1.1f;
        public float magicUpgradeAmount = 1.1f;
        public float fireUpgradeAmount = 1.1f;
        public float lightningUpgradeAmount = 1.1f;
        public float holyUpgradeAmount = 1.1f;

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirement")]
        public int strengthREQ = 0;
        public int dexterityREQ = 0;
        public int intelligenceREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Scaling")]
        public int strengthScaling = 0;
        public int dexterityScaling = 0;
        public int intelligenceScaling = 0;
        public int faithScaling = 0;

        [Header("Weapon Base Damage")]
        public int physicalBaseDamage = 0;
        public int magicBaseDamage = 0;
        public int fireBaseDamage = 0;
        public int lightningBaseDamage = 0;
        public int holyBaseDamage = 0;

        [Header("Weapon Upgrade Damage")]
        public int physicalUpgradeDamage = 0;
        public int magicUpgradeDamage = 0;
        public int fireUpgradeDamage = 0;
        public int lightningUpgradeDamage = 0;
        public int holyUpgradeDamage = 0;

        [Header("Weapon Scaling Damage")]
        public int physicalScalingDamage = 0;
        public int magicScalingDamage = 0;
        public int fireScalingDamage = 0;
        public int lightningScalingDamage = 0;
        public int holyScalingDamage = 0;

        [Header("Weapon Modifier Damage")]
        public int physicalDamageModifier = 0;
        public int magicDamageModifier = 0;
        public int fireDamageModifier = 0;
        public int lightningDamageModifier = 0;
        public int holyDamageModifier = 0;

        [Header("Weapon Total Damage")]
        public int attackPower = 0;
        public int physicalDamage = 0;
        public List<CharacterAttribute> physicalDamageScaling = new List<CharacterAttribute> { CharacterAttribute.Strength, CharacterAttribute.Dexterity };
        public int magicDamage = 0;
        public List<CharacterAttribute> magicDamageScaling = new List<CharacterAttribute> { CharacterAttribute.Intelligence};
        public int fireDamage = 0;
        public List<CharacterAttribute> fireDamageScaling = new List<CharacterAttribute> { };
        public int lightningDamage = 0;
        public List<CharacterAttribute> lightningDamageScaling = new List<CharacterAttribute> { CharacterAttribute.Faith };
        public int holyDamage = 0;
        public List<CharacterAttribute> holyDamageScaling = new List<CharacterAttribute> { CharacterAttribute.Faith };
        public float CriticalModifier = 1.00f;

        [Header("Weapon Blocking Absorption")]
        public float physicalBaseDamageAbsorption = 50;
        public float magicBaseDamageAbsorption = 50;
        public float fireBaseDamageAbsorption = 50;
        public float lightningBaseDamageAbsorption = 50;
        public float holyBaseDamageAbsorption = 50;
        public float stability = 50;

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;

        [Header("Attack Modifiers")]
        public float light_Attack_01_Modifier = 1.0f;
        public PhysicalDamageType light_Attack_01_PhysicalDamageType;
        public float light_Attack_02_Modifier = 1.1f;
        public PhysicalDamageType light_Attack_02_PhysicalDamageType;
        public float heavy_Attack_01_Modifier = 1.4f;
        public PhysicalDamageType heavy_Attack_01_PhysicalDamageType;
        public float heavy_Attack_02_Modifier = 1.5f;
        public PhysicalDamageType heavy_Attack_02_PhysicalDamageType;
        public float charge_Attack_01_Modifier = 2.0f;
        public PhysicalDamageType charge_Attack_01_PhysicalDamageType;
        public float charge_Attack_02_Modifier = 2.1f;
        public PhysicalDamageType charge_Attack_02_PhysicalDamageType;
        public float running_Light_Attack_01_Modifier = 1.1f;
        public PhysicalDamageType running_Light_Attack_01_PhysicalDamageType;
        public float running_Heavy_Attack_01_Modifier = 1.5f;
        public PhysicalDamageType running_Heavy_Attack_01_PhysicalDamageType;
        public float rolling_Light_Attack_01_Modifier = 0.9f;
        public PhysicalDamageType rolling_Light_Attack_01_PhysicalDamageType;
        public float rolling_Heavy_Attack_01_Modifier = 1.3f;
        public PhysicalDamageType rolling_Heavy_Attack_01_PhysicalDamageType;
        public float backstep_Light_Attack_01_Modifier = 1.0f;
        public PhysicalDamageType backstep_Light_Attack_01_PhysicalDamageType;
        public float backstep_Heavy_Attack_01_Modifier = 1.4f;
        public PhysicalDamageType backstep_Heavy_Attack_01_PhysicalDamageType;
        public float jumping_Light_Attack_01_Modifier = 1.0f;
        public PhysicalDamageType jumping_Light_Attack_01_PhysicalDamageType;
        public float jumping_Heavy_Attack_01_Modifier = 1.5f;
        public PhysicalDamageType jumping_Heavy_Attack_01_PhysicalDamageType;

        [Header("Dual Attack Modifiers")]
        public float dual_Light_Attack_01_Modifier = 0.77f;
        public PhysicalDamageType dual_Light_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Light_Attack_Off_01_PhysicalDamageType;
        public float dual_Light_Attack_02_Modifier = 0.87f;
        public PhysicalDamageType dual_Light_Attack_Main_02_PhysicalDamageType;
        public PhysicalDamageType dual_Light_Attack_Off_02_PhysicalDamageType;
        public float dual_Heavy_Attack_01_Modifier = 0.94f;
        public PhysicalDamageType dual_Heavy_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Heavy_Attack_Off_01_PhysicalDamageType;
        public float dual_Heavy_Attack_02_Modifier = 1.0f;
        public PhysicalDamageType dual_Heavy_Attack_Main_02_PhysicalDamageType;
        public PhysicalDamageType dual_Heavy_Attack_Off_02_PhysicalDamageType;
        public float dual_Charge_Attack_01_Modifier = 1.3f;
        public PhysicalDamageType dual_Charge_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Charge_Attack_Off_01_PhysicalDamageType;
        public float dual_Charge_Attack_02_Modifier = 1.4f;
        public PhysicalDamageType dual_Charge_Attack_Main_02_PhysicalDamageType;
        public PhysicalDamageType dual_Charge_Attack_Off_02_PhysicalDamageType;
        public float dual_Running_Light_Attack_01_Modifier = 0.77f;
        public PhysicalDamageType dual_Running_Light_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Running_Light_Attack_Off_01_PhysicalDamageType;
        public float dual_Running_Heavy_Attack_01_Modifier = 0.94f;
        public PhysicalDamageType dual_Running_Heavy_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Running_Heavy_Attack_Off_01_PhysicalDamageType;
        public float dual_Rolling_Light_Attack_01_Modifier = 0.68f;
        public PhysicalDamageType dual_Rolling_Light_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Rolling_Light_Attack_Off_01_PhysicalDamageType;
        public float dual_Rolling_Heavy_Attack_01_Modifier = 0.81f;
        public PhysicalDamageType dual_Rolling_Heavy_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Rolling_Heavy_Attack_Off_01_PhysicalDamageType;
        public float dual_Backstep_Light_Attack_01_Modifier = 0.77f;
        public PhysicalDamageType dual_Backstep_Light_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Backstep_Light_Attack_Off_01_PhysicalDamageType;
        public float dual_Backstep_Heavy_Attack_01_Modifier = 0.9f;
        public PhysicalDamageType dual_Backstep_Heavy_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Backstep_Heavy_Attack_Off_01_PhysicalDamageType;
        public float dual_Jumping_Light_Attack_01_Modifier = 0.77f;
        public PhysicalDamageType dual_Jumping_Light_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Jumping_Light_Attack_Off_01_PhysicalDamageType;
        public float dual_Jumping_Heavy_Attack_01_Modifier = 0.98f;
        public PhysicalDamageType dual_Jumping_Heavy_Attack_Main_01_PhysicalDamageType;
        public PhysicalDamageType dual_Jumping_Heavy_Attack_Off_01_PhysicalDamageType;

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