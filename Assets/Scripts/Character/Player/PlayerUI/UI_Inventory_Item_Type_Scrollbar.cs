using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

namespace TraverserProject
{
    public class UI_Inventory_Item_Type_Scrollbar : MonoBehaviour
    {
        [SerializeField] float scrollAmount = 0.0623f;
        Scrollbar scrollbar;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] RectTransform viewport;

        [Header("Increase / Decrease Arrows")]
        public GameObject decreaseArrow;
        public GameObject increaseArrow;

        [Header("Inventory or Storage")]
        public bool isInventory = true;

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
        }

        public void IncrementSliderValue()
        {
            int slotnumber = 0;
            if (isInventory)
            {
                slotnumber = (int)PlayerUIManager.Singleton.playerUIInventoryManager.currentSelectedInventoryCategorySelectSlot + 1;
                if (slotnumber > 19)
                {
                    slotnumber = 0;
                }

                PlayerUIManager.Singleton.playerUIInventoryManager.ChangeSelectedInventoryCategorySelectSlot(slotnumber);
            }
            if (!IsSelectedPrefabInViewOfViewport(slotnumber))
            {
                if (slotnumber== 0)
                    scrollbar.value = 0;
                else
                {
                    scrollbar.value += scrollAmount;
                    //slotnumber = Mathf.Clamp(slotnumber + 1, 0, PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);
                    //float i = ((float)slotnumber / PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);
                    //scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, 1f - i, Time.deltaTime / 0.2f);
                }
            }
        }

        public void DecrementSliderValue()
        {
            int slotnumber = (int)PlayerUIManager.Singleton.playerUIInventoryManager.currentSelectedInventoryCategorySelectSlot - 1;
            if (slotnumber < 0)
            {
                slotnumber = 19;
            }
            PlayerUIManager.Singleton.playerUIInventoryManager.ChangeSelectedInventoryCategorySelectSlot(slotnumber);


            if (!IsSelectedPrefabInViewOfViewport(slotnumber))
            {
                if (slotnumber == 19)
                    scrollbar.value = 1;
                else
                {
                    scrollbar.value -= scrollAmount;
                    //slotnumber = Mathf.Clamp(slotnumber - 1, 0, PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);
                    //float i = ((float)slotnumber / PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs.Count - 1);
                    //scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, -(1f - i), Time.deltaTime / 0.2f);
                }
            }
        }

        private bool IsSelectedPrefabInViewOfViewport(int slotNumber)
        {
            RectTransform rect = PlayerUIManager.Singleton.playerUIInventoryManager.inventoryCategorySelectSlotPrefabs[slotNumber].GetComponent<RectTransform>();
            Vector2 v = rect.position;
            bool inView = RectTransformUtility.RectangleContainsScreenPoint(viewport, v);
            return inView;
        }
    }
}