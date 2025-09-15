using TraverserProject;
using UnityEngine;

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
        [SerializeField] float staminaRegenerationAmount = 50;
        [SerializeField] float staminaRegenerationDelay = 2;

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

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.isSprinting.Value)
                return;

            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
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
    }

}