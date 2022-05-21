using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] Transform itemHolder;
        [SerializeField] int viewModelLayer;

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        const int MaxItems = 2;

        /// <summary>
        /// The items the inventory contains
        /// </summary>
        public List<GameObject> itemArray = new List<GameObject>();

        GameObject activeItemPrefab;

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.Inventory1.performed += ctx => CmdChangeActiveItem(0);
            player.Controls.Player.Inventory2.performed += ctx => CmdChangeActiveItem(1);

            player.Controls.Player.Drop.performed += ctx => CmdDropActiveItem();
        }

        public void AddItem(GameObject prefab)
        {
            if (itemArray.Count >= MaxItems) { return; }

            itemArray.Add(prefab);
        }

        [Command]
        public void CmdChangeActiveItem(int newIndex)
        {
            if (activeItemPrefab != null)
            {
                Destroy(activeItemPrefab);
                NetworkServer.UnSpawn(activeItemPrefab);
            }

            if (itemArray.Count <= newIndex) { return; }

            GameObject item = itemArray[newIndex];

            if (item == null) { return; }

            activeItemPrefab = Instantiate(item);
            NetworkServer.Spawn(activeItemPrefab);
            activeItemPrefab.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            activeItemPrefab.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            activeItemPrefab.GetComponent<Item>().holder = transform;
            RpcSetCollision(activeItemPrefab.GetComponent<Item>(), false);
            //item.gameObject.layer = viewModelLayer; // TODO: Change this to a LayerMask
        }

        [ClientRpc]
        void RpcSetCollision(Item item, bool enable)
        {
            foreach (var collider in item.colliders)
            {
                collider.enabled = enable;
            }
        }

        [Command]
        public void CmdDropActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.GetComponent<Item>().holder = null;
            RpcSetCollision(activeItemPrefab.GetComponent<Item>(), true);
        }
    }
}
