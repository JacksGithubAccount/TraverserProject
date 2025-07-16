using UnityEngine;

namespace TraverserProject
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        [Header("FootSteps")]
        [SerializeField] protected AudioClip[] footSteps;


        protected virtual void Awake()
        {

            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.Singleton.rollSFX);
        }

        public virtual void PlayDamageGruntFX()
        {
            if (damageGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGruntFX()
        {
            if (attackGrunts.Length > 0)
                PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(attackGrunts));

        }

        public virtual void PlayFootStepSoundFX()
        {
            if (footSteps.Length > 0)
                PlaySoundFX(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(footSteps));
        }

        public virtual void PlayBlockSoundFX()
        {

        }
    }
}