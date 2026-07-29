using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

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
        [SerializeField] Button characterHairButton;
        [SerializeField] Button characterHairColorButton;
        [SerializeField] Button characterSexButton;
        [SerializeField] TextMeshProUGUI characterSexText;
        [SerializeField] Button startGameButton;

        [Header("Character Creation Secondary Panel Menus")]
        [SerializeField] GameObject characterClassMenu;
        [SerializeField] GameObject characterHairMenu;
        [SerializeField] GameObject characterHairColorMenu;
        [SerializeField] GameObject characterNameMenu;
        [SerializeField] TMP_InputField characterNameInputField;

        [Header("Character Creation Class Panel Buttons")]
        [SerializeField] Button[] characterClassButtons;
        [SerializeField] Button[] characterHairButtons;
        [SerializeField] Button[] characterHairColorButtons;

        [Header("Color Sliders")]
        [SerializeField] Slider redSlider;
        [SerializeField] Slider greenSlider;
        [SerializeField] Slider blueSlider;

        [Header("Character Profile Icons")]
        public Image[] characterProfileIcons;

        [Header("Hidden Gear")]
        private HeadEquipmentItem hiddenHelmet;

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
            WorldGameSessionManager.Singleton.StartGameAsHost();
            CharacterProfileIconMaker.Singleton.CreateAllProfileIcons();
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

        public void ToggleBodyType()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.isMale.Value = !player.playerNetworkManager.isMale.Value;

            if (player.playerNetworkManager.isMale.Value)
            {
                characterSexText.text = "MALE";
            }
            else
            {
                characterSexText.text = "FEMALE";
            }
        }

        public void OpenTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(true);

        }

        public void CloseTitleScreenMainMenu()
        {
            titleScreenMainMenu.SetActive(false);
        }

        public void OpenCharacterCreationMenu()
        {
            CloseTitleScreenMainMenu();

            titleScreenCharacterCreationMenu.SetActive(true);

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerBodyManager.ToggleBodyType(true);
        }

        public void CloseCharacterCreationMenu()
        {
            titleScreenCharacterCreationMenu.SetActive(false);

            OpenTitleScreenMainMenu();
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

        public void OpenChooseHairStyleSubMenu()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            ToggleCharacterCreationScreenMainMenuButtons(false);
            characterHairMenu.SetActive(true);
            if (characterHairButtons.Length > 0)
            {
                characterHairButtons[0].Select();
                characterHairButtons[0].OnSelect(null);
            }

            if (player.playerInventoryManager.headEquipment != null)
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmor();
        }

        public void CloseChooseHairStyleSubMenu()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            ToggleCharacterCreationScreenMainMenuButtons(true);
            characterHairMenu.SetActive(false);
            characterHairButton.Select();
            characterHairButton.OnSelect(null);

            if (hiddenHelmet != null)
                player.playerInventoryManager.headEquipment = hiddenHelmet;

            player.playerEquipmentManager.EquipArmor();
        }

        public void OpenChooseHairColorSubMenu()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            ToggleCharacterCreationScreenMainMenuButtons(false);
            characterHairColorMenu.SetActive(true);
            if (characterHairButtons.Length > 0)
            {
                characterHairColorButtons[0].Select();
                characterHairColorButtons[0].OnSelect(null);
            }

            if (player.playerInventoryManager.headEquipment != null)
                hiddenHelmet = Instantiate(player.playerInventoryManager.headEquipment);

            player.playerInventoryManager.headEquipment = null;
            player.playerEquipmentManager.EquipArmor();
        }

        public void CloseChooseHairColorSubMenu()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            ToggleCharacterCreationScreenMainMenuButtons(true);
            characterHairColorMenu.SetActive(false);
            characterHairColorButton.Select();
            characterHairColorButton.OnSelect(null);

            if (hiddenHelmet != null)
                player.playerInventoryManager.headEquipment = hiddenHelmet;

            player.playerEquipmentManager.EquipArmor();
        }

        public void OpenChooseNameSubMenu()
        {
            //PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            ToggleCharacterCreationScreenMainMenuButtons(false);

            characterNameButton.gameObject.SetActive(false);
            characterNameMenu.SetActive(true);

            characterNameInputField.Select();

        }

        public void CloseChooseNameSubMenu()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            PlayerManager playertest = PlayerUIManager.Singleton.localPlayer;


            ToggleCharacterCreationScreenMainMenuButtons(true);

            characterNameMenu.SetActive(false);
            characterNameButton.gameObject.SetActive(true);

            characterNameButton.Select();

            player.playerNetworkManager.characterName.Value = characterNameInputField.text;
        }

        private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
        {
            characterNameButton.enabled = status;
            characterClassButton.enabled = status;
            characterHairButton.enabled = status;
            characterHairColorButton.enabled = status;
            characterSexButton.enabled = status;
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
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
            CloseChooseCharacterClassSubMenu();
        }

        public void PreviewClass(int classID)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            if (startingClasses.Length <= 0)
                return;

            startingClasses[classID].SetClass(player);
        }

        public void SetCharacterClass(PlayerManager player, int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith, int luck
            , WeaponItem[] mainHandWeapons, WeaponItem[] offHandWeapons, HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment
            , LegEquipmentItem legEquipment, HandEquipmentItem handEquipment, QuickSlotItem[] quickSlotItems, SpellItem[] spells, AccessoryEquipmentItem[] accessories, List<Item> inventory)
        {
            hiddenHelmet = null;

            player.playerNetworkManager.vigor.Value = vitality;
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
            player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[0].itemID;
            player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlots[0];

            player.playerInventoryManager.weaponsInLeftHandSlots[0] = Instantiate(offHandWeapons[0]);
            player.playerInventoryManager.weaponsInLeftHandSlots[1] = Instantiate(offHandWeapons[1]);
            player.playerInventoryManager.weaponsInLeftHandSlots[2] = Instantiate(offHandWeapons[2]);
            player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[0].itemID;
            player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[0];

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

            if (accessories[0] != null)
                player.playerInventoryManager.accessoryEquipment[0] = Instantiate(accessories[0]);
            if (accessories[1] != null)
                player.playerInventoryManager.accessoryEquipment[1] = Instantiate(accessories[1]);
            if (accessories[2] != null)
                player.playerInventoryManager.accessoryEquipment[2] = Instantiate(accessories[2]);
            if (accessories[3] != null)
                player.playerInventoryManager.accessoryEquipment[3] = Instantiate(accessories[3]);

            player.playerInventoryManager.quickSlotItemIndex = 0;

            if (quickSlotItems[0] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[0] = Instantiate(quickSlotItems[0]);
            if (quickSlotItems[1] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[1] = Instantiate(quickSlotItems[1]);
            if (quickSlotItems[2] != null)
                player.playerInventoryManager.quickSlotItemsInQuickSlots[2] = Instantiate(quickSlotItems[2]);
            player.playerEquipmentManager.LoadQuickSlotItemEquipment(player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex]);

            player.playerInventoryManager.quickSlotSpellIndex = 0;

            if (spells[0] != null)
                player.playerInventoryManager.spellItemsInQuickSlots[0] = Instantiate(spells[0]);
            if (spells[1] != null)
                player.playerInventoryManager.spellItemsInQuickSlots[1] = Instantiate(spells[1]);
            if (spells[2] != null)
                player.playerInventoryManager.spellItemsInQuickSlots[2] = Instantiate(spells[2]);
            player.playerEquipmentManager.LoadSpellItemEquipment(player.playerInventoryManager.spellItemsInQuickSlots[player.playerInventoryManager.quickSlotSpellIndex]);

            player.playerInventoryManager.itemsInInventory.Clear();
            foreach (var item in inventory)
            {
                player.playerInventoryManager.itemsInInventory.Add(item);
            }

        }

        public void SelectHair(int hairID)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.hairStyleID.Value = hairID;

            CloseChooseHairStyleSubMenu();
        }

        public void PreviewHair(int hairID)
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.hairStyleID.Value = hairID;
        }

        public void SelectHairColor()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;

            CloseChooseHairColorSubMenu();
        }

        public void PreviewHairColor()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            player.playerNetworkManager.hairColorRed.Value = redSlider.value;
            player.playerNetworkManager.hairColorGreen.Value = greenSlider.value;
            player.playerNetworkManager.hairColorBlue.Value = blueSlider.value;
        }

        public void SetRedColorSlider(float redValue)
        {
            redSlider.value = redValue;
        }

        public void SetGreenColorSlider(float greenValue)
        {
            greenSlider.value = greenValue;
        }

        public void SetBlueColorSlider(float blueValue)
        {
            blueSlider.value = blueValue;
        }
    }
}