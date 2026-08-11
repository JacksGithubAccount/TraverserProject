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
                AddTextToMessageTemplate();
            }
            else if (messageType == MessageType.Word1)
            {
                AddTextToMessageWord();
            }
            else if (messageType == MessageType.Conjunction)
            {
                AddTextToMessageWord();
            }
            else if (messageType == MessageType.Template2)
            {
                AddTextToMessageWord();
            }
            else if (messageType == MessageType.Word2)
            {
                AddTextToMessageWord();
            }
            PlayerUIManager.Singleton.playerUIMessageManager.CloseSubMenu();
        }

        private void AddTextToMessageTemplate()
        {
            PlayerUIManager.Singleton.playerUIMessageManager.AddTemplateToSelectedTemplate(messageText.text);
        }
        private void AddTextToMessageWord()
        {
            PlayerUIManager.Singleton.playerUIMessageManager.AddWordToSelectedWord(messageText.text);
        }
    }
}
