using Com.Dot.SZN.Characters;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Lamp Item", menuName = "Item/Lamp Item")]
    public class LampItem : SimpleItem
    {
        public override void OnUse(NetworkIdentity owner)
        {
            var subLight = owner.GetComponent<PlayerInventory>().viewModelTransform.GetComponentInChildren<Light>();
            subLight.enabled = !subLight.enabled;
        }
    }
}
