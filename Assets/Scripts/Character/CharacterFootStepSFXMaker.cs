using UnityEngine;

namespace TraverserProject
{

    public class CharacterFootStepSFXMaker : MonoBehaviour
    {
        CharacterManager character;

        AudioSource audioSource;
        GameObject steppedOnObject;

        private bool hasTouchedGround = false;
        private bool hasPlayedFootStepSFX = false;
        [SerializeField] float distanceToGround = 0.05f;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            character = GetComponentInParent<CharacterManager>();
        }

        private void FixedUpdate()
        {
            CheckForFootSteps();
        }

        private void CheckForFootSteps()
        {
            if (character == null)
                return;

            if (!character.characterNetworkManager.isMoving.Value)
                return;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.Singleton.GetEnviroLayers()))
            {
                hasTouchedGround = true;

                if (!hasPlayedFootStepSFX)
                    steppedOnObject = hit.transform.gameObject;

            }
            else
            {
                hasTouchedGround = false;
                hasPlayedFootStepSFX = false;
                steppedOnObject = null;
            }

            if (hasTouchedGround && !hasPlayedFootStepSFX)
            {
                hasPlayedFootStepSFX = true;
                PlayFootStepSoundFX();
            }




        }

        private void PlayFootStepSoundFX()
        {
            if (character.characterNetworkManager.isSneaking.Value)
                return;

            character.characterSoundFXManager.PlayFootStepSoundFX();
        }

    }
}