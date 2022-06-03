using Com.Dot.SZN.Characters;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.InventorySystem
{
    public struct Item : NetworkMessage
    {
        public string id;

        public Item(string id)
        {
            this.id = id;
        }
    }

    public struct RemoveItem : NetworkMessage
    {
        // TODO: Change this to a string
        public int index;

        public RemoveItem(int index)
        {
            this.index = index;
        }
    }

    public struct ChangeItem : NetworkMessage
    {
        public int index;

        public ChangeItem(int index)
        {
            this.index = index;
        }
    }

    public struct UseItem : NetworkMessage
    {
        public int index;

        public UseItem(int index)
        {
            this.index = index;
        }
    }
}
