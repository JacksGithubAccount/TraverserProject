using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    public class PlayerRecipeManager : MonoBehaviour
    {
        public static PlayerRecipeManager Singleton;

        [Header("Recipes")]
        public List<Recipe> recipesLearnt = new List<Recipe>();

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