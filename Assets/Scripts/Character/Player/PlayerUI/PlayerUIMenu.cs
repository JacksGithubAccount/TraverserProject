using UnityEngine;
using System.Collections;

namespace TraverserProject
{

    public class PlayerUIMenu : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        public virtual void OpenMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = true;
            menu.SetActive(true);
        }

        public virtual void CloseMenu()
        {
            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

        public virtual void CloseMenuAfterFixedFrame()
        {
            StartCoroutine(WaitThenCloseMenu());
        }

        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            PlayerUIManager.Singleton.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

    }
}