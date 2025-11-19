
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Recovery Effect")]
    public class TakeRecoveryEffect : InstantCharacterEffect
    {
        [Header("Character Causing Recovery")]
        public CharacterManager characterCausingRecovery;

        [Header("Recovery")]
        public int recoveryAmount;

        [Header("Final Recovery Amount")]
        protected int finalRecoveryAmount = 0;

        [Header("Sound FX")]
        public bool willPlayRecoverySFX = true;
        public AudioClip recoverySoundFX;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);

            PlayRecoverySFX(character);
            PlayRecoveryVFX(character);

        }

        protected virtual void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            if (characterCausingRecovery != null)
            {

            }
            finalRecoveryAmount = Mathf.RoundToInt(recoveryAmount);

            if (finalRecoveryAmount <= 0)
            {
                finalRecoveryAmount = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalRecoveryAmount;
        }

        protected void PlayRecoveryVFX(CharacterManager character)
        {
            //character.characterEffectsManager.PlayBloodSplatterVFX();
        }

        protected void PlayRecoverySFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntFX();
        }
    }
}
