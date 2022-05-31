using Mirror;
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
