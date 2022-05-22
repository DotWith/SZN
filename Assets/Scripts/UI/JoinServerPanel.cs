using Mirror;
using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Dot.SZN.UI
{
    public class JoinServerPanel : MonoBehaviour
    {
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

        public GameObject serverJoinButton;
        public Transform contextPanel;

        public NetworkDiscovery networkDiscovery;

        public List<GameObject> serverButtons = new List<GameObject>();

        public void HostServer()
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        public void RefreshServerList()
        {
            foreach (var server in serverButtons)
            {
                Destroy(server);
            }

            networkDiscovery.StartDiscovery();

            foreach (ServerResponse info in discoveredServers.Values)
            {
                Debug.Log(info);
                var serverButton = Instantiate(serverJoinButton, contextPanel);
                serverButton.GetComponent<Button>().onClick.AddListener(() => Connect(info));

                serverButtons.Add(serverButton);
            }
        }

        void Connect(ServerResponse info)
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }
    }
}
