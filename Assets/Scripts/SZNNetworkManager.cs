using Com.Dot.SZN.InventorySystem;
using Mirror;
using System.Linq;
using UnityEngine;

namespace Com.Dot.SZN
{
    public class SZNNetworkManager : NetworkRoomManager
    {
        public GameObject hud;

        public override void Awake()
        {
            base.Awake();

            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            InventoryManager.singleton.SetupClient();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            InventoryManager.singleton.UnregisterClient();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            InventoryManager.singleton.SetupServer();
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();

            if (IsSceneActive(GameplayScene))
            {
                if (NetworkClient.isConnected)
                    Instantiate(hud);
            }
        }
    }
}
