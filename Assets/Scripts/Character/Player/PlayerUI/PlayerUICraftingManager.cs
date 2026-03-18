using UnityEngine;

namespace TraverserProject
{
    public class PlayerUICraftingManager : PlayerUIMenu
    {
        

        public override void OpenMenu()
        {
            base.OpenMenu();

            CheckForUnlockedRecipes();

        }

        private void CheckForUnlockedRecipes()
        {
            for (int i = 0; i < h; i++)
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

        public void CraftSelectedItem()
        {

        }
    }
}
