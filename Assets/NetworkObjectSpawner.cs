using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class NetworkObjectSpawner : MonoBehaviour
    {
        [Header("Object")]
        [SerializeField] GameObject networkGameObject;
        [SerializeField] GameObject instantiatedGameObject;


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