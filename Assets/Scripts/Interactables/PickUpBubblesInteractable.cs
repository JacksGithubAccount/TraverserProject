using UnityEngine;

namespace TraverserProject
{

    public class PickUpBubblesInteractable : Interactable
    {
        public int bubbleCount = 0;

        public override void Interact(PlayerManager player)
        {
            WorldSaveGameManager.Singleton.currentCharacterData.hasDeadSpot = false;
            player.playerStatsManager.AddBubbles(bubbleCount);
            Destroy(gameObject);
        }

    }
}