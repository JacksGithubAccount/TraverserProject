using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TraverserProject
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Singleton { get; set; }

        public PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

        private void Awake()
        {
            //there can only be one of this object in the game at any one time. if another exist, destroy it
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }
        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_0" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_0" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_0" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_0" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_0" + (int)currentCharacterSlotBeingUsed + 1;
                    break;

            }
            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                Debug.Log("2");
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            if (!saveFileDataWriter.CheckToSeeIfFileExist())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            TitleScreenManager.Singleton.DisplayNoFreeCharacterSlotsPopUp();
        }

            private void NewGame()
            {
                player.playerNetworkManager.vitality.Value = 10;
                player.playerNetworkManager.endurance.Value = 10;
                

                SaveGame();
                StartCoroutine(LoadWorldScene());
            }
            public void LoadGame()
            {
                saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
                saveFileDataWriter = new SaveFileDataWriter();
                saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
                saveFileDataWriter.saveFileName = saveFileName;
                currentCharacterData = saveFileDataWriter.LoadSaveFile();

                StartCoroutine(LoadWorldScene());
            }

            public void SaveGame()
            {
                saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

                saveFileDataWriter = new SaveFileDataWriter();
                saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
                saveFileDataWriter.saveFileName = saveFileName;

                player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

                saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
            }

            public void DeleteGame(CharacterSlot characterSlot)
            {
                saveFileDataWriter = new SaveFileDataWriter();
                saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                saveFileDataWriter.DeleteSaveFile();

            }

            private void LoadAllCharacterProfiles()
            {
                saveFileDataWriter = new SaveFileDataWriter();
                saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
                characterSlot01 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
                characterSlot02 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
                characterSlot03 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
                characterSlot04 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
                characterSlot05 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
                characterSlot06 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
                characterSlot07 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
                characterSlot08 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
                characterSlot09 = saveFileDataWriter.LoadSaveFile();
                saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
                characterSlot10 = saveFileDataWriter.LoadSaveFile();
            }
            public IEnumerator LoadWorldScene()
            {
                //for one world scene use this
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
                //use for different scenes to levels in project
                //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

                player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);

                yield return null;
            }
            public int GetWorldSceneIndex()
            {
                return worldSceneIndex;
            }
        }
    }