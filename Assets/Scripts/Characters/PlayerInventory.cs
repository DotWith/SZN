using Mirror;
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
        /// The items the inventory contains
        /// </summary>
        internal SimpleItem[] itemArray;

        int maxItems;

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        internal int MaxItems
        {
            get { return maxItems; }
            set
            {
                itemArray = new SimpleItem[value];
                maxItems = value;
            }
        }

        /// <summary>
        /// The current item for this inventory
        /// </summary>
        [SyncVar(hook = nameof(OnItemChanged))]
        int activeItemSynced = -1;

        GameObject activeItemPrefab;

        void OnItemChanged(int _Old, int _New)
        {
            if (itemArray.Length < _New) { return; }

            SimpleItem item = itemArray[_New];

            if (activeItemPrefab != null)
                Destroy(activeItemPrefab);
            else if (item.modelPrefab != null)
            {
                activeItemPrefab = Instantiate(item.modelPrefab, itemSpawn);
                activeItemPrefab.layer = viewModelLayer; // TODO: Change this to a LayerMask
            }
        }

        public override void OnStartAuthority()
        {
            MaxItems = 2;

            enabled = true;

            player.Controls.Player.Inventory1.performed += ctx => CmdChangeActiveItem(0);
            player.Controls.Player.Inventory2.performed += ctx => CmdChangeActiveItem(1);
        }

        public void AddItem(SimpleItem item) => itemArray.SetValue(item, GetAvailableSlotIndex());

        [Command]
        public void CmdChangeActiveItem(int newIndex) => activeItemSynced = newIndex;

        /// <summary>
        /// Ireatate through each item until we find a available slot to place our item
        /// </summary>
        /// <returns></returns>
        int GetAvailableSlotIndex()
        {
            for (int i = 0; i < MaxItems; i++)
            {
                if (itemArray.GetValue(i) != null)
                {
                    return i;
                }
            }

            return 0; // IDK how anyone would get here
        }
    }

    /// <summary>
    /// This can be extended for other item types.
    /// </summary>
    [System.Serializable]
    public struct SimpleItem
    {
        public string name;
        [TextArea]
        public string description;
        public Sprite icon;
        public GameObject modelPrefab;
    }
}
