using UnityEngine;
using System.Collections;

namespace TraverserProject
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        [Header("Flags")]
        public bool ignoreGravity = false;
        public bool isRolling = false;
        public bool isGrounded = true;
        public bool canRotate = true;
        public bool canRun = true;
        public bool canMove = true;
        public bool canRoll = true;
        public bool isRidingLift = false;
        public bool isOpeningDoor = false;
        public bool isExitingLadder = false;

        [HideInInspector] public bool canExitTopOfLadder = false;
        [HideInInspector] public bool canExitLadderWithRightHand = false;
        [HideInInspector] public bool canExitLadderWithLeftHand = false;

        [Header("Ladder")]
        public float knockOffLadderWindow = 10;
        private Coroutine enableCanBeKnockedOffLadderCoroutine;

        [Header("Slope Sliding")]
        [SerializeField] float slopeSlideStartPositionYOffset = 1;
        [SerializeField] float slopeSlideSphereCastMaxDistance = 2;
        private Vector3 slopeSlideVelocity;
        [SerializeField] float slopeSlideSpeed = -11;
        [SerializeField] float slopeSlideSpeedMultiplier = -3;
        [SerializeField] float slipperySurfaceMaxAngle = 15;
        public bool isSliding = false;
        public bool isSlidingOffCharacter = false;
        private bool slideUntilGrounded = false;
        private Coroutine slideOffCharacterCoroutine;
        [SerializeField] float characterSlideOffHeadCollisionMaxDistanceCheck = 5;
        [SerializeField] float characterCollisionCheckSphereMultiplier = 1.5f;

        [Header("Ladder")]
        public Transform[] interactedLadderClimbPositions;
        public int currentLadderClimbPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            //DontDestroyOnLoad(this);
        }
        protected virtual void Update()
        {
            HandleGroundCheck();

            if (ignoreGravity)
                return;

            SetGroundedVelocity();
            HandleSlopeSlideCheck();



            if (character.characterLocomotionManager.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;

            }

            if (character.characterController.enabled)
                character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!isGrounded)
                slideUntilGrounded = true;
        }

        protected void HandleGroundCheck()
        {
            if (isGrounded)
            {
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                if (!isGrounded)
                    OnIsNotGrounded();
            }
            else
            {
                //depending on character setup, sometimes making the ground check sphere radius different whilst not grounded has benefits
                isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

                //if jumping or gaining altitude, we are not grounded
                if (yVelocity.y > 0)
                {
                    isGrounded = false;
                    return;
                }

                if (isGrounded)
                    OnIsGrounded();
            }
        }

        //draws sphere around character
        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }

        public void EnableCanRotate()
        {
            canRotate = true;
        }

        public void DisableCanRotate()
        {
            canRotate = false;
        }

        public void EnableCanMove()
        {
            canMove = true;
        }

        public void DisableCanMove()
        {
            canMove = false;
        }

        public void EnableCanRoll()
        {
            canRoll = true;
        }

        public void DisableCanRoll()
        {
            canRoll = false;
        }

        public void EnableIsClimbingLadder()
        {
            if (!character.IsOwner)
                return;

            character.characterNetworkManager.isClimbingLadder.Value = true;
        }

        public void DisableIsClimbingLadder()
        {
            if (!character.IsOwner)
                return;

            character.characterNetworkManager.isClimbingLadder.Value = false;
        }

        //slopes and sliding
        private void HandleSlopeSlideCheck()
        {
            if (slopeSlideVelocity == Vector3.zero)
                isSliding = false;

            if (!isGrounded && slideUntilGrounded)
            {
                SetSlopeSlideVelocity(WorldUtilityManager.Singleton.GetEnviroLayers());

                return;
            }

            if (!isGrounded)
                return;

            SetSlopeSlideVelocity(WorldUtilityManager.Singleton.GetSlipperyEnviroLayers());
        }

        //this function determines what our slope slide velocity will be when not grounded
        private void SetSlopeSlideVelocity(LayerMask layers)
        {
            Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + slopeSlideStartPositionYOffset, transform.position.z);

            //use a sphere cast to determine the angle of what's below us and if the angle is too great, we adjust slope slide velocity
            if (Physics.SphereCast(startPosition, groundCheckSphereRadius, Vector3.down, out RaycastHit hitinfo, slopeSlideSphereCastMaxDistance, layers))
            {
                float angle = Vector3.Angle(hitinfo.normal, Vector3.up);
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);

                if (angle >= slipperySurfaceMaxAngle)
                {
                    slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal);
                    return;
                }
            }
            //otherwise set slope slide velocity to zero
            else
            {
                slopeSlideVelocity = Vector3.zero;
            }

            if (isSliding)
            {
                slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * slopeSlideSpeedMultiplier;

                if (slopeSlideVelocity.magnitude > 1)
                    return;
            }

            slopeSlideVelocity = Vector3.zero;
        }

        private void SetGroundedVelocity()
        {


            if (slopeSlideVelocity != Vector3.zero)
            {
                //if in process of jumping and jump is gaining height, no slide off surface
                if (character.characterNetworkManager.isJumping.Value && yVelocity.y > 0)
                {
                    isSliding = false;
                }
                else
                {
                    isSliding = true;
                }
            }

            if (isSliding)
            {
                yVelocity.y += WorldUtilityManager.Singleton.slopeSlideForce * Time.deltaTime;
                Vector3 slideVelocity = slopeSlideVelocity;

                if (character.characterController.enabled)
                    character.characterController.Move(slideVelocity * Time.deltaTime);
            }

            if (isGrounded)
            {
                if (yVelocity.y <= 0 && !isSliding)
                    yVelocity.y = groundedYVelocity;
            }
            else if (!isGrounded && !isSlidingOffCharacter)
            {
                Collider[] characterColliders = Physics.OverlapSphere(transform.position, groundCheckSphereRadius * characterCollisionCheckSphereMultiplier, WorldUtilityManager.Singleton.GetCharacterLayers());

                for (int i = 0; i < characterColliders.Length; i++)
                {
                    if (characterColliders[i].gameObject.transform.root == character.gameObject.transform.root)
                        continue;

                    CharacterController controller = characterColliders[i].GetComponent<CharacterController>();

                    if (controller == null)
                        continue;

                    if ((controller.collisionFlags & CollisionFlags.CollidedBelow) != 0)
                    {
                        isSlidingOffCharacter = true;
                        SlideOffCharacter();
                    }
                }
            }

            if (!character.characterController.enabled)
                return;

            //this is a desync prevention measure
            if (!character.IsOwner)
            {
                float distance = Vector3.Distance(transform.position, character.characterNetworkManager.networkPosition.Value);

                if (distance > 2.5f)
                {
                    yVelocity = Vector3.zero;
                    character.transform.position = character.characterNetworkManager.networkPosition.Value;
                }
            }
        }

        protected virtual void SlideOffCharacter()
        {
            if (slideOffCharacterCoroutine != null)
                StopCoroutine(slideOffCharacterCoroutine);

            slideOffCharacterCoroutine = StartCoroutine(SlideOffCharacterCoroutine());
        }

        protected virtual IEnumerator SlideOffCharacterCoroutine()
        {
            while (!isGrounded)
            {
                if (Physics.SphereCast(character.transform.position, groundCheckSphereRadius, Vector3.down, out RaycastHit hitInfo, characterSlideOffHeadCollisionMaxDistanceCheck, WorldUtilityManager.Singleton.GetCharacterLayers()))
                {
                    Vector3 characterSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, yVelocity.y, 0), hitInfo.normal);
                    yVelocity.y += WorldUtilityManager.Singleton.slopeSlideForce * Time.deltaTime;
                    Vector3 slideVelocity = characterSlideVelocity;

                    if (character.characterController.enabled)
                        character.characterController.Move(slideVelocity * Time.deltaTime);

                    yield return null;
                }
                yield return null;
            }

            isSlidingOffCharacter = false;

            yield return null;
        }

        protected virtual void OnIsGrounded()
        {
            slideUntilGrounded = false;
        }

        protected virtual void OnIsNotGrounded()
        {

        }

        public void EnableCanBeKnockedOffLadderForATime(float time)
        {
            if (!character.IsOwner)
                return;

            if (enableCanBeKnockedOffLadderCoroutine != null)
                StopCoroutine(enableCanBeKnockedOffLadderCoroutine);

            enableCanBeKnockedOffLadderCoroutine = StartCoroutine(CanBeKnockedOffLadderCoroutine(time));
            character.characterNetworkManager.canBeKnockedOffLadder.Value = true;
        }

        private IEnumerator CanBeKnockedOffLadderCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            character.characterNetworkManager.canBeKnockedOffLadder.Value = false;
        }

        public void FallFromLadder()
        {
            if (!character.IsOwner)
                return;

            character.characterNetworkManager.isClimbingLadder.Value = false;
            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Ladder_Fall_Idle_01", true);

        }

    }
}