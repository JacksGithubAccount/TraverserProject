using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_CraftingItemInformationSlot : MonoBehaviour
    {
        public Image itemIcon;
        public Image highlightIcon;
        [SerializeField] public Item currentItem;
        [SerializeField] public ItemCategory currentItemCategory;

        public void AddItem(Item item)
        {
            if (item == null)
            {
                itemIcon.enabled = false;
                return;
            }

            itemIcon.enabled = true;

            currentItem = item;
            itemIcon.sprite = item.itemIcon;
        }

        public void AddItemCategory(ItemCategory itemCategory)
        {
            itemIcon.enabled = true;

            currentItemCategory = itemCategory;
            if(WorldItemDatabase.Singleton.itemCategoryIcons.Find(x => x.name == itemCategory.ToString()))
                itemIcon.sprite = WorldItemDatabase.Singleton.itemCategoryIcons.Find(x => x.name == itemCategory.ToString());

        }
    }
}

