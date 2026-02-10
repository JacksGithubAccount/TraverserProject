using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class WorldLocationRendererManager : MonoBehaviour
    {
        [Header("Scene I.D.")]
        [HideInInspector] public int renderSceneID;

        [Header("Root GameObjects")]
        [SerializeField] public List<GameObject> rootGameObjects = new List<GameObject>();

        [Header("Mesh Renderers")]
        [SerializeField] public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private Coroutine toggleAllMeshRenderersCoroutine;

        private void Awake()
        {
            //Gets the scene id of the scene this gameobject is placed in
            renderSceneID = gameObject.scene.buildIndex;
            WorldLocationManager.Singleton.AddLocationRenderManagerToList(this);
        }

        private void Start()
        {
            //When a scene is loaded into the world, you may optionally enable the gameobjects over time to help prevent possible stutters

            //if a loading screen is present, ignore over time call and enable everything instantly
            if (PlayerUIManager.Singleton.playerUILoadingScreenManager.LoadingScreenIsActive())
            {
                ToggleRootObjects(true);
            }
            else
            {
                StartCoroutine(EnableRootGameObjectsOverTime());
            }
        }

        //Root GameObjects

        public void FindAllRootObjects()
        {
            rootGameObjects = new List<GameObject>();

            GameObject[] rootObjectsInScene = gameObject.scene.GetRootGameObjects();

            for (int i = 0; i < rootObjectsInScene.Length; i++)
            {
                if (rootObjectsInScene[i] == gameObject)
                    continue;

                if (rootGameObjects.Contains(rootObjectsInScene[i]))
                    continue;

                rootGameObjects.Add(rootObjectsInScene[i]);
            }
        }

        public void ToggleRootObjects(bool status)
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(status);
            }
        }
        private IEnumerator EnableRootGameObjectsOverTime()
        {
            for (int i = 0; i < rootGameObjects.Count; i++)
            {
                if (rootGameObjects[i] == null)
                    continue;

                rootGameObjects[i].SetActive(true);

                yield return new WaitForEndOfFrame();
            }
        }


        //Renderers
        public void FindAllMeshRenderers()
        {
            MeshRenderer[] allMeshRenderers = FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            meshRenderers = new List<MeshRenderer>();

            for (int i = 0; i < allMeshRenderers.Length; i++)
            {
                if (allMeshRenderers[i].gameObject.scene != gameObject.scene)
                    continue;

                if (!meshRenderers.Contains(allMeshRenderers[i]))
                    meshRenderers.Add(allMeshRenderers[i]);
            }
        }

        public void ToggleMeshRenderers(bool status)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;
            }
        }

        public void ToggleAllMeshRenderersOverTime(bool status)
        {
            if (toggleAllMeshRenderersCoroutine != null)
                StopCoroutine(toggleAllMeshRenderersCoroutine);

            toggleAllMeshRenderersCoroutine = StartCoroutine(ToggleAllMeshRenderersOverTimeCoroutine(status));
        }

        private IEnumerator ToggleAllMeshRenderersOverTimeCoroutine(bool status)
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] == null)
                    continue;

                meshRenderers[i].enabled = status;
            }
        }
    }
}