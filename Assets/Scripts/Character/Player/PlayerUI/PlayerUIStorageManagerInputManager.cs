using UnityEngine;

namespace TraverserProject
{

    public class PlayerUIStorageManagerInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        PlayerUIStorageManager playerUIStorageManager;

        [Header("Inputs")]
        [SerializeField] bool moveStorageCategoryUpInput;
        [SerializeField] bool moveStorageCategoryDownInput;

        private void Awake()
        {
            playerUIStorageManager = GetComponentInParent<PlayerUIStorageManager>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerActions.RB.performed += i => moveStorageCategoryUpInput = true;
                playerControls.PlayerActions.LB.performed += i => moveStorageCategoryDownInput = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            HandlePlayerUIStorageManagerInputs();
        }

        private void HandlePlayerUIStorageManagerInputs()
        {
            if (moveStorageCategoryUpInput)
            {
                moveStorageCategoryUpInput = false;
                playerUIStorageManager.UpdateStorageCategoryIndex(true);
            }

            if (moveStorageCategoryDownInput)
            {
                moveStorageCategoryDownInput = false;
                playerUIStorageManager.UpdateStorageCategoryIndex(false);
            }
        }

    }
}