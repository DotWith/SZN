using Com.Dot.SZN.Interactables;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] GameObject itemPrefab;
        [SerializeField] Transform itemHolder;

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        const int MaxItems = 2;

        /// <summary>
        /// The items the inventory contains
        /// </summary>
        public List<SimpleItem> itemArray = new List<SimpleItem>();

        BasicItem activeItemPrefab;

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.Inventory1.performed += ctx => CmdChangeActiveItem(0);
            player.Controls.Player.Inventory2.performed += ctx => CmdChangeActiveItem(1);

            player.Controls.Player.Use.performed += ctx => CmdUseActiveItem();
            player.Controls.Player.Drop.performed += ctx => CmdDropActiveItem();
        }

        public void AddItem(SimpleItem info)
        {
            if (itemArray.Count >= MaxItems) { return; }

            itemArray.Add(info);
        }

        [Command]
        void CmdChangeActiveItem(int newIndex)
        {
            if (activeItemPrefab != null)
            {
                Destroy(activeItemPrefab);
                NetworkServer.UnSpawn(activeItemPrefab.gameObject);
            }

            if (itemArray.Count <= newIndex) { return; }

            SimpleItem info = itemArray[newIndex];

            if (info == null) { return; }

            activeItemPrefab = Instantiate(itemPrefab).GetComponent<BasicItem>();
            NetworkServer.Spawn(activeItemPrefab.gameObject);
            activeItemPrefab.netIdentity.RemoveClientAuthority();
            activeItemPrefab.netIdentity.AssignClientAuthority(connectionToClient);
            activeItemPrefab.holder = transform;
            activeItemPrefab.itemInfo = info;
            RpcSetCollision(activeItemPrefab, false);
        }

        [ClientRpc]
        void RpcSetCollision(BasicItem item, bool enable)
        {
            foreach (var collider in item.GetComponentsInChildren<Collider>())
            {
                collider.enabled = enable;
            }
        }

        [Command]
        void CmdUseActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.itemInfo.Use(player);
        }

        [Command]
        void CmdDropActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.holder = null;
            RpcSetCollision(activeItemPrefab, true);
            activeItemPrefab = null;
        }
    }
}
