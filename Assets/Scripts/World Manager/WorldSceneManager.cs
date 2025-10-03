using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class WorldSceneManager : NetworkBehaviour
    {
        public static WorldSceneManager Singleton;

        //loaded scenes
        public List<Scene> loadedScenes = new List<Scene>();

        //queued scenes
        private List<string> queuedSceneIDs = new List<string>();
        private int queuedScenesToLoad = 0;
        private Coroutine loadingAdditiveScenesCoroutine;

        //loading status
        private bool sceneIsLoading = false;
        private bool sceneIsUnloading = false;


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

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            NetworkManager.SceneManager.OnSceneEvent += OnSceneEvent;

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            NetworkManager.SceneManager.OnSceneEvent -= OnSceneEvent;

        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            if (!NetworkManager.IsServer)
                return;

            switch (sceneEvent.SceneEventType)
            {
                case SceneEventType.Load:
                    sceneIsLoading = true;
                    break;
                case SceneEventType.Unload:
                    sceneIsUnloading = true;
                    break;
                case SceneEventType.Synchronize:
                    break;
                case SceneEventType.ReSynchronize:
                    break;
                case SceneEventType.LoadEventCompleted:
                    break;
                case SceneEventType.UnloadEventCompleted:
                    sceneIsUnloading = false;
                    break;
                case SceneEventType.LoadComplete:
                    //called when scene is loaded and adds to loaded scene list
                    loadedScenes.Add(sceneEvent.Scene);

                    //clears list IDs with scenes to load is zero
                    if (queuedScenesToLoad <= 0)
                        queuedSceneIDs.Clear();

                    //double checks if scene is loaded, if they are, remove from list
                    for (int i = 0; i < loadedScenes.Count; i++)
                    {
                        if (!loadedScenes[i].isLoaded)
                            loadedScenes.RemoveAt(i);
                    }

                    sceneIsLoading = false;
                    break;
                case SceneEventType.UnloadComplete:
                    break;
                case SceneEventType.SynchronizeComplete:
                    break;
                case SceneEventType.ActiveSceneChanged:
                    break;
                case SceneEventType.ObjectSceneChanged:
                    break;
                default:
                    break;
            }

        }

        public void LoadWorldScene(int buildIndex)
        {
            PlayerUIManager.Singleton.playerUILoadingScreenManager.ActivateLoadingScreen();

            string worldScene = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            NetworkManager.Singleton.SceneManager.LoadScene(worldScene, LoadSceneMode.Single);

            PlayerUIManager.Singleton.localPlayer.LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.Singleton.currentCharacterData);

        }

        private void LoadAdditiveScene(string sceneName)
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (loadedScenes[i].name == sceneName && loadedScenes[i].isLoaded)
                    return;

                var loadSceneStatus = NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            }
        }

        public void LoadAdditiveScenes(List<string> scenesToLoad)
        {
            if (!NetworkManager.IsServer)
                return;

            for (int i = 0; i < scenesToLoad.Count; i++)
            {
                queuedSceneIDs.Add(scenesToLoad[i]);
            }
            queuedScenesToLoad = queuedSceneIDs.Count;

            if (loadingAdditiveScenesCoroutine != null)
                StopCoroutine(loadingAdditiveScenesCoroutine);

            loadingAdditiveScenesCoroutine = StartCoroutine(LoadAdditiveScenesCoroutine());
        }

        //used to load multiple additive scenes at once when entering new area
        private IEnumerator LoadAdditiveScenesCoroutine()
        {
            for (int i = 0; i < queuedSceneIDs.Count; i++)
            {
                while (sceneIsLoading || sceneIsUnloading)
                {
                    yield return null;
                }

                if (queuedSceneIDs[i] == null)
                    continue;

                LoadAdditiveScene(queuedSceneIDs[i]);
                queuedScenesToLoad--;

                yield return new WaitForFixedUpdate();

            }
            queuedScenesToLoad = 0;
            loadingAdditiveScenesCoroutine = null;

            yield return null;
        }

    }
}