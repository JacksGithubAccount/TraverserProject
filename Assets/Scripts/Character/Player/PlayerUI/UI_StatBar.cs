using TraverserProject;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_StatBar : MonoBehaviour
    {
        [SerializeField] protected Slider slider;
        protected RectTransform rectTransform;

        [Header("Bar Options")]
        [SerializeField] protected bool scaleBarLengthWithStats = true;
        [SerializeField] protected float widthScaleMultiplier = 1;

        [Header("Fill Color")]
        [SerializeField] protected Image fillImage;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void Start()
        {

        }

        public virtual void SetStat(int newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                PlayerUIManager.Singleton.playerUIHudManager.RefreshHUD();
            }
        }

        public void ToggleBarFillColor(bool isPoisoned)
        {
            if (fillImage == null)
                return;

            if (isPoisoned)
            {
                fillImage.color = WorldUtilityManager.Singleton.GetPoisonedColor();
            }
            else
            {
                fillImage.color = WorldUtilityManager.Singleton.GetRegularColor(); 
            }
        }
    }

}