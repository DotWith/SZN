using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu]
    public class SimpleItem : ScriptableObject
    {
        public new string name;

        [Multiline]
        public string description;

        [Space]
        public Sprite icon;
    }
}
