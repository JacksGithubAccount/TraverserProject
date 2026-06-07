using TMPro;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerUIStatusMenuManager : PlayerUIMenu
    {
        [Header("First Part")]
        [SerializeField] TextMeshProUGUI characterNameText;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] TextMeshProUGUI bubblesHeldText;
        [SerializeField] TextMeshProUGUI bubbledNeededText;

        [Header("Attributes")]
        [SerializeField] TextMeshProUGUI vigorText;
        [SerializeField] TextMeshProUGUI mindText;
        [SerializeField] TextMeshProUGUI enduranceText;
        [SerializeField] TextMeshProUGUI strengthText;
        [SerializeField] TextMeshProUGUI dexterityText;
        [SerializeField] TextMeshProUGUI intelligenceText;
        [SerializeField] TextMeshProUGUI faithText;
        [SerializeField] TextMeshProUGUI luckText;

        [Header("Base Stats")]
        [SerializeField] TextMeshProUGUI healthText;
        [SerializeField] TextMeshProUGUI focusPointsText;
        [SerializeField] TextMeshProUGUI staminaText;
        [SerializeField] TextMeshProUGUI equipLoadText;
        [SerializeField] TextMeshProUGUI equipLoadTypeText;
        [SerializeField] TextMeshProUGUI poiseText;
        [SerializeField] TextMeshProUGUI itemDiscoveryText;
        [SerializeField] TextMeshProUGUI spellSlotsText;

        [Header("Attack Power")]
        [SerializeField] TextMeshProUGUI rWeapon1Text;
        [SerializeField] TextMeshProUGUI rWeapon2Text;
        [SerializeField] TextMeshProUGUI rWeapon3Text;
        [SerializeField] TextMeshProUGUI lWeapon1Text;
        [SerializeField] TextMeshProUGUI lWeapon2Text;
        [SerializeField] TextMeshProUGUI lWeapon3Text;

        [Header("Defense Power")]
        [SerializeField] TextMeshProUGUI physicalText;
        [SerializeField] TextMeshProUGUI bluntText;
        [SerializeField] TextMeshProUGUI pierceText;
        [SerializeField] TextMeshProUGUI slashText;
        [SerializeField] TextMeshProUGUI magicText;
        [SerializeField] TextMeshProUGUI fireText;
        [SerializeField] TextMeshProUGUI lightningText;
        [SerializeField] TextMeshProUGUI holyText;

        [Header("Resistances")]
        [SerializeField] TextMeshProUGUI immunityText;
        [SerializeField] TextMeshProUGUI robustnessText;
        [SerializeField] TextMeshProUGUI focusText;
        [SerializeField] TextMeshProUGUI vitalityText;


        public override void OpenMenu()
        {
            base.OpenMenu();

            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            LoadStatusInformation();
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
        }

        private void LoadStatusInformation()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            characterNameText.text = player.playerNetworkManager.characterName.Value.ToString();
            levelText.text = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString(); ;
            bubblesHeldText.text = player.playerStatsManager.bubbles.ToString();
            bubbledNeededText.text = PlayerUIManager.Singleton.playerUILevelUpManager.CalculateLevelCost(player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(), (player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes() + 1)).ToString();

            vigorText.text = player.playerNetworkManager.vigor.Value.ToString();
            mindText.text = player.playerNetworkManager.mind.Value.ToString();
            enduranceText.text = player.playerNetworkManager.endurance.Value.ToString();
            strengthText.text = player.playerNetworkManager.strength.Value.ToString();
            dexterityText.text = player.playerNetworkManager.dexterity.Value.ToString();
            intelligenceText.text = player.playerNetworkManager.intelligence.Value.ToString();
            faithText.text = player.playerNetworkManager.faith.Value.ToString();
            luckText.text = player.playerNetworkManager.luck.Value.ToString();

            healthText.text = player.playerNetworkManager.currentHealth.Value.ToString() + "/" + player.playerNetworkManager.maxHealth.Value.ToString();
            focusPointsText.text = player.playerNetworkManager.currentFocusPoints.Value.ToString() + "/" + player.playerNetworkManager.maxFocusPoints.Value.ToString();
            staminaText.text = player.playerNetworkManager.currentStamina.Value.ToString() + "/" + player.playerNetworkManager.maxStamina.Value.ToString();
            equipLoadText.text = "Placeholder";
            equipLoadTypeText.text = "Placeholder";
            poiseText.text = player.playerStatsManager.basePoiseDefense.ToString();
            itemDiscoveryText.text = "Placeholder";
            spellSlotsText.text = "Placeholder";

            rWeapon1Text.text = player.playerInventoryManager.weaponsInRightHandSlots[0].attackPower.ToString();
            rWeapon2Text.text = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider.physicalDamage.ToString();
            //rWeapon2Text.text = player.playerInventoryManager.weaponsInRightHandSlots[1].attackPower.ToString();
            rWeapon3Text.text = player.playerInventoryManager.weaponsInRightHandSlots[2].attackPower.ToString();
            lWeapon1Text.text = player.playerInventoryManager.weaponsInLeftHandSlots[0].attackPower.ToString();
            lWeapon2Text.text = player.playerInventoryManager.weaponsInLeftHandSlots[1].attackPower.ToString();
            lWeapon3Text.text = player.playerInventoryManager.weaponsInLeftHandSlots[2].attackPower.ToString();

            physicalText.text = player.playerStatsManager.armorPhysicalDamageAbsorption.ToString();
            bluntText.text = player.playerStatsManager.armorBluntDamageAbsorption.ToString();
            pierceText.text = player.playerStatsManager.armorPierceDamageAbsorption.ToString();
            slashText.text = player.playerStatsManager.armorSlashDamageAbsorption.ToString();
            magicText.text = player.playerStatsManager.armorMagicDamageAbsorption.ToString();
            fireText.text = player.playerStatsManager.armorFireDamageAbsorption.ToString();
            lightningText.text = player.playerStatsManager.armorLightningDamageAbsorption.ToString();
            holyText.text = player.playerStatsManager.armorHolyDamageAbsorption.ToString();

            immunityText.text = player.playerStatsManager.armorImmunity.ToString();
            robustnessText.text = player.playerStatsManager.armorRobustness.ToString();
            focusText.text = player.playerStatsManager.armorFocus.ToString();
            vitalityText.text = player.playerStatsManager.armorVitality.ToString();
        }

    }
}