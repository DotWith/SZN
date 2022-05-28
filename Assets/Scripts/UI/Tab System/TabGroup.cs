using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.UI.TabSystem
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public List<GameObject> objectsToSwap;
        public TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {

        }

        public void OnTabExit(TabButton button)
        {

        }

        public void OnTabSelected(TabButton button)
        {
            selectedTab = button;

            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(i == index);
            }
        }
    }
}
