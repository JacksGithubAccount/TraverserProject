using UnityEngine;
using Unity.Netcode;
using TravserserProject;

namespace TraverserProject
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Singleton { get; set; }
        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;

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
    }
}