using Mirror;
using System;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Simple Item", menuName = "Item/Simple Item")]
    public class SimpleItem : ScriptableObject
    {
        [HideInInspector]
        public string id;

        public GameObject prefab;
        public Sprite icon;
        //public bool showToList = false;

        public void Reset() => id = Guid.NewGuid().ToString();

        #region Item Virtuals
        public virtual void OnAdd(NetworkIdentity owner) { }
        public virtual void OnRemove(NetworkIdentity owner) { }
        public virtual void OnEquip(NetworkIdentity owner) { }
        public virtual void OnUse(NetworkIdentity owner) { }
        #endregion
    }
}
