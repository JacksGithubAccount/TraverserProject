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

            player.playerAnimatorManager.PlayTargetActionAnimation("Ladder_Start_Climbing_From_Bottom_01", true); 
        }
    }
}
