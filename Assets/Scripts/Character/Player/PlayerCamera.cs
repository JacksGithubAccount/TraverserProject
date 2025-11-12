using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TraverserProject
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Singleton;
        public Camera cameraObject;
        public PlayerManager player;
        public Transform cameraPivotTransform;
        public float cameraPivotYPositionOffset = 1.5f;

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 0.1f; // biggest the numer, longer it takes for camera to move to its position during movement
        [SerializeField] float leftAndRightRotationSpeed = 220;
        [SerializeField] float upAndDownRotationSpeed = 220;
        [SerializeField] float minimumPivot = -30; //lowest point to look down
        [SerializeField] float maximumPivot = 60; //highest point to look up
        [SerializeField] float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;


        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

        [Header("Lock On")]
        [SerializeField] float lockOnRadius = 20;
        [SerializeField] float minimumViewableAngle = -50;
        [SerializeField] float maximumViewableAngle = 50;
        [SerializeField] float lockOnTargetFollowSpeed = 1;
        [SerializeField] float setCameraHeightSpeed = 1;
        [SerializeField] float unlockedCameraHeight = 1.65f;
        [SerializeField] float lockedCameraHeight = 2.0f;
        private Coroutine camerLockOnHeightCoroutine;
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;

        [Header("Ranged Aim")]
        public Vector3 aimDirection;

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
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        private void Update()
        {
            //Vector3 targetLockOnTransform = testObject.transform.position;
            //Vector2 lockOnCrosshairPosition = RectTransformUtility.WorldToScreenPoint(cameraObject, targetLockOnTransform);
            
            //PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.transform.position = lockOnCrosshairPosition;
            //PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.SetActive(true);
        }
        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }
        private void HandleFollowTarget()
        {
            if (player.playerNetworkManager.isAiming.Value)
            {
                Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.playerCombatManager.lockOnTransform.position, ref cameraVelocity, cameraSmoothSpeed + Time.deltaTime);
                transform.position = targetCameraPosition;
            }
            else
            {
                Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed + Time.deltaTime);
                transform.position = targetCameraPosition;
            }
        }

        private void HandleRotations()
        {
            if (player.playerNetworkManager.isAiming.Value)
            {
                HandleAimRotations();
            }
            else
            {
                HandleStandardRotations();
            }
        }

        private void HandleAimRotations()
        {
            if (!player.playerLocomotionManager.isGrounded)
                player.playerNetworkManager.isAiming.Value = false;

            if (player.isPerformingAction)
                return;

            aimDirection = cameraObject.transform.forward.normalized;

            Vector3 cameraRotationY = Vector3.zero;
            Vector3 cameraRotationX = Vector3.zero;

            leftAndRightLookAngle += (PlayerInputManager.Singleton.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.Singleton.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            cameraRotationY.y = leftAndRightLookAngle;
            cameraRotationX.x = upAndDownLookAngle;

            cameraObject.transform.localEulerAngles = new Vector3(upAndDownLookAngle, leftAndRightLookAngle, 0);
        }

        private void HandleStandardRotations()
        {
            if (player.playerNetworkManager.isLockedOn.Value)
            {
                //rotates gameObject
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                //rotates pivot object
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                //saves rotation to look angles so unlock doesnt snap too far away
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            else
            {
                leftAndRightLookAngle += (PlayerInputManager.Singleton.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                upAndDownLookAngle -= (PlayerInputManager.Singleton.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation = targetRotation;


                cameraRotation = Vector3.zero;
                cameraRotation.x = upAndDownLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
                if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
                {
                    targetCameraZPosition = -cameraCollisionRadius;
                }

                if (player.playerNetworkManager.isAiming.Value)
                {
                    cameraObjectPosition.z = 0;
                    cameraObject.transform.localPosition = cameraObjectPosition;
                    return;
                }

                cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
                cameraObject.transform.localPosition = cameraObjectPosition;

            }
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            float shortestDistanceOfleftTarget = -Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Singleton.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

                if (lockOnTarget != null)
                {
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;

                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead.Value)
                        continue;

                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;



                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.Singleton.GetEnviroLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            availableTargets.Add(lockOnTarget);
                        }
                    }

                }
            }
            //sort through available targets to find one we lock onto first
            for (int k = 0; k < availableTargets.Count; k++)
            {
                if (availableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                    Vector3 lockTargetsDirection = availableTargets[k].transform.position - player.transform.position;

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = availableTargets[k];
                    }

                    if (player.playerNetworkManager.isLockedOn.Value)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (availableTargets[k] == player.playerCombatManager.currentTarget)
                            continue;

                        //check left side  for lock on targets
                        if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfleftTarget)
                        {
                            shortestDistanceOfleftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = availableTargets[k];
                        }
                        //check right side for lock on targets
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = availableTargets[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    leftLockOnTarget = null;
                    rightLockOnTarget = null;
                    player.playerNetworkManager.isLockedOn.Value = false;

                }
            }
        }

        public void ResetCameraZPosition()
        {
            Vector3 position = cameraObject.transform.localPosition;
            position.z = cameraZPosition;
            cameraObject.transform.localPosition = position;
        }

        public void SetLockCameraHeight()
        {
            if (camerLockOnHeightCoroutine != null)
            {
                StopCoroutine(camerLockOnHeightCoroutine);
            }

            camerLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.SetActive(false);
            availableTargets.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocatingLockOnTargets();

            if (nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;

                //draws lock on crosshair on lock on target
                Vector3 targetLockOnTransform = nearestLockOnTarget.characterCombatManager.lockOnTransform.transform.position;
                Vector2 lockOnCrosshairPosition = RectTransformUtility.WorldToScreenPoint(cameraObject, targetLockOnTransform);
                //lockOnCrosshairPosition.y = Screen.height - lockOnCrosshairPosition.y;
                PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.transform.position = lockOnCrosshairPosition;
                PlayerUIManager.Singleton.playerUIHudManager.lockOnCrossHair.SetActive(true);
            }


            yield return null;
        }

        public IEnumerator SetCameraHeight()
        {
            float duration = 1;
            float timer = 0;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player != null)
                {
                    if (player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }
            }

            yield return null;


            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
                }
            }
            yield return null;
        }

    }
}