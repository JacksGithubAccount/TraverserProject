using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Messaging Item")]
    public class MessagingItem : QuickSlotItem
    {
        protected GameObject onUseVFX;

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            onUseVFX = Instantiate(WorldCharacterEffectsManager.Singleton.messageVFX);
            onUseVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            onUseVFX.transform.root.rotation = Quaternion.identity;
        }
    }
}