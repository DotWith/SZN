using Com.Dot.SZN.Interactables;
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
        [SerializeField] int viewModelLayer;
        [SerializeField] int defaultLayer;

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

        public void AddItem()
        {
            if (itemArray.Count >= MaxItems) { return; }

            itemArray.Add(itemPrefab);
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

            GameObject item = itemArray[newIndex];

            if (item == null) { return; }

            activeItemPrefab = Instantiate(item);
            NetworkServer.Spawn(activeItemPrefab);
            activeItemPrefab.GetComponent<NetworkIdentity>().RemoveClientAuthority();
            activeItemPrefab.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
            activeItemPrefab.GetComponent<BasicItem>().holder = transform;
            RpcSetCollision(activeItemPrefab.GetComponent<BasicItem>(), false);
            SetItemLayer(activeItemPrefab, true);
        }

        [ClientRpc]
        void RpcSetCollision(BasicItem item, bool enable)
        {
            foreach (var collider in item.colliders)
            {
                collider.enabled = enable;
            }
        }

        [Client]
        void SetItemLayer(GameObject item, bool viewmodel)
        {
            item.layer = viewmodel ? viewModelLayer : defaultLayer;
            for (int i = 0; i < item.transform.childCount; i++)
            {
                var child = item.transform.GetChild(i);
                child.gameObject.layer = viewmodel ? viewModelLayer : defaultLayer;
            }
        }

        [Command]
        public void CmdDropActiveItem()
        {
            if (activeItemPrefab == null) { return; }

            activeItemPrefab.GetComponent<BasicItem>().holder = null;
            RpcSetCollision(activeItemPrefab.GetComponent<BasicItem>(), true);
            SetItemLayer(activeItemPrefab, false);
            activeItemPrefab = null;
        }
    }
}
