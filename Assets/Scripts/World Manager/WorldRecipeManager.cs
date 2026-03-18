using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    public class WorldRecipeManager : MonoBehaviour
    {
        public static WorldRecipeManager Singleton;

        [Header("Recipes")]
        public List<Recipe> sitesOfGrace;

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
    }
}