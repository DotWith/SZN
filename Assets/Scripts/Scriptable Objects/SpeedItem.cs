using Com.Dot.SZN.Characters;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Speed Item", menuName = "Item/Speed Item")]
    public class SpeedItem : SimpleItem
    {
        public float speed = 2;
        public float time = 5;

        public override void OnUse(NetworkIdentity owner) => owner.GetComponent<Player>().move.SetMovementSpeed(speed, time);
    }
}
