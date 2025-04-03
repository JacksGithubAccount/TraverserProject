using UnityEngine;

namespace TraverserProject
{
    public class PlayerManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
