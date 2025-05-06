using System.IO;
using System;
using UnityEngine;
using System.Linq.Expressions;


namespace TraverserProject
{
    [System.Serializable ]
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath ="";
        public string saveFileName ="";

        public bool CheckToSeeIfFileExist()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATE SAVE FILE, AT SAVE PATH: " + savePath);
                //serialize to json
                string dataToStore = JsonUtility.ToJson(characterData, true);
                //writes file to system
                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }catch(Exception ex)
            {
                Debug.LogError("ERROR WHILE TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED: " + savePath + "\n" + ex);
            }
        }

        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                    Debug.Log("ERROR LOADING SAVE: " + ex);
                }
            }
            
            return characterData;
        }
    }
}
