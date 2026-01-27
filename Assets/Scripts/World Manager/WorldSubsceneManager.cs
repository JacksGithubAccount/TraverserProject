using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;

namespace TraverserProject
{

    public class WorldSubsceneManager : MonoBehaviour
    {
        public static WorldSubsceneManager Singleton;

        private Dictionary<WorldLocationSceneSet, List<PlayerManager>> playersInLocation = new Dictionary<WorldLocationSceneSet, List<PlayerManager>>();

        [Header("Probe Volume Set")]
        [SerializeField] ProbeVolumeBakingSet bakeSet;

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



        public List<string> GenerateDoNotUnloadListBasedOfPlayerLocations()
        {
            List<string> doNotUnloadLocations = new List<string>();

            //world scene is never unloaded
            doNotUnloadLocations.Add(WorldSceneManager.Singleton.world);

            List<WorldLocationSceneSet> areasWithPlayersActive = new List<WorldLocationSceneSet>();

            //search each world scene with active entries
            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                //null checker and clean
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }

                //if a scene has at least one player, add that scene to active players
                if (pair.Value.Count > 0 && !areasWithPlayersActive.Contains(pair.Key))
                    areasWithPlayersActive.Add(pair.Key);
            }

            //go through world locations that are active, adn add their required scenes to the do not unload list
            for (int i = 0; i < areasWithPlayersActive.Count; i++)
            {
                List<string> scenesRequired = areasWithPlayersActive[i].GetRequiredSceneIDsForWorldLocation();

                for (int j = 0; j < scenesRequired.Count; j++)
                {
                    doNotUnloadLocations.Add(scenesRequired[j]);
                }
            }

            return doNotUnloadLocations;
        }

        //called whenever a player enters a new additive scene
        public void LoadAreaBasedOnAreaCurrentlyIn(WorldLocationSceneSet areaCurrentlyIn, PlayerManager player)
        {
            if (IsPlayerAlreadyInArea(areaCurrentlyIn, player))
                return;

            RemovePlayerFromPreviousLocation(player);

            AddPlayerToNewLocation(areaCurrentlyIn, player);

            LoadAdditiveScenesAroundCurrentArea(areaCurrentlyIn);

            WorldSceneManager.Singleton.CheckForUnrequiredScenes();
        }

        private bool IsPlayerAlreadyInArea(WorldLocationSceneSet area, PlayerManager player)
        {
            bool isPlayerInArea = false;

            if (playersInLocation.ContainsKey(area) && playersInLocation[area].Contains(player))
                isPlayerInArea = true;

            return isPlayerInArea;
        }

        private void RemovePlayerFromPreviousLocation(PlayerManager player)
        {
            if (player == null)
                return;

            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                if (pair.Value.Contains(player))
                    pair.Value.Remove(player);

                //null checker and clean
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }

        private void AddPlayerToNewLocation(WorldLocationSceneSet area, PlayerManager player)
        {
            if (player == null)
                return;
            //set the baking set
            if (player.IsOwner)
                StartCoroutine(WaitThenSetActiveScene());

            if (!playersInLocation.ContainsKey(area))
                playersInLocation[area] = new List<PlayerManager>();

            if (!playersInLocation[area].Contains(player))
                playersInLocation[area].Add(player);

            player.areaCurrentlyIn = area;

            foreach (KeyValuePair<WorldLocationSceneSet, List<PlayerManager>> pair in playersInLocation)
            {
                //null checker and clean
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    if (pair.Value[i] == null)
                        pair.Value.RemoveAt(i);
                }
            }
        }

        private void LoadAdditiveScenesAroundCurrentArea(WorldLocationSceneSet area)
        {
            List<string> scenesToLoad = new List<string>();

            List<WorldLocationSceneSet> worldLocations = new List<WorldLocationSceneSet>();

            scenesToLoad = area.GetRequiredSceneIDsForWorldLocation();

            if (scenesToLoad.Count <= 0)
                return;

            WorldSceneManager.Singleton.LoadAdditiveScenes(scenesToLoad);
        }

        private IEnumerator WaitThenSetActiveScene()
        {
            bool hasScene = false;

            while (!hasScene)
            {
                for (int i = 0; i < WorldSceneManager.Singleton.loadedScenes.Count; i++)
                {
                    if (WorldSceneManager.Singleton.loadedScenes[i].name == WorldSceneManager.Singleton.world)
                    {
                        hasScene = true;
                        ProbeReferenceVolume.instance.SetActiveScene(WorldSceneManager.Singleton.loadedScenes[i]);
                        ProbeReferenceVolume.instance.SetActiveBakingSet(bakeSet);
                    }
                    yield return null;
                }
            }

            yield return null;
        }

    }
}