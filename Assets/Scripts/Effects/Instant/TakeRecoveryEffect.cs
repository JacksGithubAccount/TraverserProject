
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Recovery")]
    public class TakeRecoveryEffect : InstantCharacterEffect
    {
        [Header("Character Causing Recovery")]
        public CharacterManager characterCausingRecovery;

        [Header("Recovery")]
        public int recoveryAmount;
    }
}
