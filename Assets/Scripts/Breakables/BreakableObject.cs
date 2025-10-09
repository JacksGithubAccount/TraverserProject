using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class BreakableObject : NetworkBehaviour
    {
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Status")]
        public NetworkVariable<bool> isBroken = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        //used for client/non owners who break locally does so instantly, then update server for everyone else
        [HideInInspector] public bool isBrokenLocal = false;

        //when object breaks, don't disable gameobject, only mesh renderers and colliders so network info can still be passed
        [Header("Mesh Renderers")]
        [SerializeField] private MeshRenderer[] meshRenderers;

        [Header("Collision")]
        [SerializeField] Collider[] meshColliders;

        [Header("SFX")]
        private AudioSource audioSource;
        [SerializeField] AudioClip[] brokenSFX;

        //TODO: add an activation beacon

        [Header("On Break Settings")]
        [SerializeField] bool addForceOnBreak = false;
        [SerializeField] float addedExplosionDebrisForce = 350;
        [SerializeField] float addedForceDebrisRadius = 5;
        [SerializeField] float addedTorqueDebrisForceMinimum = 250;
        [SerializeField] float addedTorqueDebrisForceMaximum = 500;

        [Header("Instantiated Broken Object")]
        private GameObject brokenObjectPrefab;
        private GameObject instantiatedBrokenObject;

        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            meshColliders = GetComponentsInChildren<Collider>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            //create beacon here
            //add to world object list if loading/unload breakables depending on scenes loaded
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            isBroken.OnValueChanged += OnIsBrokenChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;
            networkRotation.OnValueChanged += OnNetworkRotationChanged;

            if (!NetworkManager.Singleton.IsHost)
            {
                OnIsBrokenChanged(false, isBroken.Value);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            //destroy beacon

            //destroy broken object
            if (instantiatedBrokenObject != null)
                Destroy(instantiatedBrokenObject);

            isBroken.OnValueChanged -= OnIsBrokenChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            networkRotation.OnValueChanged -= OnNetworkRotationChanged;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

        }

        private void OnTriggerEnter(Collider other)
        {
            AICharacterManager aiCharacter = other.GetComponent<AICharacterManager>();

            if (aiCharacter != null)
                BreakObject();

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (player.playerNetworkManager.isJumping.Value)
                    BreakObject();
            }

            DamageCollider damageCollider = other.GetComponent<DamageCollider>();

            if (damageCollider != null)
                BreakObject();
        }

        private void BreakObject()
        {
            if (isBroken.Value || isBrokenLocal)
                return;

            PlayBreakFX();
            BreakObjectServerRpc();
        }

        [ServerRpc]
        private void BreakObjectServerRpc()
        {

        }

        private void OnIsBrokenChanged(bool oldStatus, bool newStatus)
        {

        }

        private void PlayBreakFX()
        {
            isBrokenLocal = true;

            if (!gameObject.activeInHierarchy)
                return;

            instantiatedBrokenObject = Instantiate(brokenObjectPrefab, transform);

            if (addForceOnBreak)
            {
                Rigidbody[] rigidbodies = instantiatedBrokenObject.GetComponentsInChildren<Rigidbody>();

                for (int i = 0; i < rigidbodies.Length; i++)
                {
                    rigidbodies[i].AddExplosionForce(addedExplosionDebrisForce, rigidbodies[i].transform.position, addedForceDebrisRadius);
                    Vector3 torqueDirection = Random.onUnitSphere;
                    rigidbodies[i].AddTorque(torqueDirection * Random.Range(addedTorqueDebrisForceMinimum, addedTorqueDebrisForceMaximum), ForceMode.Impulse);
                }
            }
        }

        private void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {

        }

        private void OnNetworkRotationChanged(Quaternion oldRotation, Quaternion newRotation)
        {

        }

        private void ToggleMeshRenderers(bool status)
        {

        }

    }
}