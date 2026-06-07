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
            int upgradePower = 0;
            for (int i = 0; i <= upgradeLevel; i++)
            {
                if (i >= 1)
                    upgradePower += 11;
            }

            //scaling power
            int physicalScalingPower = CalculateDamageBasedOnScaling(weapon.physicalBaseDamage, weapon.physicalDamageScaling, weapon.strengthScaling, weapon.dexterityScaling, weapon.intelligenceScaling, weapon.faithScaling);
            //item.magicDamage = CalculateDamageBasedOnScaling(item.magicDamage, item.physicalDamageScaling, item.strengthScaling, item.dexterityScaling, item.intelligenceScaling, item.faithScaling);

            weapon.attackPower = weapon.physicalDamage + weapon.magicDamage + weapon.fireDamage + weapon.lightningDamage + weapon.holyDamage;
        }

        private int CalculateDamageBasedOnScaling(int damage, List<CharacterAttribute> scalings, int strScaling, int dexScaling, int intScaling, int faiScaling)
        {
            int totalDamage = damage;

            foreach (var scaling in scalings)
            {
                switch (scaling)
                {
                    case CharacterAttribute.Vigor:
                        totalDamage += player.playerNetworkManager.vigor.Value;
                        break;
                    case CharacterAttribute.Mind:
                        totalDamage += player.playerNetworkManager.mind.Value;
                        break;
                    case CharacterAttribute.Endurance:
                        totalDamage += player.playerNetworkManager.endurance.Value;
                        break;
                    case CharacterAttribute.Strength:
                        totalDamage += ((strScaling / 100) * player.playerNetworkManager.strength.Value);
                        break;
                    case CharacterAttribute.Dexterity:
                        totalDamage += ((dexScaling / 100) * player.playerNetworkManager.dexterity.Value);
                        break;
                    case CharacterAttribute.Intelligence:
                        totalDamage += ((intScaling / 100) * player.playerNetworkManager.intelligence.Value);
                        break;
                    case CharacterAttribute.Faith:
                        totalDamage += ((faiScaling / 100) * player.playerNetworkManager.faith.Value);
                        break;
                    case CharacterAttribute.Luck:
                        totalDamage += player.playerNetworkManager.luck.Value;
                        break;
                    default:
                        break;
                }

            }

            return totalDamage;
        }

        public override void CalculateTotalArmorAbsorption()
        {
            armorPhysicalDamageAbsorption = 0;
            armorBluntDamageAbsorption = 0;
            armorPierceDamageAbsorption = 0;
            armorSlashDamageAbsorption = 0;
            armorMagicDamageAbsorption = 0;
            armorFireDamageAbsorption = 0;
            armorLightningDamageAbsorption = 0;
            armorHolyDamageAbsorption = 0;

            armorImmunity = 0;
            armorRobustness = 0;
            armorFocus = 0;
            armorVitality = 0;

            basePoiseDefense = 0;

            //head
            if (player.playerInventoryManager.headEquipment != null)
            {
                armorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
                armorBluntDamageAbsorption += player.playerInventoryManager.headEquipment.bluntDamageAbsorption;
                armorPierceDamageAbsorption += player.playerInventoryManager.headEquipment.pierceDamageAbsorption;
                armorSlashDamageAbsorption += player.playerInventoryManager.headEquipment.slashDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.headEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.headEquipment.immunity;
                armorRobustness += player.playerInventoryManager.headEquipment.robustness;
                armorFocus += player.playerInventoryManager.headEquipment.focus;
                armorVitality += player.playerInventoryManager.headEquipment.vitality;

                basePoiseDefense += player.playerInventoryManager.headEquipment.poise;

            }
            //body
            if (player.playerInventoryManager.bodyEquipment != null)
            {
                armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
                armorBluntDamageAbsorption += player.playerInventoryManager.bodyEquipment.bluntDamageAbsorption;
                armorPierceDamageAbsorption += player.playerInventoryManager.bodyEquipment.pierceDamageAbsorption;
                armorSlashDamageAbsorption += player.playerInventoryManager.bodyEquipment.slashDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.bodyEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.bodyEquipment.immunity;
                armorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
                armorFocus += player.playerInventoryManager.bodyEquipment.focus;
                armorVitality += player.playerInventoryManager.bodyEquipment.vitality;

                basePoiseDefense += player.playerInventoryManager.bodyEquipment.poise;

            }
            //hand
            if (player.playerInventoryManager.handEquipment != null)
            {
                armorPhysicalDamageAbsorption += player.playerInventoryManager.handEquipment.physicalDamageAbsorption;
                armorBluntDamageAbsorption += player.playerInventoryManager.handEquipment.bluntDamageAbsorption;
                armorPierceDamageAbsorption += player.playerInventoryManager.handEquipment.pierceDamageAbsorption;
                armorSlashDamageAbsorption += player.playerInventoryManager.handEquipment.slashDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.handEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.handEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.handEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.handEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.handEquipment.immunity;
                armorRobustness += player.playerInventoryManager.handEquipment.robustness;
                armorFocus += player.playerInventoryManager.handEquipment.focus;
                armorVitality += player.playerInventoryManager.handEquipment.vitality;

                basePoiseDefense += player.playerInventoryManager.handEquipment.poise;

            }
            //leg
            if (player.playerInventoryManager.legEquipment != null)
            {
                armorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
                armorBluntDamageAbsorption += player.playerInventoryManager.legEquipment.bluntDamageAbsorption;
                armorPierceDamageAbsorption += player.playerInventoryManager.legEquipment.pierceDamageAbsorption;
                armorSlashDamageAbsorption += player.playerInventoryManager.legEquipment.slashDamageAbsorption;
                armorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
                armorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
                armorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;
                armorHolyDamageAbsorption += player.playerInventoryManager.legEquipment.holyDamageAbsorption;

                armorImmunity += player.playerInventoryManager.legEquipment.immunity;
                armorRobustness += player.playerInventoryManager.legEquipment.robustness;
                armorFocus += player.playerInventoryManager.legEquipment.focus;
                armorVitality += player.playerInventoryManager.legEquipment.vitality;

                basePoiseDefense += player.playerInventoryManager.legEquipment.poise;

            }

            //totals
            armorPhysicalDamageAbsorption += armorPhysicalDamageAbsorption * (player.playerNetworkManager.armorPhysicalDamageAbsorptionModifer.Value / 100);
            armorBluntDamageAbsorption += armorBluntDamageAbsorption * (player.playerNetworkManager.armorBluntDamageAbsorptionModifer.Value / 100);
            armorPierceDamageAbsorption += armorPierceDamageAbsorption * (player.playerNetworkManager.armorPierceDamageAbsorptionModifer.Value / 100);
            armorSlashDamageAbsorption += armorSlashDamageAbsorption * (player.playerNetworkManager.armorSlashDamageAbsorptionModifer.Value / 100);
            armorMagicDamageAbsorption += armorMagicDamageAbsorption * (player.playerNetworkManager.armorMagicDamageAbsorptionModifer.Value / 100);
            armorFireDamageAbsorption += armorFireDamageAbsorption * (player.playerNetworkManager.armorFireDamageAbsorptionModifer.Value / 100);
            armorLightningDamageAbsorption += armorLightningDamageAbsorption * (player.playerNetworkManager.armorLightningDamageAbsorptionModifer.Value / 100);
            armorHolyDamageAbsorption += armorHolyDamageAbsorption * (player.playerNetworkManager.armorHolyDamageAbsorptionModifer.Value / 100);

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