using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.InventorySystem
{
    [DisallowMultipleComponent]
    public class Inventory : MonoBehaviour
    {
        #region Voids
        /// <summary>Add said item from this inventory</summary>
        /// <param name="id"></param>
        public void AddItem(string id)
        {
            var msg = new Item(id);
            NetworkServer.SendToAll(msg);
        }

        /// <summary>Remove said item from this inventory</summary>
        /// <param name="index"></param>
        public void RemoveItem(int index)
        {
            var msg = new RemoveItem(index);
            NetworkServer.SendToAll(msg);
        }

        /// <summary>Change to this index</summary>
        /// <param name="index"></param>
        public void ChangeItem(int index)
        {
            var msg = new ChangeItem(index);
            NetworkServer.SendToAll(msg);
        }

        /// <summary>Use said item at that index</summary>
        /// <param name="index"></param>
        public void UseItem(int index)
        {
            var msg = new UseItem(index);
            NetworkServer.SendToAll(msg);
        }
        #endregion // Voids
    }
}
