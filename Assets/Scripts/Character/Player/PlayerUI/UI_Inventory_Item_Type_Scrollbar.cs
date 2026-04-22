using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TraverserProject
{
    public class UI_Inventory_Item_Type_Scrollbar : MonoBehaviour
    {
        [SerializeField] float scrollAmount = 0.0623f;
        Scrollbar scrollbar;

        [Header("Increase / Decrease Arrows")]
        public GameObject decreaseArrow;
        public GameObject increaseArrow;

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
        }

        public void IncrementSliderValue()
        {
            scrollbar.value += scrollAmount;
        }

        public void DecrementSliderValue()
        {
            scrollbar.value -= scrollAmount;
        }
    }
}