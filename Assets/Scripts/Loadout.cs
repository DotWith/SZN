using Com.Dot.SZN.PersistentData;
using Com.Dot.SZN.ScriptableObjects;
using UnityEngine;

namespace Com.Dot.SZN
{
    public class Loadout : MonoBehaviour, ISaveable
    {
        public SimpleItem[] defaultLoadout;

        public void LoadFromSaveData(SaveData saveData)
        {
            throw new System.NotImplementedException();
        }

        public void PopulateSaveData(SaveData saveData)
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            var array = new ISaveable[] { this };
            SaveDataManager.SaveJsonData(array);
        }
    }
}
