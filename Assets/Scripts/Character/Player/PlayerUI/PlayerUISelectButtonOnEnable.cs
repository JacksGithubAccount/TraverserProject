using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class PlayerUISelectButtonOnEnable : MonoBehaviour
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.Select();
            button.OnSelect(null);
        }

    }
}