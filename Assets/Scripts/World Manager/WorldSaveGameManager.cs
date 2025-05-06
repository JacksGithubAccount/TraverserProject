using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TraverserProject
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Singleton { get; set; }

        [SerializeField] PlayerManager player;

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
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_" + (int)currentCharacterSlotBeingUsed + 1;
                    break;

            }
            return fileName;
        }

        public void CreateNewGame()
        {
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
            currentCharacterData = new CharacterSaveData();

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

        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter= new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath= Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();
        }
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}