using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {
        public override void ProcessEffect(CharacterManager character)
        {

            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            if (character.isDead.Value)
                return;

            CalculateDamage(character);

            character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;

        }

        protected override void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            if (characterCausingDamage != null)
            {

            }
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }


            character.characterNetworkManager.totalPoiseDamage.Value -= poiseDamage;

            //stores poise daamage taken for other interactions
            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

            float remainingPoise = character.characterNetworkManager.basePoiseDefense.Value + character.characterNetworkManager.offensivePoiseBonus.Value + character.characterNetworkManager.totalPoiseDamage.Value;

            if (remainingPoise <= 0)
                poiseIsBroken = true;

            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
        }

    }
}