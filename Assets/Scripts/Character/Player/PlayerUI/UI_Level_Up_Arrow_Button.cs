using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Level_Up_Arrow_Button : UI_Arrow_Button, IPointerEnterHandler, IPointerExitHandler
{
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        gameObject.SetActive(false);
    }
}
