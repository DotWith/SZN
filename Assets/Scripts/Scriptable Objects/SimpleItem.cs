using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interactables;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Simple Item", menuName = "Item/Simple Item")]
    public class SimpleItem : ScriptableObject
    {
        public BasicItem itemPrefab;

        [Header("Properties")]
        public new string name;

        [Multiline]
        public string description;

        [Header("Icon")]
        public Sprite icon;

        public virtual void Use(Player client) { }
    }
}
