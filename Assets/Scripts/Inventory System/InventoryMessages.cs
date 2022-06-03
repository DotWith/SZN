using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.InventorySystem
{
    public struct Item : NetworkMessage
    {
        public string id;
        public GameObject obj;

        public Item(string id, GameObject obj)
        {
            this.id = id;
            this.obj = obj;
        }
    }

    public struct RemoveItem : NetworkMessage
    {
        public int activeItem;

        public RemoveItem(int activeItem)
        {
            this.activeItem = activeItem;
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
        public int activeItem;

        public UseItem(int activeItem)
        {
            this.activeItem = activeItem;
        }
    }
}
