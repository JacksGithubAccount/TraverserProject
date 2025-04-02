using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace TraverserProject
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Singleton { get; set; }

        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {
            //there can only be one of this object in the game at any one time. if another exist, destroy it
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

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }
    }
}