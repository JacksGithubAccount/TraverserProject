using TraverserProject;
using UnityEngine;
using UnityEngine.UI;

public class UI_Craft_Amount_Slider : MonoBehaviour
{
    private Slider slider;

    [Header("Increase / Decrease Arrows")]
    public GameObject decreaseArrow;
    public GameObject increaseArrow;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void EnableArrows()
    {
        if (decreaseArrow != null && increaseArrow != null)
        {
            decreaseArrow.SetActive(true);
            increaseArrow.SetActive(true);

            if (slider != null)
            {
                slider.Select();
                slider.OnSelect(null);
            }
        }
    }

    public void DisableArrows()
    {
        UI_Level_Up_Arrow_Button dbutton = decreaseArrow.GetComponent<UI_Level_Up_Arrow_Button>();
        UI_Level_Up_Arrow_Button ibutton = increaseArrow.GetComponent<UI_Level_Up_Arrow_Button>();

        if (dbutton == null || ibutton == null)
            return;


        if (!dbutton.mouseOverArrow)
        {
            decreaseArrow.SetActive(false);
        }
        if (!ibutton.mouseOverArrow)
        {
            increaseArrow.SetActive(false);
        }

    }

    public void IncrementSliderValue()
    {
        if (slider.value == slider.maxValue)
        {
            slider.value = slider.minValue;
        }
        else
        {
            slider.value++;
        }

    }

    public void DecrementSliderValue()
    {
        if (slider.value == slider.minValue)
        {
            slider.value = slider.maxValue;
        }
        else
        {
            slider.value--;
        }
    }
}
