using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace TraverserProject
{
    public class UI_Inventory_Category_Select_Arrow : UI_Arrow_Button, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler

    {
        public bool isPressed;
        private Coroutine heldButtonCoroutine;

        private readonly static float INITIAL_TIME_NEEDED = 0.1f;
        private float timer = 0;

        [Header("Increase/Decrease")]
        [SerializeField] bool isIncreaseArrow = true;
        [SerializeField] UI_Inventory_Item_Type_Scrollbar scrollbar;

        public void OnUpdateSelected(BaseEventData data)
        {
            if (isPressed)
            {
                heldButtonCoroutine = StartCoroutine(IncreaseOrDecreaseValue());
            }
            else
            {
                if (heldButtonCoroutine != null)
                {
                    StopCoroutine(heldButtonCoroutine);
                }
            }
        }
        public void OnPointerDown(PointerEventData data)
        {
            isPressed = true;
        }
        public void OnPointerUp(PointerEventData data)
        {
            isPressed = false;
        }

        private IEnumerator IncreaseOrDecreaseValue()
        {
            yield return new WaitForSeconds(1);

            while (isPressed)
            {
                timer += Time.deltaTime;
                if (timer > INITIAL_TIME_NEEDED)
                {
                    if (isIncreaseArrow)
                    {
                        scrollbar.IncrementSliderValue();
                    }
                    else
                    {
                        scrollbar.DecrementSliderValue();
                    }
                    timer = 0;
                }
                    

                yield return new WaitForSeconds(2);
            }
        }
    }
}
