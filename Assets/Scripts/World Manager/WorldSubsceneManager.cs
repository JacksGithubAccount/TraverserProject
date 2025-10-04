using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class WorldSubsceneManager : MonoBehaviour
    {
        public static WorldSubsceneManager Singleton;

        private List<PlayerManager> playersIn_Area01_Subarea00 = new List<PlayerManager>();
        private List<PlayerManager> playersIn_Area01_Subarea01 = new List<PlayerManager>();
        private List<PlayerManager> playersIn_Area01_Subarea02 = new List<PlayerManager>();
        private List<PlayerManager> playersIn_Area01_Subarea03 = new List<PlayerManager>();
        private List<PlayerManager> playersIn_Area01_Subarea04 = new List<PlayerManager>();
        private List<PlayerManager> playersIn_Area01_Subarea05 = new List<PlayerManager>();

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

        private void RemovePlayerFromPreviousLocation(PlayerManager player)
        {
            if (player == null)
                return;

            if (playersIn_Area01_Subarea00.Contains(player))
                playersIn_Area01_Subarea00.Remove(player);

            if (playersIn_Area01_Subarea01.Contains(player))
                playersIn_Area01_Subarea01.Remove(player);

            if (playersIn_Area01_Subarea02.Contains(player))
                playersIn_Area01_Subarea02.Remove(player);

            if (playersIn_Area01_Subarea03.Contains(player))
                playersIn_Area01_Subarea03.Remove(player);

            if (playersIn_Area01_Subarea04.Contains(player))
                playersIn_Area01_Subarea04.Remove(player);

            if (playersIn_Area01_Subarea05.Contains(player))
                playersIn_Area01_Subarea05.Remove(player);
        }

        public List<string> GenerateDoNotUnloadListBasedOfPlayerLocations()
        {
            List<string> doNotUnloadLocations = new List<string>();

            //world scene is never unloaded
            doNotUnloadLocations.Add(WorldSceneManager.Singleton.world);

            int playersInScene;

            //Subarea 00
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea00.Count; i++)
            {
                if (playersIn_Area01_Subarea00[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
            }

            //Subarea 01
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea01.Count; i++)
            {
                if (playersIn_Area01_Subarea01[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
            }

            //Subarea 02
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea02.Count; i++)
            {
                if (playersIn_Area01_Subarea02[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
            }

            //Subarea 03
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea03.Count; i++)
            {
                if (playersIn_Area01_Subarea03[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
            }

            //Subarea 04
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea04.Count; i++)
            {
                if (playersIn_Area01_Subarea04[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_05);
            }

            //Subarea 05
            playersInScene = 0;

            for (int i = 0; i < playersIn_Area01_Subarea05.Count; i++)
            {
                if (playersIn_Area01_Subarea05[i] != null)
                    playersInScene++;
            }

            if (playersInScene > 0)
            {
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_05);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                doNotUnloadLocations.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
            }
            return doNotUnloadLocations;
        }

        //called whenever a player enters a new additive scene
        public void LoadAreaBasedOnAreaCurrentlyIn(WorldSceneLocation areaCurrentlyIn, PlayerManager player)
        {
            if (IsPlayerAlreadyInArea(areaCurrentlyIn, player))
                return;

            RemovePlayerFromPreviousLocation(player);

            AddPlayerToNewLocation(areaCurrentlyIn, player);

            LoadAdditiveScenesAroundCurrentArea(areaCurrentlyIn);

            WorldSceneManager.Singleton.CheckForUnrequiredScenes();
        }

        private bool IsPlayerAlreadyInArea(WorldSceneLocation area, PlayerManager player)
        {
            bool isPlayerInArea = false;

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    if (playersIn_Area01_Subarea00.Contains(player))
                        isPlayerInArea = true;
                    break;
                case WorldSceneLocation.Area01_Subarea01:
                    if (playersIn_Area01_Subarea01.Contains(player))
                        isPlayerInArea = true;
                    break;
                case WorldSceneLocation.Area01_Subarea02:
                    if (playersIn_Area01_Subarea02.Contains(player))
                        isPlayerInArea = true;
                    break;
                case WorldSceneLocation.Area01_Subarea03:
                    if (playersIn_Area01_Subarea03.Contains(player))
                        isPlayerInArea = true;
                    break;
                case WorldSceneLocation.Area01_Subarea04:
                    if (playersIn_Area01_Subarea04.Contains(player))
                        isPlayerInArea = true;
                    break;
                case WorldSceneLocation.Area01_Subarea05:
                    if (playersIn_Area01_Subarea05.Contains(player))
                        isPlayerInArea = true;
                    break;
                default:
                    break;
            }
            return isPlayerInArea;
        }

        private void AddPlayerToNewLocation(WorldSceneLocation area, PlayerManager player)
        {
            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    if (!playersIn_Area01_Subarea00.Contains(player))
                        playersIn_Area01_Subarea00.Add(player);
                    break;
                case WorldSceneLocation.Area01_Subarea01:
                    if (!playersIn_Area01_Subarea01.Contains(player))
                        playersIn_Area01_Subarea01.Add(player);
                    break;
                case WorldSceneLocation.Area01_Subarea02:
                    if (!playersIn_Area01_Subarea02.Contains(player))
                        playersIn_Area01_Subarea02.Add(player);
                    break;
                case WorldSceneLocation.Area01_Subarea03:
                    if (!playersIn_Area01_Subarea03.Contains(player))
                        playersIn_Area01_Subarea03.Add(player);
                    break;
                case WorldSceneLocation.Area01_Subarea04:
                    if (!playersIn_Area01_Subarea04.Contains(player))
                        playersIn_Area01_Subarea04.Add(player);
                    break;
                case WorldSceneLocation.Area01_Subarea05:
                    if (!playersIn_Area01_Subarea05.Contains(player))
                        playersIn_Area01_Subarea05.Add(player);
                    break;
                default:
                    break;
            }
        }

        private void LoadAdditiveScenesAroundCurrentArea(WorldSceneLocation area)
        {
            List<string> scenesToLoad = new List<string>();

            switch (area)
            {
                case WorldSceneLocation.Area01_Subarea00:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                    break;
                case WorldSceneLocation.Area01_Subarea01:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                    break;
                case WorldSceneLocation.Area01_Subarea02:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                    break;
                case WorldSceneLocation.Area01_Subarea03:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_02);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                    break;
                case WorldSceneLocation.Area01_Subarea04:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_03);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_05);
                    break;
                case WorldSceneLocation.Area01_Subarea05:
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_05);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_00);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_01);
                    scenesToLoad.Add(WorldSceneManager.Singleton.area_01_Subarea_04);
                    break;
                default:
                    break;

            }

            if (scenesToLoad.Count <= 0)
                return;

            WorldSceneManager.Singleton.LoadAdditiveScenes(scenesToLoad);
        }

    }
}