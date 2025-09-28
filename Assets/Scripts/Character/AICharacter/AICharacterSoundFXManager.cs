using UnityEngine;

namespace TraverserProject
{

    public class AICharacterSoundFXManager : CharacterSoundFXManager
    {
        [Header("Blocking SFX")]
        [SerializeField] AudioClip[] blockingSFX;

        public override void PlayBlockSoundFX()
        {
            if (blockingSFX.Length <= 0)
                return;

            PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(blockingSFX));
        }


    }
}