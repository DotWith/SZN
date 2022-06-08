using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Com.Dot.SZN.InventorySystem
{
    [DisallowMultipleComponent]
    public class InventoryManager : MonoBehaviour
    {
        /*[Header("Inventory Settings")]

        [FormerlySerializedAs("m_MaxInventoryItems")]
        [SerializeField]
        [Tooltip("Maximum number of items in all inventories")]
        public int maxItems = 10;

        public static InventoryManager singleton { get; private set; }

        InventoryList<string> items = new InventoryList<string>();
        List<SimpleItem> loadedItems = new List<SimpleItem>();

        int activeItem;

        public void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            loadedItems = Resources.LoadAll<SimpleItem>("Items").ToList();
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
        }

        #region Registered Handles
        void OnItem(Item msg)
        {
            // Checks if we are going above the limit
            if (items.GetCount() >= maxItems)
                return;

            items.AddValue(activeItem, msg.id);

            SimpleItem item = GetItem(activeItem);

            if (item == null)
                return;

            item.OnAdd();
        }
        
        void OnRemoveItem(RemoveItem msg)
        {
            // Checks if the item is the same
            if (activeItem.Equals(msg.index))
                return;

            SimpleItem item = GetItem(activeItem);

            if (item == null)
                return;

            item.OnRemove();

            items.DeleteValue(activeItem);
        }

        void OnChangeItem(ChangeItem msg)
        {
            // Checks if the item we want to change to exists
            if (items.GetCount() < msg.index)
                return;

            activeItem = msg.index;

            SimpleItem item = GetItem(activeItem);

            if (item == null)
                return;

            item.OnEquip();
        }

        void OnUseItem(UseItem msg)
        {
            // Checks if we are selecting that item
            if (!activeItem.Equals(msg.index))
                return;

            SimpleItem item = GetItem(activeItem);

            if (item == null)
                return;

            item.OnUse();
        }
        #endregion // Registered Handles

        public SimpleItem GetItem(int id) => loadedItems.Find(i => i.id == items.GetValue(id));*/
    }
}
