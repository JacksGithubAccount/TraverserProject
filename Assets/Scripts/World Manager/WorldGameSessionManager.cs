using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class WorldGameSessionManager : MonoBehaviour
    {

        public static WorldGameSessionManager Singleton;
        [Header("Active players in session")]
        public List<PlayerManager> players = new List<PlayerManager>();

        private Coroutine revivalCoroutine;

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