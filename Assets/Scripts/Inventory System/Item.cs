using Mirror;
using System;
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
        public string id;
        public int activeItem;

        public RemoveItem(string id, int activeItem)
        {
            this.id = id;
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
