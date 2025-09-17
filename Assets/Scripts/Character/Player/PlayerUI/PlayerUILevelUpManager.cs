using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        [Header("Character Stats")]
        [SerializeField] TextMeshProUGUI characterLevelText;
        [SerializeField] TextMeshProUGUI bubblesHeldText;
        [SerializeField] TextMeshProUGUI bubblesCostText;
        [SerializeField] TextMeshProUGUI vigorLevelText;
        [SerializeField] TextMeshProUGUI mindLevelText;
        [SerializeField] TextMeshProUGUI enduranceLevelText;
        [SerializeField] TextMeshProUGUI strengthLevelText;
        [SerializeField] TextMeshProUGUI dexterityLevelText;
        [SerializeField] TextMeshProUGUI intelligenceLevelText;
        [SerializeField] TextMeshProUGUI faithLevelText;
        [SerializeField] TextMeshProUGUI luckLevelText;

        [Header("Projected Character Stats")]
        [SerializeField] TextMeshProUGUI projectedCharacterLevelText;
        [SerializeField] TextMeshProUGUI projectedBubblesHeldText;
        [SerializeField] TextMeshProUGUI projectedVigorLevelText;
        [SerializeField] TextMeshProUGUI projectedMindLevelText;
        [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
        [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
        [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
        [SerializeField] TextMeshProUGUI projectedIntelligenceLevelText;
        [SerializeField] TextMeshProUGUI projectedFaithLevelText;
        [SerializeField] TextMeshProUGUI projectedLuckLevelText;

        [Header("Sliders")]
        public CharacterAttribute currentSelectedAttribute;
        [SerializeField] Slider vigorSlider;
        [SerializeField] Slider mindSlider;
        [SerializeField] Slider enduranceSlider;
        [SerializeField] Slider strengthSlider;
        [SerializeField] Slider dexteritySlider;
        [SerializeField] Slider intelligenceSlider;
        [SerializeField] Slider faithSlider;
        [SerializeField] Slider luckSlider;

        public override void OpenMenu()
        {
            base.OpenMenu();

            SetCurrentStats();
        }

        private void SetCurrentStats()
        {
            characterLevelText.text = PlayerUIManager.Singleton.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();
            projectedCharacterLevelText.text = PlayerUIManager.Singleton.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();

            bubblesHeldText.text = PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles.ToString();
            projectedBubblesHeldText.text = PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles.ToString();
            bubblesCostText.text = "0";

            vigorLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.vigor.Value.ToString();
            projectedVigorLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.vigor.Value.ToString();
            vigorSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.vigor.Value;

            mindLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.mind.Value.ToString();
            projectedMindLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.mind.Value.ToString();
            mindSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.mind.Value;

            enduranceLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.endurance.Value.ToString();
            projectedEnduranceLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.endurance.Value.ToString();
            enduranceSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.endurance.Value;

            strengthLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.strength.Value.ToString();
            projectedStrengthLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.strength.Value.ToString();
            strengthSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.strength.Value;

            dexterityLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            projectedDexterityLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.dexterity.Value.ToString();
            dexteritySlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.dexterity.Value;

            intelligenceLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            projectedIntelligenceLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.intelligence.Value.ToString();
            intelligenceSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.intelligence.Value;

            faithLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.faith.Value.ToString();
            projectedFaithLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.faith.Value.ToString();
            faithSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.faith.Value;

            luckLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.luck.Value.ToString();
            projectedLuckLevelText.text = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.luck.Value.ToString();
            luckSlider.minValue = PlayerUIManager.Singleton.localPlayer.playerNetworkManager.luck.Value;

            vigorSlider.Select();
            vigorSlider.OnSelect(null);
        }

        public void UpdateSliderBasedOnCurrentlySelectedAttribute()
        {
            switch (currentSelectedAttribute)
            {
                case CharacterAttribute.Vigor:
                    projectedVigorLevelText.text = vigorSlider.value.ToString();
                    break;
                case CharacterAttribute.Mind:
                    projectedMindLevelText.text = mindSlider.value.ToString();
                    break;
                case CharacterAttribute.Endurance:
                    projectedEnduranceLevelText.text = enduranceSlider.value.ToString();
                    break;
                case CharacterAttribute.Strength:
                    projectedStrengthLevelText.text = strengthSlider.value.ToString();
                    break;
                case CharacterAttribute.Dexterity:
                    projectedDexterityLevelText.text = dexteritySlider.value.ToString();
                    break;
                case CharacterAttribute.Intelligence:
                    projectedIntelligenceLevelText.text = intelligenceSlider.value.ToString();
                    break;
                case CharacterAttribute.Faith:
                    projectedFaithLevelText.text = faithSlider.value.ToString();
                    break;
                case CharacterAttribute.Luck:
                    projectedLuckLevelText.text = luckSlider.value.ToString();
                    break;
                default:
                    break;
            }
        }

        public void ConfirmLevels()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.vigor.Value = Mathf.RoundToInt(vigorSlider.value);
            player.playerNetworkManager.mind.Value = Mathf.RoundToInt(mindSlider.value);
            player.playerNetworkManager.endurance.Value = Mathf.RoundToInt(enduranceSlider.value);
            player.playerNetworkManager.strength.Value = Mathf.RoundToInt(strengthSlider.value);
            player.playerNetworkManager.dexterity.Value = Mathf.RoundToInt(dexteritySlider.value);
            player.playerNetworkManager.intelligence.Value = Mathf.RoundToInt(intelligenceSlider.value);
            player.playerNetworkManager.faith.Value = Mathf.RoundToInt(faithSlider.value);
            player.playerNetworkManager.luck.Value = Mathf.RoundToInt(luckSlider.value);

            SetCurrentStats();
        }
    }
}