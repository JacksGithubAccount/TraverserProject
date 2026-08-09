using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TraverserProject
{
    public class UI_MessageWordCategorySelectSlot : MonoBehaviour
    {
        public TextMeshProUGUI wordText;
        public Image highlightIcon;
        public WordCategory wordCategory;


        private void Awake()
        {
            highlightIcon.enabled = false;
        }

        public void SelectSlot()
        {
            highlightIcon.enabled = true;                
            
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void DisplayWordsBasedOnWordCategory()
        {
            PlayerUIManager.Singleton.playerUIMessageManager.DisplayWordsList(wordCategory);
        }

        public void SetTextOfMessageCategorySlot(string text)
        {
            wordText.text = text;
        }
    }
}
