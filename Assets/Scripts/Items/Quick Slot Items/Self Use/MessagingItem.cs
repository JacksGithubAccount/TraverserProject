using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Messaging Item")]
    public class MessagingItem : QuickSlotItem
    {       
        [Header("Message")]
        public GameObject messageInteractablePrefab;
        private string messageString;
        protected GameObject onUseVFX;

        public override void AttemptToUseItem(PlayerManager player)
        {
            PlayerUIManager.Singleton.playerUIMessageManager.OpenMenu();
            PlayerUIManager.Singleton.playerUIMessageManager.messagingItem = this;
        }

        public void ResumeAttemptToUseItem(PlayerManager player, string message)
        {
            messageString = message;
            base.AttemptToUseItem(player);
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            GameObject messageInteractable = Instantiate(messageInteractablePrefab);
            MessageInteractable mi = messageInteractable.GetComponent<MessageInteractable>();
            mi.messagePopUp = messageString;
            messageInteractable.transform.position = player.playerEffectsManager.effectTransform.position;
            messageInteractable.transform.position += new Vector3(0, -1, 0);
            messageInteractable.transform.root.rotation = Quaternion.identity;
        }
    }
}