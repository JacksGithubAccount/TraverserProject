using UnityEngine;
using Unity.Netcode;
using TravserserProject;
using NUnit.Framework;
using System.Collections.Generic;

namespace TraverserProject
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Singleton { get; set; }
        [HideInInspector] public PlayerManager localPlayer;
        private AudioSource audioSource;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUICharacterMenuManager playerUICharacterMenuManager;
        [HideInInspector] public PlayerUIEquipmentManager playerUIEquipmentManager;
        [HideInInspector] public PlayerUISiteOfGraceManager playerUISiteOfGraceManager;
        [HideInInspector] public PlayerUITeleportLocationManager playerUITeleportLocationManager;
        [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;
        [HideInInspector] public PlayerUILevelUpManager playerUILevelUpManager;
        [HideInInspector] public PlayerUIWeaponUpgradeManager playerUIWeaponUpgradeManager;
        [HideInInspector] public PlayerUIAnvilMenuManager playerUIAnvilMenuManager;
        [HideInInspector] public PlayerUIBlacksmithMenuManager playerUIBlacksmithMenuManager;
        [HideInInspector] public PlayerUICraftingManager playerUICraftingManager;

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;
        public bool popUpWindowIsOpen = false;

        [Header("Open Menus")]
        public Stack<PlayerUIMenu> openMenus = new Stack<PlayerUIMenu>();


        private void Awake()
        {
            //there can only be one of this object in the game at any one time. if another exist, destroy it
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }

            audioSource = GetComponent<AudioSource>();

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
            playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();
            playerUIWeaponUpgradeManager = GetComponentInChildren<PlayerUIWeaponUpgradeManager>();
            playerUIAnvilMenuManager = GetComponentInChildren<PlayerUIAnvilMenuManager>();
            playerUIBlacksmithMenuManager = GetComponentInChildren<PlayerUIBlacksmithMenuManager>();
            playerUICraftingManager = GetComponentInChildren<PlayerUICraftingManager>();
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        private void Update()
        {
            if (startGameAsClient)
            {
                startGameAsClient = false;
                //first shutdown network as host to start as client
                NetworkManager.Singleton.Shutdown();
                NetworkManager.Singleton.StartClient();
            }
        }

        public void CloseAllMenuWindows()
        {
            playerUICharacterMenuManager.CloseMenuAfterFixedFrame();
            playerUIEquipmentManager.CloseMenuAfterFixedFrame();
            playerUISiteOfGraceManager.CloseMenuAfterFixedFrame();
            playerUITeleportLocationManager.CloseMenuAfterFixedFrame();
            playerUILevelUpManager.CloseMenuAfterFixedFrame();
            playerUIWeaponUpgradeManager.CloseMenuAfterFixedFrame();
            playerUIAnvilMenuManager.CloseMenuAfterFixedFrame();
            playerUIBlacksmithMenuManager.CloseMenuAfterFixedFrame();
            playerUICraftingManager.CloseMenuAfterFixedFrame();

            openMenus.Clear();
        }

        public void CloseAllOpenMenuWindows()
        {
            foreach (var menu in openMenus)
            {
                menu.CloseMenu();
            }
            openMenus.Clear();
        }

        // UI SFX
        public void PlayUnableToContinueSFX()
        {
            if (WorldSoundFXManager.Singleton.unableToContinueUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.Singleton.unableToContinueUISFX);
        }

        public void ConfirmSFX()
        {
            if (WorldSoundFXManager.Singleton.confirmUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.Singleton.confirmUISFX);
        }

        public void HoverSFX()
        {
            if (WorldSoundFXManager.Singleton.hoverUISFX == null)
                return;

            audioSource.PlayOneShot(WorldSoundFXManager.Singleton.hoverUISFX);
        }
    }
}