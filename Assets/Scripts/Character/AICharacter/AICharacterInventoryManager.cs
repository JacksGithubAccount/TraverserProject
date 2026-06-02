using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        AICharacterManager aiCharacter;
        [Header("Loot Chance")]
        public int dropItemChance = 10;
        [SerializeField] Item[] droppableItems;

        [Header("Shop")]
        public Shops characterShopID;
        [SerializeField] CharacterShop characterShop;
        private bool shopHasBeenGenerated = false;

        protected override void Awake()
        {
            base.Awake();
            aiCharacter = GetComponent<AICharacterManager>();
        }

        public void DropItem()
        {
            if (!aiCharacter.IsOwner)
                return;

            bool willDropItem = false;
            int itemChanceRoll = Random.Range(0, 100);

            if (itemChanceRoll <= dropItemChance)
                willDropItem = true;

            if (!willDropItem)
                return;

            if (droppableItems.Length > 0)
            {
                Item generatedItem = droppableItems[Random.Range(0, droppableItems.Length)];

                if (generatedItem == null)
                    return;


                GameObject itemPickUpInteractableGameObject = Instantiate(WorldItemDatabase.Singleton.creatureDropPickUpItemPrefab);
                PickUpItemInteractable pickUpInteractable = itemPickUpInteractableGameObject.GetComponent<PickUpItemInteractable>();

                itemPickUpInteractableGameObject.GetComponent<NetworkObject>().Spawn();
                pickUpInteractable.itemID.Value = generatedItem.itemID;
                pickUpInteractable.networkPosition.Value = transform.position;
                pickUpInteractable.droppingCreatureID.Value = aiCharacter.NetworkObjectId;
            }
        }

        public override void RemoveItemFromInventory(Item item)
        {
            base.RemoveItemFromInventory(item);
            SaveShopData();
        }

        public void GenerateShop()
        {
            if (shopHasBeenGenerated)
                return;

            shopHasBeenGenerated = true;

            //if shop has been generated once, load old shop from saved character info to retain previous purchased item values
            if (WorldSaveGameManager.Singleton.currentCharacterData.shopsGenerated.Contains((int)characterShopID))
            {
                itemsInInventory = WorldItemDatabase.Singleton.GetShopItemsFromSerializedData(WorldSaveGameManager.Singleton.currentCharacterData.shopsGeneratedData[(int)characterShopID]);
                return;
            }


            //otherwise, generate shop for first time then save it
            characterShop.GenerateCharacterInventoryFromShopItems(aiCharacter);

            SaveShopData();
        }

        public void SaveShopData()
        {
            WorldSaveGameManager.Singleton.currentCharacterData.shopsGenerated.Add((int)characterShopID);

            if (WorldSaveGameManager.Singleton.currentCharacterData.shopsGeneratedData.ContainsKey((int)characterShopID))
                WorldSaveGameManager.Singleton.currentCharacterData.shopsGeneratedData.Remove((int)characterShopID);

            WorldSaveGameManager.Singleton.currentCharacterData.shopsGeneratedData.Add((int)characterShopID, WorldSaveGameManager.Singleton.GetSerializableShopItemsFromItemList(itemsInInventory));

        }

    }
}