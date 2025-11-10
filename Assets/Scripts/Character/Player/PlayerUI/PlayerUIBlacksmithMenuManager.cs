using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerUIBlacksmithMenuManager : PlayerUIMenu
    {
        public void StartMenuDialogue()
        {       
            GameObject blacksmithFinder = GameObject.Find("NPC_BlacksmithDummy 03(Clone)");
            AICharacterManager aiBlacksmith = blacksmithFinder.GetComponent<AICharacterManager>();
            aiBlacksmith.aiCharacterSoundFXManager.PlayCurrentMenuDialogueEvent();
        }
    }
}
