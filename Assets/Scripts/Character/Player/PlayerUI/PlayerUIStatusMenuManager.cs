using TMPro;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerUIStatusMenuManager : PlayerUIMenu
    {
        [Header("Status")]
        [SerializeField] GameObject statusWindow;

        [Header("Panel 1")]
        [SerializeField] GameObject panel1Window;

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

    }
}