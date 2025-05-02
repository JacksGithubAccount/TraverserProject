using TraverserProject;
using UnityEngine;

namespace TravserserProject
{

    public class CharacterStatsManager : MonoBehaviour
    {

        CharacterManager character;
        [Header("Stamina Regeneration")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationAmount = 2;
        [SerializeField] float staminaRegenerationDelay = 2;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            //any equation for stamina
            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
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
    }

}
