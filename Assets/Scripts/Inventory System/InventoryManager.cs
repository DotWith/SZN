using Com.Dot.SZN.ScriptableObjects;
using Mirror;
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

        int activeItem;

        public void Awake()
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetupClient()
        {
            NetworkClient.RegisterHandler<Item>(OnItem);
            NetworkClient.RegisterHandler<RemoveItem>(OnRemoveItem);
            NetworkClient.RegisterHandler<ChangeItem>(OnChangeItem);
            NetworkClient.RegisterHandler<UseItem>(OnUseItem);
        }

        #region Client Callbacks
        /// <summary>Add said item from this inventory</summary>
        /// <param name="id"></param>
        [ClientCallback]
        public void AddItem(string id)
        {
            var msg = new Item(id);
            NetworkServer.SendToAll(msg);
        }

        /// <summary>Remove said item from this inventory</summary>
        /// <param name="id"></param>
        [ClientCallback]
        public void RemoveItem(string id)
        {
            var msg = new RemoveItem(id, activeItem);
            NetworkServer.SendToAll(msg);
        }

        [ClientCallback]
        public void ChangeItem(int index)
        {
            var msg = new ChangeItem(index);
            NetworkServer.SendToAll(msg);
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
            if (items.ToList().Count >= maxItems)
                return;

            items.AddValue(activeItem, msg.id);
        }

        void OnRemoveItem(RemoveItem msg)
        {
            // Checks if the item is the same
            if (activeItem.Equals(msg.activeItem))
                return;

            items.DeleteValue(activeItem);
        }

        void OnChangeItem(ChangeItem msg)
        {
            // Checks if the item we want to change to exists
            if (items.ToList().Count < msg.index)
                return;

            activeItem = msg.index;
        }

        void OnUseItem(UseItem msg)
        {
            // Checks if we are selecting that item
            if (!activeItem.Equals(msg.activeItem))
                return;

            SimpleItem item = Resources.LoadAll<SimpleItem>("Items").ToList().Find(i => i.id == items.GetValue(activeItem));

            if (item == null)
                return;

            Debug.Log($"ItemUse: {item.name}");
            item.Use();
        }
        #endregion // Registered Handles
    }
}
