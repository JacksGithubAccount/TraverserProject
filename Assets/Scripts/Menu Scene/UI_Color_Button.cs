using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_Color_Button : MonoBehaviour
    {
        [Header("Colors")]
        private float redValue;
        private float greenValue;
        private float blueValue;

        [SerializeField] Image colorImage;

        private void Awake()
        {
            redValue = colorImage.color.r * 255;
            greenValue = colorImage.color.g * 255;
            blueValue = colorImage.color.b * 255;
        }

        public void SetSliderValuesToColor()
        {
            TitleScreenManager.Singleton.SetRedColorSlider(redValue);
            TitleScreenManager.Singleton.SetGreenColorSlider(greenValue);
            TitleScreenManager.Singleton.SetBlueColorSlider(blueValue);
            TitleScreenManager.Singleton.PreviewHairColor();
        }

        public void ConfirmColor()
        {
            TitleScreenManager.Singleton.CloseChooseHairColorSubMenu();
        }
    }
}