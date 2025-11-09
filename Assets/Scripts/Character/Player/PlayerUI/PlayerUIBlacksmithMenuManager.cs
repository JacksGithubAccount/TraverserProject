using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerUIBlacksmithMenuManager : PlayerUIMenu
    {
        public void StartMenuDialogue(CharacterDialogue characterDialogue)
        {
            GameObject aiCharacterFinder = new GameObject();
            
            
            
            aiCharacterFinder.AddComponent<SphereCollider>();
            SphereCollider collider = aiCharacterFinder.GetComponent<SphereCollider>();
            collider.transform.position = PlayerUIManager.Singleton.localPlayer.transform.position;
            collider.radius = 2;
            collider.isTrigger = true;
            aiCharacterFinder.AddComponent<MenuDialogueCollider>();
            MenuDialogueCollider mdcollider = aiCharacterFinder.GetComponent<MenuDialogueCollider>();
            AICharacterManager aic = mdcollider.GetAICharacterManager();
            characterDialogue.PlayDialogueEvent(aic);
        }
    }
}
