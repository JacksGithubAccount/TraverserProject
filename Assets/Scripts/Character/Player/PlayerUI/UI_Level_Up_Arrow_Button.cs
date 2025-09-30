using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Level_Up_Arrow_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOverArrow = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverArrow = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverArrow = false;
        gameObject.SetActive(false);
    }
}
