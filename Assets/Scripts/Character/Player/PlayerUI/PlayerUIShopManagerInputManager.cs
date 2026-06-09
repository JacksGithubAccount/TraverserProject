using UnityEngine;

namespace TraverserProject
{

    public class PlayerUIShopManagerInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        PlayerUIShopManager playerUIShopManager;

        [Header("Inputs")]
        [SerializeField] bool moveShopCategoryUpInput;
        [SerializeField] bool moveShopCategoryDownInput;

        private void Awake()
        {
            playerUIShopManager = GetComponentInParent<PlayerUIShopManager>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerActions.RB.performed += i => moveShopCategoryUpInput = true;
                playerControls.PlayerActions.LB.performed += i => moveShopCategoryDownInput = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            HandlePlayerUIShopManagerInputs();
        }

        private void HandlePlayerUIShopManagerInputs()
        {
            if (moveShopCategoryUpInput)
            {
                moveShopCategoryUpInput = false;
                playerUIShopManager.UpdateShopCategoryIndex(true);
            }

            if (moveShopCategoryDownInput)
            {
                moveShopCategoryDownInput = false;
                playerUIShopManager.UpdateShopCategoryIndex(false);
            }
        }

    }
}