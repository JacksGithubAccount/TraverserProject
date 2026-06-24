using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_ShopItemCategory : MonoBehaviour
    {
        public ShopCategory shopCategory;
        public Button button;
        public Image buttonImage;

        [Header("Storage or shop")]
        public bool isShopCategory = true;


        public void SetCategory()
        {
            //Resets every other buttons "selected" color before "selecting"a new one
            if (!isShopCategory)
            {
                
            }else
            {
                ResetButtonColor(PlayerUIManager.Singleton.playerUIShopManager.shopCategories);
                PlayerUIManager.Singleton.playerUIShopManager.shopCategory = shopCategory;
                PlayerUIManager.Singleton.playerUIShopManager.SortShopByCategory();
            }
                buttonImage.color = button.colors.selectedColor;
        }

        private void ResetButtonColor(UI_ShopItemCategory[] itemCatagories)
        {
            for (int i = 0; i < itemCatagories.Length; i++)
            {
                if (itemCatagories[i] == null)
                    continue;

                itemCatagories[i].buttonImage.color = itemCatagories[i].button.colors.normalColor;
            }            
        }

    }
}