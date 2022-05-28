using Com.Dot.SZN.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Speed Item", menuName = "Item/Speed Item")]
    public class SpeedItem : SimpleItem
    {
        public float speed = 2;
        public float time = 5;

        public override void Use(Player client)
        {
            client.move.SetMovementSpeedForTime(speed, time);
        }
    }
}