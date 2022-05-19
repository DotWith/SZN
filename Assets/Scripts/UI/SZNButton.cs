using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class SZNButton
    : Selectable,
    IPointerClickHandler,
    ISubmitHandler,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    [SerializeField] Image underline;

    public ButtonClickedEvent OnClick = new ButtonClickedEvent();

    protected override void Start()
    {
        base.Start();
        underline.gameObject.SetActive(false);
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;

        UISystemProfilerApi.AddMarker("Button.onClick", this);
        OnClick.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Press();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        underline.gameObject.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        underline.gameObject.SetActive(false);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        underline.gameObject.SetActive(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        underline.gameObject.SetActive(false);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Press();

        // if we get set disabled during the press
        // don't run the coroutine.
        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
