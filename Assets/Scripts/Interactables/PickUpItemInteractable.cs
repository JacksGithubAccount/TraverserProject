using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{

    public class PickUpItemInteractable : Interactable
    {
        public ItemPickUpType pickUpType;

        [Header("Item")]
        [SerializeField] Item item;

        [Header("World Spawn Pick Up")]
        [SerializeField] int itemID;
        [SerializeField] bool hasBeenLooted = false;

        protected override void Start()
        {
            base.Start();

            if (pickUpType == ItemPickUpType.WorldSpawn)
                CheckIfWorldItemWasAlreadyLooted();
        }

        private void CheckIfWorldItemWasAlreadyLooted()
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                gameObject.SetActive(false);
                return;
            }


            if (!WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
            {
                WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Add(itemID, false);
            }
            hasBeenLooted = WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted[itemID];

            if (hasBeenLooted)
                gameObject.SetActive(false);

        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Singleton.pickUpItemSFX);

            player.playerInventoryManager.AddItemToInventory(item);

            PlayerUIManager.Singleton.playerUIPopUpManager.SendItemPopUp(item, 1);

            if (pickUpType == ItemPickUpType.WorldSpawn)
            {
                if (WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Remove(itemID);
                }
                WorldSaveGameManager.Singleton.currentCharacterData.worldItemsLooted.Add(itemID, true);
            }

            Destroy(gameObject);
        }

    }
}