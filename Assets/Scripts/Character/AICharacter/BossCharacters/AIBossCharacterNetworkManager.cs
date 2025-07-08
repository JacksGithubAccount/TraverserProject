using UnityEngine;

namespace TraverserProject
{

    public class AIBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager aiBossCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIBossCharacterManager>();
        }


        public override void CheckHealth(int oldValue, int newValue)
        {
            base.CheckHealth(oldValue, newValue);

            if (aiBossCharacter.IsOwner)
            {

                if (currentHealth.Value <= 0)
                    return;

                float healthNeededForShift = maxHealth.Value * (aiBossCharacter.minimumHealthPercentageToShift / 100);
                if (currentHealth.Value <= healthNeededForShift)
                {
                    aiBossCharacter.PhaseShift();
                }
            }
        }

    }
}