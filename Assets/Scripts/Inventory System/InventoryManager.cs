using Com.Dot.SZN.Characters;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Com.Dot.SZN.InventorySystem
{
    [DisallowMultipleComponent]
    public class InventoryManager : MonoBehaviour
    {
        [Header("Inventory Settings")]

        [FormerlySerializedAs("m_MaxInventoryItems")]
        [SerializeField]
        [Tooltip("Maximum number of items in all inventories")]
        public int maxItems = 10;

        public static InventoryManager singleton { get; private set; }

        InventoryList<string> items = new InventoryList<string>();
        List<SimpleItem> loadedItems = new List<SimpleItem>();

        public Action<InventoryList<string>, int> onSyncItems;
        public Action onRemoveItem;

        int activeItem;

        public void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            // TODO: Make server sided
            //loadedItems = Resources.LoadAll<SimpleItem>("Items").ToList();
        }

        public void SetupClient()
        {
            NetworkClient.RegisterHandler<Item>(OnItem);
            NetworkClient.RegisterHandler<RemoveItem>(OnRemoveItem);
            NetworkClient.RegisterHandler<ChangeItem>(OnChangeItem);
            NetworkClient.RegisterHandler<UseItem>(OnUseItem);
        }

        public void UnregisterClient()
        {
            NetworkClient.UnregisterHandler<Item>();
            NetworkClient.UnregisterHandler<RemoveItem>();
            NetworkClient.UnregisterHandler<ChangeItem>();
            NetworkClient.UnregisterHandler<UseItem>();
        }

        public void SetupServer()
        {
            loadedItems = Resources.LoadAll<SimpleItem>("Items").ToList();
        }

        #region Client Callbacks
        /// <summary>Add said item from this inventory</summary>
        /// <param name="id"></param>
        [ClientCallback]
        public void AddItem(string id, GameObject obj)
        {
            var msg = new Item(id, obj);
            NetworkServer.SendToAll(msg);
        }

        /// <summary>Remove said item from this inventory</summary>
        /// <param name="id"></param>
        [ClientCallback]
        public void RemoveItem()
        {
            var msg = new RemoveItem(activeItem);
            NetworkServer.SendToAll(msg);

            onRemoveItem?.Invoke();
        }

        [ClientCallback]
        public void ChangeItem(int index)
        {
            var msg = new ChangeItem(index);
            NetworkServer.SendToAll(msg);

            SyncItems();
        }

        [ClientCallback]
        public void UseItem()
        {
            var msg = new UseItem(activeItem);
            NetworkServer.SendToAll(msg);
        }
        #endregion // Client Callbacks

        #region Registered Handles
        void OnItem(Item msg)
        {
            // Checks if we are going above the limit
            if (items.GetCount() >= maxItems)
                return;

            items.AddValue(activeItem, msg.id);

            SimpleItem item = GetItem(items.GetValue(activeItem));

            if (item == null)
                return;

            item.OnPickup();

            NetworkServer.Destroy(msg.obj);
        }
        
        void OnRemoveItem(RemoveItem msg)
        {
            // Checks if the item is the same
            if (activeItem.Equals(msg.activeItem))
                return;

            SimpleItem item = GetItem(items.GetValue(activeItem));

            if (item == null)
                return;

            item.OnDrop();

            items.DeleteValue(activeItem);
        }

        void OnChangeItem(ChangeItem msg)
        {
            // Checks if the item we want to change to exists
            if (items.GetCount() < msg.index)
                return;

            activeItem = msg.index;

            SimpleItem item = GetItem(items.GetValue(activeItem));

            if (item == null)
                return;

            item.OnEquip();
        }

        void OnUseItem(UseItem msg)
        {
            // Checks if we are selecting that item
            if (!activeItem.Equals(msg.activeItem))
                return;

            SimpleItem item = GetItem(items.GetValue(activeItem));

            if (item == null)
                return;

            item.OnUse();
        }
        #endregion // Registered Handles

        /*[Server]
        public void Update()
        {
            InvokeRepeating(nameof(VerifyInventories), 3f, 3f);
        }

        [ServerCallback]
        public void VerifyInventories()
        {
            Debug.Log("Verify All Inventories");
        }*/

        // SECURTY!
        void SyncItems() => onSyncItems?.Invoke(items, activeItem);

        [Server]
        public SimpleItem GetItem(string id) => loadedItems.Find(i => i.id == id);
    }
}
