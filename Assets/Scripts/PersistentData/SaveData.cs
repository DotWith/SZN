using Com.Dot.SZN.ScriptableObjects;
using UnityEngine;

namespace Com.Dot.SZN.PersistentData
{
    [System.Serializable]
    public class SaveData
    {
        public SimpleItem[] loadout { get; private set; }

        public string ToJson() => JsonUtility.ToJson(this);
        public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
    }

    public interface ISaveable
    {
        void PopulateSaveData(SaveData saveData);
        void LoadFromSaveData(SaveData saveData);
    }
}
