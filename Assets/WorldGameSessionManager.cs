using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class WorldGameSessionManager : MonoBehaviour
    {

        public static WorldGameSessionManager Singleton;
        [Header("Active players in session")]
        public List<PlayerManager> players = new List<PlayerManager>();

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