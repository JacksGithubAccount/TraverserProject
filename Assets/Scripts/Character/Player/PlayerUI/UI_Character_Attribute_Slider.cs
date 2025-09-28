using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_Character_Attribute_Slider : MonoBehaviour
    {
        [SerializeField] CharacterAttribute sliderAttribute;
        private Slider slider;

        [Header("Increase / Decrease Arrows")]
        public GameObject decreaseArrow;
        public GameObject increaseArrow;


        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public void SetCurrentSelectedAttribute()
        {
            PlayerUIManager.Singleton.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
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

            if (dbutton == null)
                return;
            
            if (!dbutton.mouseOverArrow)
                decreaseArrow.SetActive(false);

            UI_Level_Up_Arrow_Button ibutton = increaseArrow.GetComponent<UI_Level_Up_Arrow_Button>();

            if (ibutton == null)
                return;

            if (!ibutton.mouseOverArrow)
                increaseArrow.SetActive(false);
        }

        public void IncrementSliderValue()
        {
            slider.value++;
        }

        public void DecrementSliderValue()
        {
            slider.value--;
        }

    }
}