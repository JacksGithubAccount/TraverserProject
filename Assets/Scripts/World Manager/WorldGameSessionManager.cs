using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;

namespace TraverserProject
{

    public class WorldGameSessionManager : MonoBehaviour
    {

        public static WorldGameSessionManager Singleton;
        [Header("Active players in session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;

        [Header("Active Lobby")]
        public Lobby? currentLobby;
        private FacepunchTransport transport;
        private Coroutine joiningAsClientCoroutine;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
        }

        private void OnDestroy()
        {

            SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
            SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;
        }

        private void OnApplicationQuit()
        {
            DisconnectFromLobby();
        }

        private void OnEnabled()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)
        {
            // if we aren't on the menu scene, allow others to join lobby
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                ToggleLobbyIsJoinable(true);
            }
            else
            {
                ToggleLobbyIsJoinable(false);
            }
        }

        //Facepunch
        public void ToggleLobbyIsJoinable(bool status)
        {
            currentLobby?.SetJoinable(status);
        }

        private void OnLobbyCreated(Result result, Lobby lobby)
        {
            if (result != Result.OK)
            {
                Debug.LogError($"Lobby could not be created, {result}", this);
                return;
            }

            lobby.SetPublic();
            lobby.SetJoinable(false); //we only want to set to joinable once we are in the world
            lobby.SetGameServer(lobby.Owner.Id);
        }

        private void OnGameLobbyJoinRequested(Lobby joinedLobby, SteamId steamID)
        {
            //if on main menu, do not allow join until they load into the world
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsClient)
                {
                    Debug.Log("We are not allowed to join another game, we aren't a client or a host. Start the game first.");
                    return;
                }

                //save before joining
                WorldSaveGameManager.Singleton.SaveGame();
                NetworkManager.Singleton.Shutdown();

                Debug.Log($"Attempting to join game, {joinedLobby.Id}, from {steamID}");
                ;
                currentLobby = joinedLobby;

                //if we have a current lobby, join it
                currentLobby?.Join();
            }
        }

        private void OnLobbyEntered(Lobby lobby)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                return;

            }
            else
            {
                StartGameAsClient(lobby.Owner.Id);
            }
        }



        public async void StartGameAsHost()
        {
            NetworkManager.Singleton.StartHost();

            //to go back to testing with a host and client on one pc, comment out this line
            //currentLobby = await SteamMatchmaking.CreateLobbyAsync(4);
        }

        public void StartGameAsClient(SteamId id)
        {
            if (PlayerUIManager.Singleton.localPlayer.isDead.Value)
            {
                return;
            }

            if (joiningAsClientCoroutine != null)
                StopCoroutine(joiningAsClientCoroutine);

            joiningAsClientCoroutine = StartCoroutine(AttemptToJoinAsClient(id));
        }

        private IEnumerator AttemptToJoinAsClient(SteamId id)
        {
            while (transport.targetSteamId != id)
            {
                transport.targetSteamId = id;
                yield return null;
            }

            yield return null;

            NetworkManager.Singleton.StartClient();
        }

        public void DisconnectFromLobby()
        {
            currentLobby?.Leave();
        }

        public void WaitThenReviveHost()
        {
            if (revivalCoroutine != null)
                StopCoroutine(revivalCoroutine);

            revivalCoroutine = StartCoroutine(ReviveHostCoroutine(5));
        }

        private IEnumerator ReviveHostCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            PlayerUIManager.Singleton.playerUILoadingScreenManager.ActivateLoadingScreen();

            PlayerUIManager.Singleton.localPlayer.ReviveCharacter();

            WorldAIManager.Singleton.ResetAllCharacters();
            WorldInteractablesManager.Singleton.ResetAllRespawnableItems();

            for (int i = 0; i < WorldObjectManager.Singleton.sitesOfGrace.Count; i++)
            {
                if (WorldObjectManager.Singleton.sitesOfGrace[i].siteOfGraceID == WorldSaveGameManager.Singleton.currentCharacterData.lastSiteOfGraceRestedAt)
                {
                    WorldObjectManager.Singleton.sitesOfGrace[i].TeleportToSiteOfGrace();
                    break;
                }
            }
            WorldObjectManager.Singleton.sitesOfGrace[0].TeleportToSiteOfGrace();

            PlayerUIManager.Singleton.playerUILoadingScreenManager.DeactivateLoadingScreen();
        }

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {
            if (!players.Contains(player))

            {
                players.Add(player);
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }


        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {
            if (!players.Contains(player))

            {
                players.Remove(player);
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }
        }

    }
}