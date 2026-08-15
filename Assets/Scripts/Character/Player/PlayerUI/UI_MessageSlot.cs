using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_MessageSlot : MonoBehaviour
    {
        public Image highlightIcon;
        public Image greyedOutIcon;
        public Image GlowIcon;
        public TextMeshProUGUI messageText;
        public MessageType messageType;

        

        private void Awake()
        {
            highlightIcon.enabled = false;
            greyedOutIcon.enabled = false;
            GlowIcon.enabled = false;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void SetTextOfMessageSlot(string text)
        {
            messageText.text = text;
        }

        public void AddTextToMessage()
        {
            if (messageType == MessageType.Template1)
            {
                AddTextToMessageTemplate(true);
            }
            else if (messageType == MessageType.Word1)
            {
                AddTextToMessageWord(true);
            }
            else if (messageType == MessageType.Conjunction)
            {
                AddConjunctionToMessage();
            }
            else if (messageType == MessageType.Template2)
            {
                AddTextToMessageTemplate(false);
            }
            else if (messageType == MessageType.Word2)
            {
                AddTextToMessageWord(false);
            }
            PlayerUIManager.Singleton.playerUIMessageManager.CloseSubMenu();
        }

        private void AddTextToMessageTemplate(bool isFirst)
        {
            PlayerUIManager.Singleton.playerUIMessageManager.AddTemplateToSelectedTemplate(messageText.text, isFirst);
        }
        private void AddTextToMessageWord(bool isFirst)
        {
            PlayerUIManager.Singleton.playerUIMessageManager.AddWordToSelectedWord(messageText.text, isFirst);
        }
        private void AddConjunctionToMessage()
        {
            PlayerUIManager.Singleton.playerUIMessageManager.AddConjunctionToSelectedConjunction(messageText.text);
        }
    }
}
