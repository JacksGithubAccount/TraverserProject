using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUITeleportLocationManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        [SerializeField] GameObject[] teleportLocations;

        public void OpenTeleportLocationManagerMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = true;
            menu.SetActive(true);

            CheckForUnlockedTeleports();

        }

        public void CloseTeleportLocationManagerMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        private void CheckForUnlockedTeleports()
        {
            bool hasFirstSelectedButton = false;

            for (int i = 0; i < teleportLocations.Length; i++)
            {
                for (int s = 0; s < WorldObjectManager.Singleton.sitesOfGrace.Count; s++)
                {
                    if (WorldObjectManager.Singleton.sitesOfGrace[s] == null)
                        WorldObjectManager.Singleton.sitesOfGrace.RemoveAt(s);

                    if (WorldObjectManager.Singleton.sitesOfGrace[s].siteOfGraceID == i)
                    {
                        if (WorldObjectManager.Singleton.sitesOfGrace[s].isActivated.Value)
                        {
                            teleportLocations[i].SetActive(true);

                            if (!hasFirstSelectedButton)
                            {
                                hasFirstSelectedButton = true;
                                teleportLocations[i].GetComponent<Button>().Select();
                                teleportLocations[i].GetComponent<Button>().OnSelect(null);
                            }
                        }
                        else
                        {
                            teleportLocations[i].SetActive(false);
                        }
                    }
                }
            }
        }

        public void TeleportToSiteOfGrace(int siteID)
        {
            for (int i = 0; i < WorldObjectManager.Singleton.sitesOfGrace.Count; i++)
            {               
                if (WorldObjectManager.Singleton.sitesOfGrace[i].siteOfGraceID == siteID)
                {
                    WorldObjectManager.Singleton.sitesOfGrace[i].TeleportToSiteOfGrace();
                    return;
                }
            }
        }

    }
}