using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.UI.TabSystem
{
    public class TabGroup : MonoBehaviour
    {
        public List<TabButton> tabButtons;
        public List<GameObject> objectsToSwap;
        
        TabButton selectedTab;

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);

            if (tabButtons.Count == 1)
                OnTabSelected(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                button.Hover();
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            if (selectedTab != null)
            {
                selectedTab.Deselect();
            }

            selectedTab = button;

            selectedTab.Select();

            ResetTabs();
            int index = button.transform.GetSiblingIndex();
            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                objectsToSwap[i].SetActive(i == index);
            }
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab != null && button == selectedTab)
                    continue;

                button.Idle();
            }
        }
    }
}
