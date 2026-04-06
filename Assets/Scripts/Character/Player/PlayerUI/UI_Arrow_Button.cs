using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Arrow_Button : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOverArrow = false;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverArrow = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        mouseOverArrow = false;
    }
}
