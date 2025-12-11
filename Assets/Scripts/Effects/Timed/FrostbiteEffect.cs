using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Frostbite Effect")]
    public class FrostbiteEffect : TimedCharacterEffect
    {
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);
        }

    }
}