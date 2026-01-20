using System;
using TMPro;
using UnityEngine;

namespace TraverserProject
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("CharacterInfo")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlot();
        }

        private void LoadSaveSlot()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath; ;

            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot01.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot01.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot02.characterName;               
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot02.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot03.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot03.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot04.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot04.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot05.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot05.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot06.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot06.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot07.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot07.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot08.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot08.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot09.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot09.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                saveFileDataWriter.saveFileName = WorldSaveGameManager.Singleton.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                if (saveFileDataWriter.CheckToSeeIfFileExist())
                {
                    characterName.text = WorldSaveGameManager.Singleton.characterSlot10.characterName;
                    timePlayed.text = WorldUtilityManager.Singleton.ConvertSecondsToHHMMSSFormat(WorldSaveGameManager.Singleton.characterSlot10.secondsPlayed);
                }
                else
                {
                    //hides slot if nothing
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Singleton.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Singleton.LoadGame();

        }
        public void SelectCurrentSlot()
        {
            TitleScreenManager.Singleton.SelectCharacterSlot(characterSlot);
        }
    }
}