using UnityEngine;

namespace TraverserProject
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Singleton;
        public Camera cameraObject;
        PlayerManager player;

        [Header("Camera Settings")]
        private Vector3 CameraVelocity;
        private float CameraSmoothSpeed = 1; // biggest the numer, longer it takes for camera to move to its position during movement


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
        }
        public void HandleAllCamereaActions()
        {
            if(player != null)
            {

            }
        }
        private void FollowPlayer()
        {
            Vector3 TargetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref CameraVelocity, CameraSmoothSpeed * Time.deltaTime);
            transform.position = TargetCameraPosition;


        }
    }
}