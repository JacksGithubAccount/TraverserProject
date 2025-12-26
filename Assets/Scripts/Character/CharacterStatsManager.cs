using TraverserProject;
using UnityEngine;
using System;

namespace TravserserProject
{

    public class CharacterStatsManager : MonoBehaviour
    {

        CharacterManager character;

        [Header("Runes")]
        public int runesDroppedOnDeath = 50;

        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float baseStaminaRegenerationAmount = 50;
        private float staminaRegenerationAmount = 0;
        [SerializeField] float staminaRegenerationDelay = 2;
        [SerializeField] float blockingStaminaRegenerationReduction = 0.2f;

        [Header("Blocking Absorptions")]
        public float blockingPhysicalAbsorption;
        public float blockingFireAbsorption;
        public float blockingMagicAbsorption;
        public float blockingLightningAbsorption;
        public float blockingHolyAbsorption;
        public float blockingStability;

        [Header("Armor Absorptions")]
        public float armorPhysicalDamageAbsorption;
        public float armorMagicDamageAbsorption;
        public float armorFireDamageAbsorption;
        public float armorLightningDamageAbsorption;
        public float armorHolyDamageAbsorption;

        [Header("Armor Resistances")]
        public float armorImmunity;   //rot and poison
        public float armorRobustness; //bleed and frost
        public float armorFocus;      //sleep and madness
        public float armorVitality;   //deathblight

        [Header("Poise")]
        public float totalPoiseDamage;
        public float offensivePoiseBonus;
        public float basePoiseDefense;
        public float defaultPoiseResetTime = 8;
        public float poiseResetTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            float health = 0;

            //any equation for health
            health = vitality * 30;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            //any equation for stamina
            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public int CalculateFocusPointsBasedOnMindLevel(int mind)
        {
            float focusPoints = 0;

            //any equation for stamina
            focusPoints = mind * 10;

            return Mathf.RoundToInt(focusPoints);
        }

        public int CalculateCharacterLevelBasedOnAttributes(bool calculateProjectedLevel = false)
        {
            if (calculateProjectedLevel)
            {
                int totalProjectedAttributes = Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.vigorSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.mindSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.enduranceSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.strengthSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.dexteritySlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.intelligenceSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.faithSlider.value) +
                    Mathf.RoundToInt(PlayerUIManager.Singleton.playerUILevelUpManager.luckSlider.value);

                //int projectedCharacterLevel = totalProjectedAttributes - 80 + 1;
                int projectedCharacterLevel = totalProjectedAttributes - (Enum.GetNames(typeof(CharacterAttribute)).Length * 10) + 1;

                if (projectedCharacterLevel < 1)
                    projectedCharacterLevel = 1;

                return projectedCharacterLevel;
            }
            else
            {
                int totalAttributes = character.characterNetworkManager.vigor.Value +
                character.characterNetworkManager.mind.Value +
                character.characterNetworkManager.endurance.Value +
                character.characterNetworkManager.strength.Value +
                character.characterNetworkManager.dexterity.Value +
                character.characterNetworkManager.intelligence.Value +
                character.characterNetworkManager.faith.Value +
                character.characterNetworkManager.luck.Value;

                //int characterLevel = totalAttributes - 80 + 1;
                int characterLevel = totalAttributes - (Enum.GetNames(typeof(CharacterAttribute)).Length * 10) + 1;

                if (characterLevel < 1)
                    characterLevel = 1;

                return characterLevel;
            }
        }

        public int CalculateBuildUpCapacityBasedOnVigorLevel(int vigor)
        {
            float capacity = 0;

            //any equation for capacity
            capacity = vigor * 15;

            return Mathf.RoundToInt(capacity);
        }

        public int CalculateBuildUpCapacityBasedOnMindLevel(int mind)
        {
            float capacity = 0;

            //any equation for capacity
            capacity = mind * 15;

            return Mathf.RoundToInt(capacity);
        }

        public int CalculateBuildUpCapacityBasedOnEnduranceLevel(int endurance)
        {
            float capacity = 0;

            //any equation for capacity
            capacity =  endurance * 15;

            return Mathf.RoundToInt(capacity);
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.isSprinting.Value)
                return;

            if (character.isPerformingAction)
                return;

            if (character.characterNetworkManager.currentStamina.Value >= character.characterNetworkManager.maxStamina.Value)
                return;

            staminaRegenerationAmount = baseStaminaRegenerationAmount + (baseStaminaRegenerationAmount * (character.characterNetworkManager.staminaRegenerationModifier.Value / 100));

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {

                staminaTickTimer += Time.deltaTime;

                Debug.Log("Stamina Regeneration Amount: " + staminaRegenerationAmount);

                if (character.characterNetworkManager.isBlocking.Value)
                    staminaRegenerationAmount *= blockingStaminaRegenerationReduction;

                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }

            }

        }
        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            //resets regen timer if action used stamina, not already regen stamina
            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }

        }


        protected virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoiseDamage = 0;
            }
        }

        public virtual void DegradeBuildUps(BuildUp buildUp, int amount, BuildUpEffect effect)
        {
            switch (buildUp)
            {
                case BuildUp.Poison:
                    character.characterNetworkManager.poisonBuildUp.Value += amount;
                    effect.buildUpRemaining = character.characterNetworkManager.poisonBuildUp.Value;
                    break;
                case BuildUp.Bleed:
                    character.characterNetworkManager.bleedBuildUp.Value += amount;
                    effect.buildUpRemaining = character.characterNetworkManager.bleedBuildUp.Value;
                    break;
                case BuildUp.Frost:
                    character.characterNetworkManager.frostBuildUp.Value += amount;
                    effect.buildUpRemaining = character.characterNetworkManager.frostBuildUp.Value;
                    break;
                default:
                    break;
            }
        }
    }

}