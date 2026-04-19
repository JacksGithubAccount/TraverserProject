using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUIInventoryManager : PlayerUIMenu
    {
        [Header("Inventory")]
        [SerializeField] GameObject inventoryWindow;
        [SerializeField] GameObject inventorySlotPrefab;
        [SerializeField] Transform inventoryContentWindow;
        [SerializeField] Item currentlySelectedItem;
        [HideInInspector] List<GameObject> inventorySlotPrefabs = new List<GameObject>();


        public override void OpenMenu()
        {
            base.OpenMenu();

            ToggleInventoryButtons(true);
            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //RefreshMenu();

        }

        public void ToggleInventoryButtons(bool isEnabled)
        {
            foreach (var gameObject in inventorySlotPrefabs)
            {
                gameObject.SetActive(isEnabled);
            }

        }

        public void LoadInventory()
        {
            ClearInventorySlotPrefabs();
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {
                WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

                if (weapon != null)
                    weaponsInInventory.Add(weapon);
            }

            if (weaponsInInventory.Count <= 0)
            {
                inventoryWindow.SetActive(false);
                ToggleInventoryButtons(true);
                //RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < weaponsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(inventorySlotPrefab, inventoryContentWindow);
                UI_EquipmentInventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
                inventorySlot.AddItem(weaponsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        private void ClearInventorySlotPrefabs()
        {
            foreach (GameObject item in inventorySlotPrefabs)
            {
                Destroy(item);
            }
            inventorySlotPrefabs.Clear();
        }

    }

}