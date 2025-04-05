using UnityEngine;
using Unity.Netcode;


namespace TraverserProject
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame() 
        {
            StartCoroutine(WorldSaveGameManager.Singleton.LoadNewGame());
        }
    }
}
