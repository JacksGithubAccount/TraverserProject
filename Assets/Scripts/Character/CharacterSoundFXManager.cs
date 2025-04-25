using UnityEngine;

namespace TraverserProject
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {

            audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX()
        {
            audioSource.Play(WorldSoundFXManager.Singleton.rollSFX);
        }
    }
}