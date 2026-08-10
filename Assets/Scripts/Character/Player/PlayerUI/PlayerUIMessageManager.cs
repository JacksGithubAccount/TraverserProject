using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TraverserProject
{
    public class PlayerUIMessageManager : PlayerUIMenu
    {
        [HideInInspector] public string currentMessage = "";
        [HideInInspector] public string selectedTemplate = "";
        [HideInInspector] public string selectedWord = "";
        [HideInInspector] public MessagingItem messagingItem;

        public TextMeshProUGUI messageDisplayText;

        public GameObject messageSlotPrefab;
        public GameObject messageCategorySlotPrefab;
        [SerializeField] GameObject closeSubmenuWindow;


        [Header("Templates")]
        [SerializeField] GameObject templatesMenuWindow;
        [SerializeField] Transform templatesContentWindow;
        [SerializeField] TextMeshProUGUI templatesDisplayButtonText;
        [HideInInspector] List<GameObject> templatesSlotPrefabs = new List<GameObject>();

        [Header("Words")]
        [SerializeField] GameObject wordsMenuWindow;
        [SerializeField] Transform wordsCategoryContentWindow;
        [SerializeField] Transform wordsListContentWindow;
        [SerializeField] TextMeshProUGUI wordsDisplayButtonText;
        [HideInInspector] List<GameObject> wordsCategorySlotPrefabs = new List<GameObject>();
        [HideInInspector] List<GameObject> wordsListSlotPrefabs = new List<GameObject>();

        [Header("Conjunctions")]
        [SerializeField] GameObject conjunctionsMenuWindow;
        [SerializeField] Transform conjunctionsContentWindow;
        [SerializeField] TextMeshProUGUI conjunctionsDisplayButtonText;
        [HideInInspector] List<GameObject> conjunctionsSlotPrefabs = new List<GameObject>();

        public List<string> templatesTexts;
        public List<string> beingsWordTexts;
        public List<string> directionsWordTexts;
        public List<string> phrasesWordTexts;
        public List<string> conjunctionsTexts;

        public override void OpenMenu()
        {
            base.OpenMenu();
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        public void ResumeAttemptToUseMessagingItem()
        {
            messagingItem.ResumeAttemptToUseItem(PlayerUIManager.Singleton.localPlayer, currentMessage);
            CloseMenu();
        }

        public void AddTemplateToSelectedTemplate(string template)
        {
            
            selectedTemplate = template;
            templatesDisplayButtonText.text = selectedTemplate;
            GenerateCurrentMessageBasedOnTemplateAndWord();
            
        }

        public void AddWordToSelectedWord(string word)
        {
            selectedWord = word;
            wordsDisplayButtonText.text = selectedWord;
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        private void GenerateCurrentMessageBasedOnTemplateAndWord()
        {            
            if (selectedTemplate != "" && selectedWord !="")
            {
                currentMessage = selectedTemplate.Replace("****", selectedWord);
            }
            else if(selectedTemplate == "")
            {
                currentMessage = selectedWord;
            }else if(selectedWord == "")
            {
                currentMessage = selectedTemplate;
            }
            
            messageDisplayText.text = currentMessage;
        }

        public void OpenTemplatesMenu()
        {
            ClearAllPrefabsInList(templatesSlotPrefabs);
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < templatesTexts.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, templatesContentWindow);
                UI_MessageSlot messageSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageSlot.SetTextOfMessageSlot(templatesTexts[i]);
                messageSlot.isTemplate = true;
                templatesSlotPrefabs.Add(messageSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button messageSlotButton = messageSlotGameObject.GetComponent<Button>();
                    messageSlotButton.Select();
                    messageSlotButton.OnSelect(null);

                }
            }
            OpenSubMenu(templatesMenuWindow);
            
        }

        public void OpenWordsMenu()
        {
            ClearAllPrefabsInList(wordsCategorySlotPrefabs);
            
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < WordCategory.GetNames(typeof(WordCategory)).Length; i++)
            {
                GameObject messageCategorySlotGameObject = Instantiate(messageCategorySlotPrefab, wordsCategoryContentWindow);
                UI_MessageWordCategorySelectSlot messageCategorySlot = messageCategorySlotGameObject.GetComponent<UI_MessageWordCategorySelectSlot>();
                messageCategorySlot.wordCategory = (WordCategory)i;
                messageCategorySlot.SetTextOfMessageCategorySlot(messageCategorySlot.wordCategory.ToString());
                wordsCategorySlotPrefabs.Add(messageCategorySlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button messageCategorySlotButton = messageCategorySlotGameObject.GetComponent<Button>();
                    messageCategorySlotButton.Select();
                    messageCategorySlotButton.OnSelect(null);

                }
            }
            OpenSubMenu(wordsMenuWindow);

        }

        public void OpenConjunctionsMenu()
        {
            ClearAllPrefabsInList(conjunctionsSlotPrefabs);
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < conjunctionsTexts.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, conjunctionsContentWindow);
                UI_MessageSlot messageSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageSlot.SetTextOfMessageSlot(conjunctionsTexts[i]);
                messageSlot.isTemplate = true;
                conjunctionsSlotPrefabs.Add(messageSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button messageSlotButton = messageSlotGameObject.GetComponent<Button>();
                    messageSlotButton.Select();
                    messageSlotButton.OnSelect(null);

                }
            }
            OpenSubMenu(conjunctionsMenuWindow);

        }

        public void DisplayWordsList(WordCategory wordCategory)
        {
            ClearAllPrefabsInList(wordsListSlotPrefabs);

            List<string> wordsList;
            
            switch(wordCategory)
            {
                case WordCategory.Beings:
                    wordsList = beingsWordTexts;
                    break;
                case WordCategory.Directions:
                    wordsList = directionsWordTexts;
                    break;
                case WordCategory.Phrases:
                    wordsList = phrasesWordTexts;
                    break;
                default:
                    wordsList = new List<string>();
                    break;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < wordsList.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, wordsListContentWindow);
                UI_MessageSlot messageListSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageListSlot.SetTextOfMessageSlot(wordsList[i]);
                wordsListSlotPrefabs.Add(messageListSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button messageListSlotButton = messageSlotGameObject.GetComponent<Button>();
                    messageListSlotButton.Select();
                    messageListSlotButton.OnSelect(null);

                }
            }
        }

        private void ClearAllPrefabsInList(List<GameObject> listOfPrefabsToDestroy)
        {
            foreach (GameObject item in listOfPrefabsToDestroy)
            {
                Destroy(item);
            }
            listOfPrefabsToDestroy.Clear();
        }

        public void DisplayWordsBasedOnWordCategory(WordCategory wordCategory)
        {
            List<string> words;

            switch(wordCategory)
            {
                case WordCategory.Beings:
                    words = beingsWordTexts;
                    break;
                case WordCategory.Directions:
                    words = directionsWordTexts;
                    break;
                case WordCategory.Phrases:
                    words = phrasesWordTexts;
                    break;
                default:
                    words = new List<string>();
                    break;

            }
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < words.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, templatesContentWindow);
                UI_MessageSlot messageSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageSlot.SetTextOfMessageSlot(words[i]);
                templatesSlotPrefabs.Add(messageSlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button messageSlotButton = messageSlotGameObject.GetComponent<Button>();
                    messageSlotButton.Select();
                    messageSlotButton.OnSelect(null);

                }
            }
        }
    }
}
