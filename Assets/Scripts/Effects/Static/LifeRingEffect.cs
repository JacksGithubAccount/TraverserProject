using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Life Ring Effect")]
    public class LifeRingEffect : StaticCharacterEffect
    {
        [SerializeField] int maxHealthGainedFromEffect;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if (character.IsOwner)
            {
                maxHealthGainedFromEffect = Mathf.RoundToInt(character.characterNetworkManager.maxHealth.Value / 10);
                character.characterNetworkManager.maxHealth.Value += maxHealthGainedFromEffect;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.maxHealth.Value -= maxHealthGainedFromEffect;
            }
        }
    }
}
