using UnityEngine;

namespace TraverserProject
{

    public class MessageInteractable : Interactable
    {
        [Header("Message")]
        [SerializeField] string messagePopUp;

        public override void Interact(PlayerManager player)
        {
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            WorldSaveGameManager.Singleton.SaveGame();

            PlayerUIManager.Singleton.playerUIPopUpManager.SendPlayerMessagePopUp(messagePopUp);
            //optionally play SFX here
        }

    }
}