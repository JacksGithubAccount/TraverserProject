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

        //do not unload
        public List<string> doNotUnloadList = new List<string>();

        //queued scenes
        private List<string> queuedSceneIDs = new List<string>();
        private List<string> queuedUnloadSceneIDs = new List<string>();
        private int queuedScenesToLoad = 0;
        private int queuedScenesToUnload = 0;
        private Coroutine loadingAdditiveScenesCoroutine;
        private Coroutine unloadingAdditiveScenesCoroutine;

        //loading status
        private bool sceneIsLoading = false;
        private bool sceneIsUnloading = false;

        [Header("Scene I.Ds")]
        public string world = "World_01";
        public string area_01_Subarea_00 = "Area_01_Subarea_00";
        public string area_01_Subarea_01 = "Area_01_Subarea_01";
        public string area_01_Subarea_02 = "Area_01_Subarea_02";
        public string area_01_Subarea_03 = "Area_01_Subarea_03";
        public string area_01_Subarea_04 = "Area_01_Subarea_04";
        public string area_01_Subarea_05 = "Area_01_Subarea_05";


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

            DontDestroyOnLoad(gameObject);
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

            StartCoroutine(UnloadAllAdditiveScenesNonNetwork());

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
                    if (queuedScenesToUnload <= 0)
                        queuedUnloadSceneIDs.Clear();

                    for (int i = 0; i < loadedScenes.Count; i++)
                    {
                        if (!loadedScenes[i].isLoaded)
                            loadedScenes.RemoveAt(i);
                    }

                    sceneIsUnloading = false;
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

        //Scene loading

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
                while (sceneIsLoading || sceneIsLoading)
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

        //Scene unloading

        private void UnloadAdditiveScene(string sceneName)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            for (int i = 0; i < doNotUnloadList.Count; i++)
            {
                if (sceneName == doNotUnloadList[i])
                    return;
            }

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (loadedScenes[i].name == sceneName && loadedScenes[i].isLoaded)
                {
                    var sceneLoad = NetworkManager.SceneManager.UnloadScene(loadedScenes[i]);
                    break;
                }
            }
        }

        public void UnloadAdditiveScenes(List<string> sceneList)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            for (int i = 0; i < sceneList.Count; i++)
            {
                queuedUnloadSceneIDs.Add(sceneList[i]);
            }

            queuedScenesToUnload = queuedUnloadSceneIDs.Count;

            if (unloadingAdditiveScenesCoroutine != null)
                StopCoroutine(unloadingAdditiveScenesCoroutine);

            unloadingAdditiveScenesCoroutine = StartCoroutine(UnloadAdditiveScenesCoroutine());
        }

        private IEnumerator UnloadAdditiveScenesCoroutine()
        {
            for (int i = 0; i < queuedUnloadSceneIDs.Count; i++)
            {
                while (sceneIsLoading || sceneIsLoading)
                {
                    yield return new WaitForFixedUpdate();
                }

                UnloadAdditiveScene(queuedUnloadSceneIDs[i]);
                queuedScenesToUnload--;

                yield return null;
            }

            queuedScenesToUnload = 0;
            unloadingAdditiveScenesCoroutine = null;
        }

        private IEnumerator UnloadAllAdditiveScenesNonNetwork()
        {
            for (int i = 0; i < loadedScenes.Count; i++)
            {
                if (loadedScenes[i] == null)
                    continue;

                if (!loadedScenes[i].IsValid())
                    continue;

                var loadingOperation = SceneManager.UnloadSceneAsync(loadedScenes[i].name);

                yield return null;

                while (loadingOperation != null && !loadingOperation.isDone)
                {
                    yield return null;
                }
            }

            yield return null;
        }

        public void CheckForUnrequiredScenes()
        {
            List<string> scenesToUnload = new List<string>();

            for (int i = 0; i < loadedScenes.Count; i++)
            {
                scenesToUnload.Add(loadedScenes[i].name);
            }

            doNotUnloadList = WorldSubsceneManager.Singleton.GenerateDoNotUnloadListBasedOfPlayerLocations();

            for (int i = 0; i < scenesToUnload.Count; i++)
            {
                if (doNotUnloadList.Contains(scenesToUnload[i]))
                    scenesToUnload.Remove(scenesToUnload[i]);
            }

            UnloadAdditiveScenes(scenesToUnload);
        }

        //SCENE IDs
        public string GetSceneIDFromWorldSceneLocation(WorldSceneLocation area)
        {
            string sceneID = "";

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    return area_01_Subarea_00;
                case WorldSceneLocation.Area01_Subarea01:
                    return area_01_Subarea_01;
                case WorldSceneLocation.Area01_Subarea02:
                    return area_01_Subarea_02;
                case WorldSceneLocation.Area01_Subarea03:
                    return area_01_Subarea_03;
                case WorldSceneLocation.Area01_Subarea04:
                    return area_01_Subarea_04;
                case WorldSceneLocation.Area01_Subarea05:
                    return area_01_Subarea_05;
                default:
                    break;
            }

            return sceneID;
        }

    }
}