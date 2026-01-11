using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    public class WorldInteractablesManager : MonoBehaviour
    {
        public static WorldInteractablesManager Singleton;
        [Header("World Respawn Items")]
        public List<PickUpItemInteractable> worldRespawnItems = new List<PickUpItemInteractable>();
        public List<DoorInteractable> doorInteractables = new List<DoorInteractable>();

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
        }
        public void ResetAllRespawnableItems()
        {
            foreach(PickUpItemInteractable item in worldRespawnItems)
            {
                if (item == null) 
                    continue;

                if(!item.hasBeenLooted)
                    continue;
                
                item.hasBeenLooted = false;
                item.gameObject.SetActive(true);

                Collider collider = item.gameObject.GetComponent<Collider>();
                if (collider == null)
                    return;

                collider.enabled = true;
            }
        }
    }
}
