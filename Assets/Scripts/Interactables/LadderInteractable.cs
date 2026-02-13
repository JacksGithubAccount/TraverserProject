using UnityEngine;

namespace TraverserProject
{
    public class LadderInteractable : Interactable
    {
        public override void Interact(PlayerManager player)
        {
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Climb_Up_01", true); 
        }
    }
}
