using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUIMenu : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        

        public virtual void OpenMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = true;

            if(!PlayerUIManager.Singleton.openMenus.Contains(this))
                PlayerUIManager.Singleton.openMenus.Push(this);

            menu.SetActive(true);
        }

        public virtual void OpenMenuAfterFixedFrame()
        {
            if (menu.activeInHierarchy)
                return;

            StartCoroutine(WaitThenOpenMenu());
        }

        public virtual void OpenSubMenu(GameObject subMenu)
        {
            if (!PlayerUIManager.Singleton.openSubmenus.Contains(subMenu))
                PlayerUIManager.Singleton.openSubmenus.Push(subMenu);

            subMenu.SetActive(true);
        }

        protected virtual IEnumerator WaitThenOpenMenu()
        {
            yield return new WaitForFixedUpdate();

            OpenMenu();
        }

        protected void ToggleGameObjectPrefabs(List<GameObject> listOfGameObjectPrefabs, bool toggle)
        {
            foreach (GameObject item in listOfGameObjectPrefabs)
            {
                Button button = item.GetComponent<Button>();
                EventTrigger et = item.GetComponent<EventTrigger>();
                if (button != null)
                    button.enabled = toggle;
                if (et != null)
                    et.enabled = toggle;
            }
        }

        public virtual void CloseMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
            if(PlayerUIManager.Singleton.openMenus.Count > 0)
                PlayerUIManager.Singleton.openMenus.Pop();
        }

        public virtual void CloseMenuForOpeningAnotherMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public virtual void CloseSubMenu()
        {
            GameObject subMenu;
            if (PlayerUIManager.Singleton.openSubmenus.Count > 0)
            {
                subMenu = PlayerUIManager.Singleton.openSubmenus.Pop();
                subMenu.SetActive(false);
            }
        }

        public virtual void CloseMenuAfterFixedFrame()
        {
            if (!menu.activeInHierarchy)
                return;

            StartCoroutine(WaitThenCloseMenu());
        }

        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            CloseMenu();
        }

    }
}