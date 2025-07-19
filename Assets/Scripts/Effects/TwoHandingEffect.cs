using UnityEngine;


namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Two Handing Effect")]
    public class TwoHandingEffect : StaticCharacterEffect
    {
        [SerializeField] int strengthGainedFromTwohandingWeapon;

        public override void ProcessStaticEffect(CharacterManager character)
        {
            base.ProcessStaticEffect(character);

            if(character.IsOwner)
            {
                strengthGainedFromTwohandingWeapon = Mathf.RoundToInt(character.characterNetworkManager.strength.Value / 2);
                Debug.Log("Strength Gained: " + strengthGainedFromTwohandingWeapon);
                character.characterNetworkManager.strengthModifier.Value += strengthGainedFromTwohandingWeapon;
            }
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (character.IsOwner)
            {
                character.characterNetworkManager.strengthModifier.Value -= strengthGainedFromTwohandingWeapon;
            }
        }
    }
}
