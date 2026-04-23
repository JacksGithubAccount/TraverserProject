using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace TraverserProject
{
    public class UI_Inventory_Item_Type_Scrollbar : MonoBehaviour
    {
        [SerializeField] float scrollAmount = 0.0623f;
        Scrollbar scrollbar;
        [SerializeField] ScrollRect scrollRect;

        [Header("Increase / Decrease Arrows")]
        public GameObject decreaseArrow;
        public GameObject increaseArrow;

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
        }

        public void IncrementSliderValue()
        {            

            int slotnumber = (int)PlayerUIManager.Singleton.playerUIInventoryManager.currentSelectedInventoryCategorySelectSlot + 1;
            if (slotnumber > 19)
            {
                slotnumber = 0;
            }

            PlayerUIManager.Singleton.playerUIInventoryManager.ChangeSelectedInventoryCategorySelectSlot(slotnumber);

            if (scrollbar.value >= 1)
                scrollbar.value = 0;
            else
            {

                slotnumber = Mathf.Clamp(slotnumber - 1, 0, PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);

                                //scrollbar.value += scrollAmount;
                float i = ((float)slotnumber / PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);
                scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, 1f - i, Time.deltaTime / 0.2f);
            }
        }

        public void DecrementSliderValue()
        {
            if (scrollbar.value <= 0)
                scrollbar.value = 1;
            else
                scrollbar.value -= scrollAmount;

            int slotnumber = (int)PlayerUIManager.Singleton.playerUIInventoryManager.currentSelectedInventoryCategorySelectSlot - 1;
            if (slotnumber < 0)
            {
                slotnumber = 19;
            }
            PlayerUIManager.Singleton.playerUIInventoryManager.ChangeSelectedInventoryCategorySelectSlot(slotnumber);
            
        }
    }
}