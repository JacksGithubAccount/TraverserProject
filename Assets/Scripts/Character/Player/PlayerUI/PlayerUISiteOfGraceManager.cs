using UnityEngine;

namespace TraverserProject
{

    public class PlayerUISiteOfGraceManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        public void OpenSiteOfGraceManagerMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = true;

            menu.SetActive(true);

        }

        public void CloseSiteOfGraceManagerMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public void OpenTeleportLocationMenu()
        {
            CloseSiteOfGraceManagerMenu();
            PlayerUIManager.Singleton.playerUITeleportLocationManager.OpenTeleportLocationManagerMenu();
        }
    }
}