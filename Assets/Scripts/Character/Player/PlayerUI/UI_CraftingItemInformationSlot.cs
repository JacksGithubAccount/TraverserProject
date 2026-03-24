using Steamworks.Ugc;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_CraftingItemInformationSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI itemAmountText;
        [SerializeField] public Item currentItem;
        [SerializeField] public ItemCategory currentItemCategory;

        public void AddItem(Item item, int amount)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
            itemAmountText.text = "x" + amount;
            itemNameText.text = item.name;
        }

        public void AddItemCategory(ItemCategory itemCategory, int amount)
        {
            itemIcon.enabled = true;

            currentItemCategory = itemCategory;
            if (WorldItemDatabase.Singleton.itemCategoryIcons.Find(x => x.name == itemCategory.ToString()))
            {
                itemIcon.sprite = WorldItemDatabase.Singleton.itemCategoryIcons.Find(x => x.name == itemCategory.ToString());
                itemAmountText.text = "x" + amount;
                itemNameText.text = itemCategory.ToString();
            }

        }
    }
}

