using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com.Dot.SZN.PersistentData
{
    public static class SaveDataManager
    {
        /// <summary>
        /// Persistent-Data-Unity
        /// </summary>
        public const string SaveDataFormat = ".pdu";

        /// <summary>
        /// Saving and Loading
        /// </summary>
        public static void SaveJsonData(IEnumerable<ISaveable> saveables)
        {
            SaveData saveData = new SaveData();

            foreach (var saveable in saveables)
            {
                saveable.PopulateSaveData(saveData);
            }

            if (WriteToFile($"SaveData{SaveDataFormat}", saveData.ToJson()))
            {
                Debug.Log("Save successful");
            }
        }

        public static void LoadJsonData(IEnumerable<ISaveable> saveables)
        {
            if (LoadFromFile($"SaveData{SaveDataFormat}", out var json))
            {
                SaveData saveData = new SaveData();
                saveData.LoadFromJson(json);

                foreach (var saveable in saveables)
                {
                    saveable.LoadFromSaveData(saveData);
                }

                Debug.Log("Load complete");
            }
        }

        #region File Manager
        public static bool WriteToFile(string a_FileName, string a_FileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

            try
            {
                File.WriteAllText(fullPath, a_FileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string a_FileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
        #endregion // File Manager
    }
}
