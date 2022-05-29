using Com.Dot.SZN.Characters;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.Dot.SZN
{
    public class SZNNetworkManager : NetworkRoomManager
    {
        public override void Awake()
        {
            base.Awake();

            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }
    }
}
