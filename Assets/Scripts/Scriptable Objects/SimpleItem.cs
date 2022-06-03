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

        public void Reset() => id = Guid.NewGuid().ToString();

        #region Item Virtuals
        public virtual void OnAdd() { }
        public virtual void OnRemove() { }
        public virtual void OnEquip() { }
        public virtual void OnUse() { }
        #endregion
    }
}
