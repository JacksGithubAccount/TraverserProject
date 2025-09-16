using UnityEngine;
using Unity.Netcode;
using TravserserProject;

namespace TraverserProject
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Singleton { get; set; }
        [HideInInspector] public PlayerManager localPlayer;

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

        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;
        public bool popUpWindowIsOpen = false;


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
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
            playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
            playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
            playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
            playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
            playerUILevelUpManager = GetComponentInChildren<PlayerUILevelUpManager>();
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
            playerUICharacterMenuManager.CloseMenu();
            playerUIEquipmentManager.CloseMenu();
            playerUISiteOfGraceManager.CloseMenu();
            playerUITeleportLocationManager.CloseMenu();
            playerUILevelUpManager.CloseMenu();
        }
    }
}