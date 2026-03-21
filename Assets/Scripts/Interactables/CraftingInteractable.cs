using UnityEngine;

namespace TraverserProject
{
    public class CraftingInteractable : Interactable
    {
        public override void Interact(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            WorldSaveGameManager.Singleton.SaveGame();

            PlayerUIManager.Singleton.playerUICraftingManager.OpenMenu();
        }

        public override void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (!player.playerNetworkManager.IsHost && hostOnlyInteractable)
                    return;

                if (!player.IsOwner)
                    return;

                player.playerInteractionManager.RemoveInteractionFromList(this);
                PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();
                PlayerUIManager.Singleton.playerUICraftingManager.CloseMenu();
            }
        }
    }
}
