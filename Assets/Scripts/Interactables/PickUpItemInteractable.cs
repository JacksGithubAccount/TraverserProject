using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TraverserProject
{

    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("Creature Loot Pick Up")]
        public NetworkVariable<int> itemID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<ulong> droppingCreatureID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public bool trackDroppingCreaturesPosition = true;

        [Header("World Spawn Pick Up")]
        [SerializeField] int worldSpawnInteractableID;
        [SerializeField] bool hasBeenLooted = false;

        [Header("Drop SFX")]
        [SerializeField] AudioClip itemDropSFX;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            itemID.OnValueChanged += OnItemIDChanged;
            networkPosition.OnValueChanged += OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged += OnDroppingCreatureIDChanged;

            if (pickUpType == ItemPickUpType.CharacterDrop)
                audioSource.PlayOneShot(itemDropSFX);

            if (!IsOwner)
            {
                OnItemIDChanged(0, itemID.Value);
                OnNetworkPositionChanged(Vector3.zero, networkPosition.Value);
                OnDroppingCreatureIDChanged(0, droppingCreatureID.Value);
            }

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            itemID.OnValueChanged -= OnItemIDChanged;
            networkPosition.OnValueChanged -= OnNetworkPositionChanged;
            droppingCreatureID.OnValueChanged -= OnDroppingCreatureIDChanged;
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }


            if (!WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.ContainsKey(worldSpawnInteractableID))
            {
                WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, false);
            }
            hasBeenLooted = WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted[worldSpawnInteractableID];

            if (hasBeenLooted)
                gameObject.SetActive(false);

        }

        public override void Interact(PlayerManager player)
        {
            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;


            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.pickUpItemSFX);

            player.playerAnimatorManager.PlayTargetActionAnimation("Pick_Up_Item_01", true);

            player.playerInventoryManager.AddItemToInventory(item);

            PlayerUIManager.Singleton.playerUIPopUpManager.SendItemPopUp(item, 1);

            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.ContainsKey((int)worldSpawnInteractableID))
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Remove(worldSpawnInteractableID);
                }
                WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Add(worldSpawnInteractableID, true);
            }

            DestroyThisNetworkObjectServerRpc();
        }

        protected void OnItemIDChanged(int oldValue, int newValue)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            item = WorldItemDatabase.Singleton.GetItemByID(itemID.Value);
        }

        protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            transform.position = networkPosition.Value;
        }

        protected void OnDroppingCreatureIDChanged(ulong oldID, ulong newID)
        {
            if (pickUpType != ItemPickUpType.CharacterDrop)
                return;

            if (trackDroppingCreaturesPosition)
                StartCoroutine(TrackDroppingCreaturesPosition());
        }

        protected IEnumerator TrackDroppingCreaturesPosition()
        {
            AICharacterManager droppingCreature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[droppingCreatureID.Value].gameObject.GetComponent<AICharacterManager>();
            bool trackCreature = false;

            if (droppingCreature != null)
                trackCreature = true;

            if (trackCreature)
            {
                while (gameObject.activeInHierarchy)
                {
                    transform.position = droppingCreature.characterCombatManager.lockOnTransform.position;
                    yield return null;
                }
            }

            yield return null;
        }

        [ServerRpc(RequireOwnership = false)]
        protected void DestroyThisNetworkObjectServerRpc()
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}