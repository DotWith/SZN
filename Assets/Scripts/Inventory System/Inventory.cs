using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.Dot.SZN.InventorySystem
{
    [DisallowMultipleComponent]
    public class Inventory : NetworkBehaviour
    {
        public readonly SyncList<string> items = new SyncList<string>();

        [SyncVar(hook = nameof(OnChangeItem))]
        public int activeItem;

        public int maxItems = 2;

        public Action<IReadOnlyCollection<string>, int> onSyncItems;
        public Action<string> onRemoveItem;

        public override void OnStartClient()
        {
            items.Callback += OnItemsUpdated;

            for (int i = 0; i < items.Count; i++)
                OnItemsUpdated(SyncList<string>.Operation.OP_ADD, i, string.Empty, items[i]);
        }

        #region Voids
        /// <summary>Add said item from this inventory</summary>
        /// <param name="id"></param>
        public bool AddItem(string id)
        {
            CmdAddItem(id);

            return items.Contains(id);
        }

        /// <summary>Remove said item from this inventory</summary>
        /// <param name="index"></param>
        public void RemoveItem(string id) => CmdRemoveItem(id);

        /// <summary>Change to this index</summary>
        /// <param name="index"></param>
        public void ChangeItem(int index) => CmdChangeItem(index);

        /// <summary>Use active item</summary>
        public void UseActiveItem() => CmdUseActiveItem();
        #endregion // Voids

        #region Commands
        [Command(requiresAuthority = false)]
        void CmdAddItem(string id)
        {
            if (!(items.Count < maxItems)) { return; }

            items.Add(id);

            var item = GetItem(id);

            if (item == null) { return; }

            item.OnAdd(netIdentity);
        }

        [Command]
        void CmdRemoveItem(string id)
        {
            var item = GetItem(id);

            if (item != null)
                item.OnRemove(netIdentity);

            items.Remove(id);
        }

        [Command]
        void CmdChangeItem(int index)
        {
            activeItem = index;

            var item = GetItem(index);

            if (item == null) { return; }

            item.OnEquip(netIdentity);
        }

        [Command]
        void CmdUseActiveItem()
        {
            var item = GetItem(activeItem);

            if (item == null) { return; }
            
            item.OnUse(netIdentity);
        }
        #endregion // Commands

        #region Find Items
        List<SimpleItem> loadedItems = new List<SimpleItem>();

        public void Start() => loadedItems = Resources.LoadAll<SimpleItem>("Items").ToList();

        public SimpleItem GetItem(int id)
        {
            if (!(items.Count > id)) { return null; }

            return loadedItems.Find(i => i.id == items[id]);
        }

        public SimpleItem GetItem(string id) => loadedItems.Find(i => i.id == id);
        #endregion // Find Items

        void OnItemsUpdated(SyncList<string>.Operation op, int index, string oldItem, string newItem)
        {
            switch (op)
            {
                case SyncList<string>.Operation.OP_ADD:
                    onSyncItems?.Invoke(items.ToList().AsReadOnly(), activeItem);
                    break;
                case SyncList<string>.Operation.OP_INSERT:
                    break;
                case SyncList<string>.Operation.OP_REMOVEAT:
                    onRemoveItem?.Invoke(oldItem);
                    break;
                case SyncList<string>.Operation.OP_SET:
                    break;
                case SyncList<string>.Operation.OP_CLEAR:
                    break;
            }
        }

        void OnChangeItem(int oldItem, int newItem)
        {
            onSyncItems?.Invoke(items.ToList().AsReadOnly(), newItem);
        }
    }
}
