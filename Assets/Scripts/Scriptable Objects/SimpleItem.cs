using Com.Dot.SZN.Characters;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Simple Item", menuName = "Item/Simple Item")]
    public class SimpleItem : ScriptableObject
    {
        public new string name;

        [Multiline]
        public string description;

        [Header("Icon")]
        public Sprite icon;

        [Header("Model")]
        public ModelInfo worldModel = new ModelInfo();
        public ModelInfo viewModel = new ModelInfo();

        public virtual void Use(Player client) { }
    }
}
