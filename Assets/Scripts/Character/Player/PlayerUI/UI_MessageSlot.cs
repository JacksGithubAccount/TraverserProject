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
        public bool isTemplate = false;

        

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
            if (isTemplate)
            {
                AddTextToMessageTemplate();
            }
            else
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
