using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class UI_ShopItemCategory : MonoBehaviour
    {
        public ShopCategory shopCategory;
        public Button button;
        public Image buttonImage;


        public void SetCategory()
        {
            //Resets every other buttons "selected" color before "selecting"a new one
            for (int i = 0; i < PlayerUIManager.Singleton.playerUIShopManager.shopCategories.Length; i++)
            {
                if (PlayerUIManager.Singleton.playerUIShopManager.shopCategories[i] == null)
                    continue;

                PlayerUIManager.Singleton.playerUIShopManager.shopCategories[i].buttonImage.color = PlayerUIManager.Singleton.playerUIShopManager.shopCategories[i].button.colors.normalColor;
            }
            PlayerUIManager.Singleton.playerUIShopManager.shopCategory = shopCategory;
            PlayerUIManager.Singleton.playerUIShopManager.SortShopByCategory();
            buttonImage.color = button.colors.selectedColor;
        }

    }
}