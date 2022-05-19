using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory
    {
        /// <summary>
        /// The items the inventory contains
        /// </summary>
        public SimpleItem[] items;

        /// <summary>
        /// The max items for this inventory
        /// </summary>
        int maxItems;

        public PlayerInventory(int maxItems)
        {
            this.maxItems = maxItems;
            items = new SimpleItem[maxItems];
        }

        public void AddItem(SimpleItem item)
        {
            items.SetValue(item, GetAvailableSlotIndex());
        }

        /// <summary>
        /// Ireatate through each item until we find a available slot to place our item
        /// </summary>
        /// <returns></returns>
        int GetAvailableSlotIndex()
        {
            for (int i = 0; i < maxItems; i++)
            {
                if (items.GetValue(i) != null)
                {
                    return i;
                }
            }

            return 0; // IDK how anyone would get here
        }
    }
}

[System.Serializable]
public struct SimpleItem
{
    public string name;
    [TextArea]
    public string description;
    public Sprite icon;
}
