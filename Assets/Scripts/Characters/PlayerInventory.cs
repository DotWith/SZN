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

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        const int MaxItems = 2;

        /// <summary>
        /// The items the inventory contains
        /// </summary>
        public List<SimpleItem> itemArray = new List<SimpleItem>();

        GameObject activeItemPrefab;

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

        public void RemoveItem(SimpleItem info)
        {
            itemArray.Remove(info);
        }

        [Command]
        void CmdChangeActiveItem(int newIndex)
        {
            if (activeItemPrefab != null)
            {
                Destroy(activeItemPrefab);
                NetworkServer.UnSpawn(activeItemPrefab);
            }

            if (itemArray.Count <= newIndex) { return; }

            SimpleItem info = itemArray[newIndex];

            if (info == null) { return; }

            activeItemPrefab = Instantiate(info.itemPrefab.gameObject);
            NetworkServer.Spawn(activeItemPrefab);
            activeItemPrefab.GetComponent<BasicItem>().netIdentity.RemoveClientAuthority();
            activeItemPrefab.GetComponent<BasicItem>().netIdentity.AssignClientAuthority(connectionToClient);
            activeItemPrefab.GetComponent<BasicItem>().holder = transform;
            activeItemPrefab.GetComponent<BasicItem>().itemInfo = info;
            RpcSetCollision(activeItemPrefab, false);
        }

        [ClientRpc]
        void RpcSetCollision(GameObject item, bool enable)
        {
            if (item.GetComponent<Collider>() == null) { return; }

            item.GetComponent<Collider>().enabled = enable;
            /*foreach (var collider in item.GetComponentsInChildren<Collider>())
            {
                collider.enabled = enable;
            }*/
        }

        [Command]
        void CmdUseActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.GetComponent<BasicItem>().Use();
        }

        [Command]
        void CmdDropActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.GetComponent<BasicItem>().holder = null;
            RpcSetCollision(activeItemPrefab, true);
            activeItemPrefab = null;

            RemoveItem(activeItemPrefab.GetComponent<BasicItem>().itemInfo);
        }
    }
}
