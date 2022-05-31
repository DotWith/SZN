using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.Events;

namespace Com.Dot.SZN.UI.TabSystem
{
    [RequireComponent(typeof(TMP_Text))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler
    {
        [SerializeField] ColorBlock colors = ColorBlock.defaultColorBlock;

        TabGroup tabGroup;
        TMP_Text text;

        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;

        public void Start()
        {
            text = GetComponent<TMP_Text>();

            tabGroup = transform.GetComponentInParent<TabGroup>();
            tabGroup.Subscribe(this);
        }

        public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(this);

        public void OnPointerClick(PointerEventData eventData) => tabGroup.OnTabSelected(this);

        public void OnPointerDown(PointerEventData eventData) => tabGroup.OnTabExit(this);

        public void Hover()
        {
            text.color = colors.highlightedColor;
        }

        public void Idle()
        {
            text.color = colors.normalColor;
        }

        public void Select()
        {
            onTabSelected?.Invoke();

            text.color = colors.selectedColor;
        }

        public void Deselect()
        {
            onTabDeselected?.Invoke();

            text.color = colors.normalColor;
        }
    }
}
