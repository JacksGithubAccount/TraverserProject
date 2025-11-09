using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class MenuDialogueCollider : MonoBehaviour
    {
        public AICharacterManager aiCharacter;
        Coroutine getAICharacterCoroutine;

        private void OnTriggerEnter(Collider other)
        {
            AICharacterManager aiCharacterTemp = other.GetComponent<AICharacterManager>();

            if (aiCharacterTemp == null)
                return;

            if(aiCharacterTemp.aiCharacterSoundFXManager.characterMenuDialogueID == CharacterMenuDialogueID.BlacksmithTalkDialogueID)
            {
                aiCharacter = aiCharacterTemp;
            }
        }

        public AICharacterManager GetAICharacterManager()
        {
            if(aiCharacter == null)
            {
                if (getAICharacterCoroutine != null)
                    StopCoroutine(getAICharacterCoroutine);

                getAICharacterCoroutine = StartCoroutine(GetAICharacterCoroutine());
            }
            return aiCharacter;
        }

        public IEnumerator GetAICharacterCoroutine()
        {
            if(aiCharacter == null)
            {
                yield return new WaitForSeconds(.01f);
            }
            yield return null;
        }

    }
}
