using UnityEngine;
using System.Collections.Generic;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "World Location Scene Set")]
    public class WorldLocationSceneSet : ScriptableObject
    {
        //scenes that this location will require to be loaded, use list of individual scenes to make loading/unloading perform without stutter
        [Header("Scenes Required")]
        public List<string> scenesRequiredForThisLocation = new List<string>();

        //other lcations that need to be loaded in when this location has been loaded (anywhere you can see from this location)
        [Header("Other Required Locations")]
        [SerializeField] WorldLocationSceneSet[] requiredLocations;


        public List<string> GetRequiredSceneIDsForWorldLocation()
        {
            List<string> totalSceneIDsRequiredForAllLocations = new List<string>();

            //add scens needed from this location
            for (int i = 0; i < scenesRequiredForThisLocation.Count; i++)
            {
                totalSceneIDsRequiredForAllLocations.Add(scenesRequiredForThisLocation[i]);
            }

            //add the scenes needed for each location that is attached to this one (other required locations)
            for (int i = 0; i < requiredLocations.Length; i++)
            {
                List<string> sceneIDsRequiredForLocation = new List<string>();

                for (int j = 0; j < requiredLocations[i].scenesRequiredForThisLocation.Count; j++)
                {
                    if (!sceneIDsRequiredForLocation.Contains(requiredLocations[i].scenesRequiredForThisLocation[j]))
                        sceneIDsRequiredForLocation.Add(requiredLocations[i].scenesRequiredForThisLocation[j]);
                }

                for (int j = 0; j < sceneIDsRequiredForLocation.Count; j++)
                {
                    if (!totalSceneIDsRequiredForAllLocations.Contains(sceneIDsRequiredForLocation[j]))
                        totalSceneIDsRequiredForAllLocations.Add(sceneIDsRequiredForLocation[j]);
                }
            }

            return totalSceneIDsRequiredForAllLocations;
        }

        public List<string> GetDoNotUnloadListForWorldLocations()
        {
            return scenesRequiredForThisLocation;
        }
    }
}