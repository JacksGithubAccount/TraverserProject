using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUILevelUpManager : PlayerUIMenu
    {
        [Header("Levels")]
        [SerializeField] int[] playerLevels = new int[100];
        [SerializeField] int baseLevelCost = 83;
        [SerializeField] int totalLevelUpCost = 0;

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
        public Slider vigorSlider;
        public Slider mindSlider;
        public Slider enduranceSlider;
        public Slider strengthSlider;
        public Slider dexteritySlider;
        public Slider intelligenceSlider;
        public Slider faithSlider;
        public Slider luckSlider;

        [Header("Buttons")]
        [SerializeField] Button confirmLevelsButton;

        private void Awake()
        {
            SetAllLevelsCosts();
        }

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
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;


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
            //passes current and projected level to set up level up costs
            CalculateLevelCost(PlayerUIManager.Singleton.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(), PlayerUIManager.Singleton.localPlayer.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true));

            projectedCharacterLevelText.text = player.characterStatsManager.CalculateCharacterLevelBasedOnAttributes(true).ToString();
            bubblesCostText.text = totalLevelUpCost.ToString();

            if (totalLevelUpCost > player.playerStatsManager.bubbles)
            {
                confirmLevelsButton.interactable = false;
            }
            else
            {
                confirmLevelsButton.interactable = true;
            }
        }

        public void ConfirmLevels()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerStatsManager.bubbles -= totalLevelUpCost;

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

        private void SetAllLevelsCosts()
        {
            for (int i = 0; i < playerLevels.Length; i++)
            {
                if (i == 0)
                    continue;

                playerLevels[i] = baseLevelCost + (50 * i);
            }
        }

        private void CalculateLevelCost(int currentLevel, int projectedLevel)
        {
            int totalCost = 0;

            for (int i = 0; i < projectedLevel; i++)
            {
                if (i < currentLevel)
                    continue;

                totalCost += playerLevels[i];
            }
            totalLevelUpCost = totalCost;


            projectedBubblesHeldText.text = (PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles - totalCost).ToString();

            if (totalCost > PlayerUIManager.Singleton.localPlayer.playerStatsManager.bubbles)
            {
                projectedBubblesHeldText.color = Color.red;
            }
            else
            {
                projectedBubblesHeldText.color = Color.white;
            }
        }
    }
}