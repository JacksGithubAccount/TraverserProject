using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Singleton;

        //Main Menu 
        [Header("Main Menu Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;
        [SerializeField] GameObject titleScreenCharacterCreationMenu;

        [Header("Main Menu Buttons")]
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button MainMenuLoadGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("Main Menu Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkayButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        //character creation menu
        [Header("Character Creation Main Panel Buttons")]
        [SerializeField] Button characterNameButton;
        [SerializeField] Button characterClassButton;
        [SerializeField] Button startGameButton;

        [Header("Character Creation Secondary Panel Menus")]
        [SerializeField] GameObject characterClassMenu;

        [Header("Character Creation Class Panel Buttons")]
        [SerializeField] Button[] characterClassButtons;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;


        [Header("Classes")]
        public CharacterClass[] startingClasses;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void AttemptToCreateNewCharacter()
        {
            if (WorldSaveGameManager.Singleton.HasFreeCharacterSlot())
            {
                OpenCharacterCreationMenu();
            }
            else
            {
                DisplayNoFreeCharacterSlotsPopUp();
            }
        }

        public void StartNewGame()
        {
            WorldSaveGameManager.Singleton.AttemptToCreateNewGame();

        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);

            MainMenuLoadGameButton.Select();
        }

        public void OpenCharacterCreationMenu()
        {
            titleScreenCharacterCreationMenu.SetActive(true);
        }

        public void CloseCharacterCreationMenu()
        {
            titleScreenCharacterCreationMenu.SetActive(false);
        }

        public void OpenChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButtons(false);
            characterClassMenu.SetActive(true);
            if (characterClassButtons.Length > 0)
            {
                characterClassButtons[0].Select();
                characterClassButtons[0].OnSelect(null);
            }
        }

        public void CloseChooseCharacterClassSubMenu()
        {
            ToggleCharacterCreationScreenMainMenuButtons(true);
            characterClassMenu.SetActive(false);
            characterClassButton.Select();
            characterClassButton.OnSelect(null);
        }

        private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
        {
            characterNameButton.enabled = status;
            characterClassButton.enabled = status;
            startGameButton.enabled = status;
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }
        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }
        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT; ;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.Singleton.DeleteGame(currentSelectedSlot);
            //refreshes screeen to make load slots reload
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }
        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }

        public void SelectClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
            CloseChooseCharacterClassSubMenu();
        }

        public void PreviewClass(int classID)
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
        }

        public void SetCharacterClass(PlayerManager player, int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith, int luck
            , WeaponItem[] mainHandWeapons, WeaponItem[] offHandWeapons, HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment
            , LegEquipmentItem legEquipment, HandEquipmentItem handEquipment, QuickSlotItem[] quickSlotItems)
        {
            player.playerNetworkManager.vitality.Value = vitality;
            player.playerNetworkManager.endurance.Value = endurance;
            player.playerNetworkManager.mind.Value = mind;
            player.playerNetworkManager.strength.Value = strength;
            player.playerNetworkManager.dexterity.Value = dexterity;
            player.playerNetworkManager.intelligence.Value = intelligence;
            player.playerNetworkManager.faith.Value = faith;
            player.playerNetworkManager.luck.Value = luck;

            player.playerInventoryManager.weaponsInRightHandSlots[0] = Instantiate(mainHandWeapons[0]);
            player.playerInventoryManager.weaponsInRightHandSlots[1] = Instantiate(mainHandWeapons[1]);
            player.playerInventoryManager.weaponsInRightHandSlots[2] = Instantiate(mainHandWeapons[2]);
            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[0].itemID;
			
			player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
            player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];
            player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[0].itemID;
		
			if (headEquipment != null)
            {
                HeadEquipmentItem equipment = Instantiate(headEquipment);
                player.playerInventoryManager.headEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.headEquipment = null;
            }

            if (bodyEquipment != null)
            {
                BodyEquipmentItem equipment = Instantiate(bodyEquipment);
                player.playerInventoryManager.bodyEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.bodyEquipment = null;
            }

            if (handEquipment != null)
            {
                HandEquipmentItem equipment = Instantiate(handEquipment);
                player.playerInventoryManager.handEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.handEquipment = null;
            }

            if (legEquipment != null)
            {
                LegEquipmentItem equipment = Instantiate(legEquipment);
                player.playerInventoryManager.legEquipment = equipment;
            }
            else
            {
                player.playerInventoryManager.legEquipment = null;
            }
            player.playerEquipmentManager.EquipArmor();

            player.playerInventoryManager.quickSlotItemIndex = 0;

            if (quickSlotItems[0] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(quickSlotItems[0]);
            if (quickSlotItems[1] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(quickSlotItems[1]);
            if (quickSlotItems[2] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(quickSlotItems[2]);
            player.playerEquipmentManager.LoadQuickSlotItemEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);

        }
    }
}