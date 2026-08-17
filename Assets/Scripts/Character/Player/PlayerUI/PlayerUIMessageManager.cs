using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUIMessageManager : PlayerUIMenu
    {
        [HideInInspector] public string currentMessage = "";
        [HideInInspector] public string currentMessage2 = "";
        [HideInInspector] public string completeMessage = "";
        [HideInInspector] public string selectedTemplate1 = "";
        [HideInInspector] public string selectedWord1 = "";
        [HideInInspector] public string selectedConjunction = "";
        [HideInInspector] public string selectedTemplate2 = "";
        [HideInInspector] public string selectedWord2 = "";
        [HideInInspector] public MessagingItem messagingItem;

        public TextMeshProUGUI messageDisplayText;

        public GameObject messageSlotPrefab;
        public GameObject messageCategorySlotPrefab;
        [SerializeField] GameObject closeSubmenuWindow;

        public MessageFormat messageFormat = MessageFormat.Short;
        public string defaultStringForButtonTexts = "______________";


        [Header("Templates")]
        [SerializeField] GameObject templatesMenuWindow;
        [SerializeField] Transform templatesContentWindow;
        [SerializeField] TextMeshProUGUI templatesDisplayButtonText1;
        [SerializeField] GameObject templatesText2;
        [SerializeField] GameObject templatesDisplayButton2GameObject;
        [SerializeField] TextMeshProUGUI templatesDisplayButtonText2;
        [HideInInspector] List<GameObject> templatesSlotPrefabs = new List<GameObject>();

        [Header("Words")]
        [SerializeField] GameObject wordsMenuWindow;
        [SerializeField] Transform wordsCategoryContentWindow;
        [SerializeField] Transform wordsListContentWindow;
        [SerializeField] TextMeshProUGUI wordsDisplayButtonText1;
        [SerializeField] GameObject wordsText2;
        [SerializeField] GameObject wordsDisplayButton2GameObject;
        [SerializeField] TextMeshProUGUI wordsDisplayButtonText2;
        [HideInInspector] List<GameObject> wordsCategorySlotPrefabs = new List<GameObject>();
        [HideInInspector] List<GameObject> wordsListSlotPrefabs = new List<GameObject>();

        [Header("Conjunctions")]
        [SerializeField] GameObject conjunctionsMenuWindow;
        [SerializeField] Transform conjunctionsContentWindow;
        [SerializeField] GameObject conjunctionsText;
        [SerializeField] GameObject conjunctionsDisplayButtonGameObject;
        [SerializeField] TextMeshProUGUI conjunctionsDisplayButtonText;
        [HideInInspector] List<GameObject> conjunctionsSlotPrefabs = new List<GameObject>();

        [Header("Lists")]
        public List<string> templatesTexts;
        public List<string> beingsWordTexts;
        public List<string> directionsWordTexts;
        public List<string> phrasesWordTexts;
        public List<string> conjunctionsTexts;

        [Header("Finish Button")]
        public Button finishButton;

        public override void OpenMenu()
        {
            ResetAllButtonTexts();
            base.OpenMenu();
            messageFormat = MessageFormat.Short;
            SetButtonsBasedOnMessageFormat(messageFormat);
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        public void ResumeAttemptToUseMessagingItem()
        {
            messagingItem.ResumeAttemptToUseItem(PlayerUIManager.Singleton.localPlayer, completeMessage);
            CloseMenu();
        }

        public void AddTemplateToSelectedTemplate(string template, bool isFirst)
        {
            if (isFirst)
            {
                selectedTemplate1 = template;
                templatesDisplayButtonText1.text = template;
            }
            else
            {
                selectedTemplate2 = template;
                templatesDisplayButtonText2.text = template;
            }
            
            GenerateCurrentMessageBasedOnTemplateAndWord();
            
        }

        public void AddWordToSelectedWord(string word, bool isFirst)
        {
            if (isFirst)
            {
                selectedWord1 = word;
                wordsDisplayButtonText1.text = word;
            }
            else
            {
                selectedWord2 = word;
                wordsDisplayButtonText2.text = word;
            }
            
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        public void AddConjunctionToSelectedConjunction(string conjunction)
        {
            selectedConjunction = conjunction;

            conjunctionsDisplayButtonText.text = conjunction;
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        public void ChangeMessageFormat()
        {
            int formatIndex = (int)messageFormat + 1;
            if (formatIndex >= Enum.GetNames(typeof(MessageFormat)).Length)
                formatIndex = 0;
            messageFormat = (MessageFormat)formatIndex;

            SetButtonsBasedOnMessageFormat(messageFormat);
            GenerateCurrentMessageBasedOnTemplateAndWord();
        }

        public void SetButtonsBasedOnMessageFormat(MessageFormat format)
        {
            switch (format)
            {
                case MessageFormat.Short:
                    conjunctionsText.SetActive(false);
                    conjunctionsDisplayButtonGameObject.SetActive(false);
                    templatesText2.SetActive(false);
                    templatesDisplayButton2GameObject.SetActive(false);
                    wordsText2.SetActive(false);
                    wordsDisplayButton2GameObject.SetActive(false);
                    break;
                case MessageFormat.Extended:
                    conjunctionsText.SetActive(true);
                    conjunctionsDisplayButtonGameObject.SetActive(true);
                    templatesText2.SetActive(true);
                    templatesDisplayButton2GameObject.SetActive(true);
                    wordsText2.SetActive(true);
                    wordsDisplayButton2GameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void GenerateCurrentMessageBasedOnTemplateAndWord()
        {            
            if (selectedTemplate1 != "" && selectedWord1 !="")
            {
                currentMessage = selectedTemplate1.Replace("****", selectedWord1);
            }
            else if(selectedTemplate1 == "")
            {
                currentMessage = selectedWord1;
            }else if(selectedWord1 == "")
            {
                currentMessage = selectedTemplate1;
            }

            if (selectedTemplate2 != "" && selectedWord2 != "")
            {
                currentMessage2 = selectedTemplate2.Replace("****", selectedWord2);
            }
            else if (selectedTemplate2 == "")
            {
                currentMessage2 = selectedWord2;
            }
            else if (selectedWord2 == "")
            {
                currentMessage2 = selectedTemplate2;
            }

            switch (messageFormat)
            {
                case MessageFormat.Short:
                    if (selectedTemplate1 != "" && selectedWord1 != "")
                    {
                        finishButton.interactable = true;
                    }
                    completeMessage = currentMessage;
                    break;
                case MessageFormat.Extended:
                    if (selectedTemplate1 != "" && selectedWord1 != "" && selectedTemplate2 != "" && selectedWord2 != "" && selectedConjunction != "")
                    {
                        finishButton.interactable = true;
                    }
                    completeMessage = currentMessage + " " + selectedConjunction + " " + currentMessage2;
                    break;
                default:
                    break;
            }
            messageDisplayText.text = completeMessage;
        }

        public void OpenTemplatesMenu(bool isFirst)
        {
            ClearAllPrefabsInList(conjunctionsSlotPrefabs);
            ClearAllPrefabsInList(templatesSlotPrefabs);
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < templatesTexts.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, templatesContentWindow);
                UI_MessageSlot messageSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageSlot.SetTextOfMessageSlot(templatesTexts[i]);
                if(isFirst)
                    messageSlot.messageType = MessageType.Template1;
                else
                    messageSlot.messageType = MessageType.Template2;

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

        public void OpenWordsMenu(bool isFirst)
        {
            ClearAllPrefabsInList(wordsCategorySlotPrefabs);
            ClearAllPrefabsInList(wordsListSlotPrefabs);

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < WordCategory.GetNames(typeof(WordCategory)).Length; i++)
            {
                GameObject messageCategorySlotGameObject = Instantiate(messageCategorySlotPrefab, wordsCategoryContentWindow);
                UI_MessageWordCategorySelectSlot messageCategorySlot = messageCategorySlotGameObject.GetComponent<UI_MessageWordCategorySelectSlot>();
                messageCategorySlot.wordCategory = (WordCategory)i;
                messageCategorySlot.SetTextOfMessageCategorySlot(messageCategorySlot.wordCategory.ToString());
                if (isFirst)
                    messageCategorySlot.messageType = MessageType.Word1;
                else
                    messageCategorySlot.messageType = MessageType.Word2;

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
            ClearAllPrefabsInList(templatesSlotPrefabs);
            ClearAllPrefabsInList(conjunctionsSlotPrefabs);
            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < conjunctionsTexts.Count; i++)
            {
                GameObject messageSlotGameObject = Instantiate(messageSlotPrefab, conjunctionsContentWindow);
                UI_MessageSlot messageSlot = messageSlotGameObject.GetComponent<UI_MessageSlot>();
                messageSlot.SetTextOfMessageSlot(conjunctionsTexts[i]);
                messageSlot.messageType = MessageType.Conjunction;
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

        public void DisplayWordsList(WordCategory wordCategory, MessageType messageType)
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
                messageListSlot.messageType = messageType;
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

        public void ResetAllButtonTexts()
        {
            templatesDisplayButtonText1.text = defaultStringForButtonTexts;
            wordsDisplayButtonText1.text = defaultStringForButtonTexts;
            conjunctionsDisplayButtonText.text = defaultStringForButtonTexts;
            templatesDisplayButtonText2.text = defaultStringForButtonTexts;
            wordsDisplayButtonText2.text = defaultStringForButtonTexts;

            selectedTemplate1 = "";
            selectedWord1 = "";
            selectedConjunction = "";
            selectedTemplate2 = "";
            selectedWord2 = "";
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
