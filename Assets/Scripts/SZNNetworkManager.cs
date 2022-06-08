using Com.Dot.SZN.InventorySystem;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using System.Linq;
using UnityEngine;

namespace Com.Dot.SZN
{
    public class SZNNetworkManager : NetworkRoomManager
    {
        [Header("Maps")]
        public int numberOfRounds = 1;
        public MapSet mapSet;

        public GameObject hud;

        MapHandler mapHandler;

        public override void Awake()
        {
            base.Awake();

            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
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

        public override void OnRoomServerPlayersReady()
        {
            ServerNextScene();
        }

        public void ServerNextScene()
        {
            mapHandler = new MapHandler(mapSet, numberOfRounds);
            ServerChangeScene(mapHandler.NextMap);
        }
    }
}
