using TraverserProject;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TraverserProject
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Singleton;
        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;


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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            Singleton.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //enables controls if loading into our world scene
            if (newScene.buildIndex == WorldSaveGameManager.Singleton.GetWorldSceneIndex())
            {
                Singleton.enabled = true;
            }
            else
            {
                Singleton.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        private void Update()
        {
            HandleMovementInput();
        }
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            //clamps movement values
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        }
    }
}
