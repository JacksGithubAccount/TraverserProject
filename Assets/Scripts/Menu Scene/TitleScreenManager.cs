using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace TraverserProject
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button MainMenuLoadGameButton;

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame() 
        {
            WorldSaveGameManager.Singleton.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.Singleton.LoadWorldScene());
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
    }
}
