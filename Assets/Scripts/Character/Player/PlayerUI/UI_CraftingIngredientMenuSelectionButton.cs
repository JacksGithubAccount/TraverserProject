using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TraverserProject
{
    public class UI_CraftingIngredientMenuSelectionButton : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemAmountText;
        [SerializeField] public ItemCategory currentItemCategory;

        public void AddItemCategory(ItemCategory itemCategory, int amount)
        {
            itemIcon.enabled = true;

            currentItemCategory = itemCategory;

                itemAmountText.text = "x" + amount;
                itemNameText.text = itemCategory.ToString();
            

        }
        public void SelectSlot()
        {
            highlightIcon.enabled = true;            
        }

        public void DeselectSlot()
        {
            highlightIcon.enabled = false;
        }

        public void SelectItemCategory()
        {

        }
    }
}
