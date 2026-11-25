using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class NetworkObjectSpawner : MonoBehaviour
    {
        public int spawnerID;
        [Header("Object")]
        [SerializeField] GameObject networkGameObject;
        private GameObject instantiatedGameObject;



        private void Awake()
        {

        }

        private void Start()
        {
            WorldObjectManager.Singleton.SpawnObject(this);
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnObject()
        {
            if (networkGameObject != null)
            {
                instantiatedGameObject = Instantiate(networkGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }

    }
}