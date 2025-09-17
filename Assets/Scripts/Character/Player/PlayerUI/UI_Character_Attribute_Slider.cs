using UnityEngine;

namespace TraverserProject
{

    public class UI_Character_Attribute_Slider : MonoBehaviour
    {
        [SerializeField] CharacterAttribute sliderAttribute;

        public void SetCurrentSelectedAttribute()
        {
            PlayerUIManager.Singleton.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
        }

    }
}