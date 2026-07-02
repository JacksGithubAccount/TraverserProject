using NUnit.Framework;
using Steamworks.Ugc;
using System.Collections.Generic;
using TraverserProject;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TravserserProject
{

    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;

        [Header("Bubbles")]
        public int bubbles = 0;
        public int bubbleMemory = 0;


        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

        }

        protected override void Start()
        {
            base.Start();
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vigor.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value);
            CalculateAllWeaponAttackPower();
        }

        public void CalculateAllWeaponAttackPower()
        {
            foreach (var item in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                CalculateWeaponAttackPower(item);
            }
            foreach (var item in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                CalculateWeaponAttackPower(item);
            }
        }
        public override void CalculateWeaponAttackPower(WeaponItem weapon)
        {

            if (weapon == null)
                return;

            //upgrade power
            int upgradeLevel = (int)weapon.upgradeLevel;
            if (upgradeLevel == 0)
            {
                weapon.physicalUpgradeDamage = 0;
                weapon.magicUpgradeDamage = 0;
                weapon.fireUpgradeDamage = 0;
                weapon.lightningUpgradeDamage = 0;
                weapon.holyUpgradeDamage = 0;
            }
            for (int i = 0; i <= upgradeLevel; i++)
            {
                if (i >= 1)
                {
                    if (weapon.physicalBaseDamage > 0)
                        weapon.physicalUpgradeDamage += Mathf.RoundToInt((weapon.physicalBaseDamage * weapon.physicalUpgradeAmount) - weapon.physicalBaseDamage);
                    if (weapon.magicBaseDamage > 0)
                        weapon.magicUpgradeDamage += Mathf.RoundToInt((weapon.magicBaseDamage * weapon.magicUpgradeAmount) - weapon.magicBaseDamage);
                    if (weapon.fireBaseDamage > 0)
                        weapon.fireUpgradeDamage += Mathf.RoundToInt((weapon.fireBaseDamage * weapon.fireUpgradeAmount) - weapon.fireBaseDamage);
                    if (weapon.lightningBaseDamage > 0)
                        weapon.lightningUpgradeDamage += Mathf.RoundToInt((weapon.lightningBaseDamage * weapon.lightningUpgradeAmount) - weapon.lightningBaseDamage);
                    if (weapon.holyBaseDamage > 0)
                        weapon.holyUpgradeDamage += Mathf.RoundToInt((weapon.holyBaseDamage * weapon.holyUpgradeAmount) - weapon.holyBaseDamage);
                }
            }

            //scaling power
            weapon.physicalScalingDamage = CalculateDamageBasedOnScaling(weapon.physicalDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);
            weapon.magicScalingDamage = CalculateDamageBasedOnScaling(weapon.magicDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);
            weapon.fireScalingDamage = CalculateDamageBasedOnScaling(weapon.fireDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);
            weapon.lightningScalingDamage = CalculateDamageBasedOnScaling(weapon.lightningDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);
            weapon.holyScalingDamage = CalculateDamageBasedOnScaling(weapon.holyDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);



            int physicalPower = weapon.physicalBaseDamage;
            if (physicalPower > 0)
                physicalPower += weapon.physicalUpgradeDamage + weapon.physicalScalingDamage;
            weapon.physicalDamage = physicalPower;

            int magicDamage = weapon.magicBaseDamage;
            if (magicDamage > 0)
                magicDamage += weapon.magicUpgradeDamage + weapon.magicScalingDamage;
            weapon.magicDamage = magicDamage;

            int fireDamage = weapon.fireBaseDamage;
            if (fireDamage > 0)
                fireDamage += weapon.fireUpgradeDamage + weapon.fireScalingDamage;
            weapon.fireDamage = fireDamage;

            int lightningDamage = weapon.lightningBaseDamage;
            if (lightningDamage > 0)
                lightningDamage += weapon.lightningUpgradeDamage + weapon.lightningScalingDamage;
            weapon.lightningDamage = lightningDamage;

            int holyDamage = weapon.holyBaseDamage;
            if (holyDamage > 0)
                holyDamage += weapon.holyUpgradeDamage + weapon.holyScalingDamage;
            weapon.holyDamage = holyDamage;

            weapon.attackPower = weapon.physicalDamage + weapon.magicDamage + weapon.fireDamage + weapon.lightningDamage + weapon.holyDamage;

            if (weapon == player.playerInventoryManager.currentRightHandWeapon)
            {
                if (player.playerEquipmentManager.rightWeaponManager == null)
                    return;

                player.playerEquipmentManager.rightWeaponManager.SetWeaponDamage(player, weapon);
            }
            else if (weapon == player.playerInventoryManager.currentLeftHandWeapon)
            {
                if (player.playerEquipmentManager.leftWeaponManager == null)
                    return;

                player.playerEquipmentManager.leftWeaponManager.SetWeaponDamage(player, weapon);
            }
        }

        private int CalculateDamageBasedOnScaling(List<CharacterAttribute> scalings, int strScaling, int dexScaling, int intScaling, int faiScaling)
        {
            int scalingDamage = 0;

            foreach (var scaling in scalings)
            {
                switch (scaling)
                {
                    case CharacterAttribute.Vigor:
                        scalingDamage += player.playerNetworkManager.vigor.Value;
                        break;
                    case CharacterAttribute.Mind:
                        scalingDamage += player.playerNetworkManager.mind.Value;
                        break;
                    case CharacterAttribute.Endurance:
                        scalingDamage += player.playerNetworkManager.endurance.Value;
                        break;
                    case CharacterAttribute.Strength:
                        scalingDamage += ((strScaling / 100) * player.playerNetworkManager.strength.Value);
                        break;
                    case CharacterAttribute.Dexterity:
                        scalingDamage += ((dexScaling / 100) * player.playerNetworkManager.dexterity.Value);
                        break;
                    case CharacterAttribute.Intelligence:
                        scalingDamage += ((intScaling / 100) * player.playerNetworkManager.intelligence.Value);
                        break;
                    case CharacterAttribute.Faith:
                        scalingDamage += ((faiScaling / 100) * player.playerNetworkManager.faith.Value);
                        break;
                    case CharacterAttribute.Luck:
                        scalingDamage += player.playerNetworkManager.luck.Value;
                        break;
                    default:
                        break;
                }

            }

            return scalingDamage;
        }

        public override void CalculateTotalArmorAbsorption()
        {
            if (!player.IsOwner)
                return;

            player.playerNetworkManager.armorPhysicalDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorBluntDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorPierceDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorSlashDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorMagicDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorFireDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorLightningDamageAbsorption.Value = 0;
            player.playerNetworkManager.armorHolyDamageAbsorption.Value = 0;

            armorImmunity = 0;
            armorRobustness = 0;
            armorFocus = 0;
            armorVitality = 0;

            player.playerNetworkManager.basePoiseDefense.Value = 0;

            //head
            if (player.playerInventoryManager.headEquipment != null)
            {
                player.playerNetworkManager.armorPhysicalDamageAbsorption.Value += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
                player.playerNetworkManager.armorBluntDamageAbsorption.Value += player.playerInventoryManager.headEquipment.bluntDamageAbsorption;
                player.playerNetworkManager.armorPierceDamageAbsorption.Value += player.playerInventoryManager.headEquipment.pierceDamageAbsorption;
                player.playerNetworkManager.armorSlashDamageAbsorption.Value += player.playerInventoryManager.headEquipment.slashDamageAbsorption;
                player.playerNetworkManager.armorMagicDamageAbsorption.Value += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
                player.playerNetworkManager.armorFireDamageAbsorption.Value += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
                player.playerNetworkManager.armorLightningDamageAbsorption.Value += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
                player.playerNetworkManager.armorHolyDamageAbsorption.Value += player.playerInventoryManager.headEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.headEquipment.immunity;
                armorRobustness += player.playerInventoryManager.headEquipment.robustness;
                armorFocus += player.playerInventoryManager.headEquipment.focus;
                armorVitality += player.playerInventoryManager.headEquipment.vitality;

                player.playerNetworkManager.basePoiseDefense.Value += player.playerInventoryManager.headEquipment.poise;

            }
            //body
            if (player.playerInventoryManager.bodyEquipment != null)
            {
                player.playerNetworkManager.armorPhysicalDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
                player.playerNetworkManager.armorBluntDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.bluntDamageAbsorption;
                player.playerNetworkManager.armorPierceDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.pierceDamageAbsorption;
                player.playerNetworkManager.armorSlashDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.slashDamageAbsorption;
                player.playerNetworkManager.armorMagicDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
                player.playerNetworkManager.armorFireDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
                player.playerNetworkManager.armorLightningDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;
                player.playerNetworkManager.armorHolyDamageAbsorption.Value += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
                armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
                armorFocus += player.playerInventoryManager.bodyEquipment.focus;
                armorVitality += player.playerInventoryManager.bodyEquipment.vitality;

                player.playerNetworkManager.basePoiseDefense.Value += player.playerInventoryManager.bodyEquipment.poise;

            }
            //hand
            if (player.playerInventoryManager.handEquipment != null)
            {
                player.playerNetworkManager.armorPhysicalDamageAbsorption.Value += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
                player.playerNetworkManager.armorBluntDamageAbsorption.Value += player.playerInventoryManager.handEquipment.bluntDamageAbsorption;
                player.playerNetworkManager.armorPierceDamageAbsorption.Value += player.playerInventoryManager.handEquipment.pierceDamageAbsorption;
                player.playerNetworkManager.armorSlashDamageAbsorption.Value += player.playerInventoryManager.handEquipment.slashDamageAbsorption;
                player.playerNetworkManager.armorMagicDamageAbsorption.Value += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
                player.playerNetworkManager.armorFireDamageAbsorption.Value += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
                player.playerNetworkManager.armorLightningDamageAbsorption.Value += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;
                player.playerNetworkManager.armorHolyDamageAbsorption.Value += player.playerInventoryManager.handEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.handEquipment.immunity;
                armorRobustness += player.playerInventoryManager.handEquipment.robustness;
                armorFocus += player.playerInventoryManager.handEquipment.focus;
                armorVitality += player.playerInventoryManager.handEquipment.vitality;

                player.playerNetworkManager.basePoiseDefense.Value += player.playerInventoryManager.handEquipment.poise;

            }
            //leg
            if (player.playerInventoryManager.legEquipment != null)
            {
                player.playerNetworkManager.armorPhysicalDamageAbsorption.Value += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
                player.playerNetworkManager.armorBluntDamageAbsorption.Value += player.playerInventoryManager.legEquipment.bluntDamageAbsorption;
                player.playerNetworkManager.armorPierceDamageAbsorption.Value += player.playerInventoryManager.legEquipment.pierceDamageAbsorption;
                player.playerNetworkManager.armorSlashDamageAbsorption.Value += player.playerInventoryManager.legEquipment.slashDamageAbsorption;
                player.playerNetworkManager.armorMagicDamageAbsorption.Value += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
                player.playerNetworkManager.armorFireDamageAbsorption.Value += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
                player.playerNetworkManager.armorLightningDamageAbsorption.Value += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;
                player.playerNetworkManager.armorHolyDamageAbsorption.Value += player.playerInventoryManager.legEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.legEquipment.immunity;
                armorRobustness += player.playerInventoryManager.legEquipment.robustness;
                armorFocus += player.playerInventoryManager.legEquipment.focus;
                armorVitality += player.playerInventoryManager.legEquipment.vitality;

                player.playerNetworkManager.basePoiseDefense.Value += player.playerInventoryManager.legEquipment.poise;

            }

            //totals
            player.playerNetworkManager.armorPhysicalDamageAbsorption.Value += player.playerNetworkManager.armorPhysicalDamageAbsorption.Value * (player.playerNetworkManager.armorPhysicalDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorBluntDamageAbsorption.Value += player.playerNetworkManager.armorBluntDamageAbsorption.Value * (player.playerNetworkManager.armorBluntDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorPierceDamageAbsorption.Value += player.playerNetworkManager.armorPierceDamageAbsorption.Value * (player.playerNetworkManager.armorPierceDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorSlashDamageAbsorption.Value += player.playerNetworkManager.armorSlashDamageAbsorption.Value * (player.playerNetworkManager.armorSlashDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorMagicDamageAbsorption.Value += player.playerNetworkManager.armorMagicDamageAbsorption.Value * (player.playerNetworkManager.armorMagicDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorFireDamageAbsorption.Value += player.playerNetworkManager.armorFireDamageAbsorption.Value * (player.playerNetworkManager.armorFireDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorLightningDamageAbsorption.Value += player.playerNetworkManager.armorLightningDamageAbsorption.Value * (player.playerNetworkManager.armorLightningDamageAbsorptionModifer.Value / 100);
            player.playerNetworkManager.armorHolyDamageAbsorption.Value += player.playerNetworkManager.armorHolyDamageAbsorption.Value * (player.playerNetworkManager.armorHolyDamageAbsorptionModifer.Value / 100);

        }

        public override int CalculateBuildUpCapacityBasedOnVigorLevelAndEquipment(int vigor)
        {
            float capacity = 0;

            //any equation for capacity
            capacity = vigor * 15 + player.playerStatsManager.armorImmunity;

            return Mathf.RoundToInt(capacity);
        }

        public override int CalculateBuildUpCapacityBasedOnMindLevelAndEquipment(int mind)
        {
            float capacity = 0;

            //any equation for capacity
            capacity = mind * 15 + player.playerStatsManager.armorFocus;

            return Mathf.RoundToInt(capacity);
        }

        public override int CalculateBuildUpCapacityBasedOnEnduranceLevelAndEquipment(int endurance)
        {
            float capacity = 0;

            //any equation for capacity
            capacity = endurance * 15 + player.playerStatsManager.armorRobustness;

            return Mathf.RoundToInt(capacity);
        }

        public void AddBubbles(int bubblesToAdd)
        {
            bubbles += bubblesToAdd;
            bubbleMemory += bubblesToAdd;

            PlayerUIManager.Singleton.playerUIHudManager.SetBubblesCount(bubblesToAdd);
        }


    }

}