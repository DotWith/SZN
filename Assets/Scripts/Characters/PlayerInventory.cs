using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] Transform itemSpawn = null;
        [SerializeField] int viewModelLayer;

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        const int MaxItems = 2;

        /// <summary>
        /// The items the inventory contains
        /// </summary>
        public List<GameObject> itemArray = new List<GameObject>();

        /// <summary>
        /// The current item for this inventory
        /// </summary>
        [SyncVar(hook = nameof(OnItemChanged))]
        int activeItemSynced = -1;

        GameObject activeItemPrefab;

        void OnItemChanged(int _Old, int _New)
        {
            if (activeItemPrefab != null)
            {
                Destroy(activeItemPrefab);
                NetworkServer.UnSpawn(activeItemPrefab);
            }

            if (itemArray.Count <= _New) { return; }

            GameObject item = itemArray[_New];

            if (item == null) { return; }

            activeItemPrefab = Instantiate(item);
            NetworkServer.Spawn(activeItemPrefab);
            activeItemPrefab.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            activeItemPrefab.GetComponent<Item>().holder = itemSpawn;
            //activeItemPrefab.layer = viewModelLayer; // TODO: Change this to a LayerMask
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.Inventory1.performed += ctx => CmdChangeActiveItem(0);
            player.Controls.Player.Inventory2.performed += ctx => CmdChangeActiveItem(1);
        }

        public void AddItem(GameObject prefab)
        {
            if (itemArray.Count >= MaxItems) { return; }

            itemArray.Add(prefab);
        }

        [Command]
        public void CmdChangeActiveItem(int newIndex) => activeItemSynced = newIndex;
    }
}
