using UnityEngine;

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
        public bool isRolling = false;
        public bool isGrounded = true;
        public bool canRotate = true;
        public bool canRun = true;
        public bool canMove = true;
        public bool canRoll = true;
        public bool isSliding = false;

        [Header("Slope Sliding")]
        [SerializeField] float slopeSlideStartPositionYOffset = 1;
        [SerializeField] float slopeSlideSphereCastMaxDistance = 2;
        private Vector3 slopeSlideVelocity;
        [SerializeField] float slopeSlideSpeed = -11;
        [SerializeField] float slopeSlideSpeedMultiplier = -3;
        [SerializeField] float slipperySurfaceMaxAngle = 15;
        private bool slideUntilGrounded = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            //DontDestroyOnLoad(this);
        }
        protected virtual void Update()
        {
            HandleGroundCheck();
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

            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
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
            else
            {

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


    }
}