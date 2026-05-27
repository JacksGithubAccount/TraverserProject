using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUIDialogueWindowManager : PlayerUIMenu
    {
        [Header("Dialogue Character")]
        [SerializeField] TextMeshProUGUI aiCharacterNameText;

        [Header("Dialogue Options")]
        [SerializeField] Button talkToCharacterButton; //always enabled
        [SerializeField] Button buyItemsButton;         //only enabled if shopkeeper
        [SerializeField] Button sellItemsButton;        //only enabled if shopkeeper
        [SerializeField] Button blacksmithButton;       //only enabled if blacksmith

        public override void OpenMenu()
        {

            base.OpenMenu();
            EnableDialogueOptionsBasedOnConversationCharacter();
        }

        public override void OpenMenuAfterFixedFrame()
        {
            base.OpenMenuAfterFixedFrame();
            EnableDialogueOptionsBasedOnConversationCharacter();

        }

        private void EnableDialogueOptionsBasedOnConversationCharacter()
        {
            buyItemsButton.gameObject.SetActive(false);
            sellItemsButton.gameObject.SetActive(false);
            blacksmithButton.gameObject.SetActive(false);

            AICharacterManager dialogueCharacter = PlayerUIManager.Singleton.localPlayer.playerInteractionManager.dialogueCharacter;

            if (dialogueCharacter == null)
            {
                CloseMenu();
                return;
            }

            aiCharacterNameText.text = dialogueCharacter.characterName;

            if (dialogueCharacter.isShop)
            {
                buyItemsButton.gameObject.SetActive(true);
                sellItemsButton.gameObject.SetActive(true);
            }

            if (dialogueCharacter.isBlacksmith)
            {
                blacksmithButton.gameObject.SetActive(true);
            }
        }

        public void OpenBuyFromShopMenu()
        {
            CloseMenu();
            PlayerUIManager.Singleton.playerUIShopManager.OpenBuyMenu();
        }

        public void OpenSellToShopMenu()
        {
            CloseMenu();
            PlayerUIManager.Singleton.playerUIShopManager.OpenSellMenu();
        }
    }
}