using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;


        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Dead Spot")]
        public bool hasDeadSpot = false;
        public float deadSpotPositionX;
        public float deadSpotPositionY;
        public float deadSpotPositionZ;
        public int deadSpotBubbleCount;

        [Header("Body Type")]
        public bool isMale = true;
        public int hairStyleID;
        public float hairColorRed;
        public float hairColorGreen;
        public float hairColorBlue;

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        public int currentFocusPoints;
        public int bubbles;

        [Header("Stats")]
        public int vigor;
        public int mind;
        public int endurance;
        public int strength;
        public int dexterity;
        public int intelligence;
        public int faith;
        public int luck;


        [Header("Sites Of Grace")]
        public int lastSiteOfGraceRestedAt = 0;
        public SerializableDictionary<int, bool> sitesOfGrace;

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened;
        public SerializableDictionary<int, bool> bossesDefeated;

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted;

        [Header("Doors")]
        public SerializableDictionary<int, bool> doorsOpened;

        [Header("Shortcuts")]
        public SerializableDictionary<int, bool> shortcutsActivated;

        [Header("Equipment")]
        public int headEquipment;
        public int bodyEquipment;
        public int handEquipment;
        public int legEquipment;

        public int rightWeaponIndex;
        public SerializableWeapon rightWeapon01;
        public SerializableWeapon rightWeapon02;
        public SerializableWeapon rightWeapon03;

        public int leftWeaponIndex;
        public SerializableWeapon leftWeapon01;
        public SerializableWeapon leftWeapon02;
        public SerializableWeapon leftWeapon03;

        public int spellIndex;
        public SerializableSpell spell01;
        public SerializableSpell spell02;
        public SerializableSpell spell03;
        public int currentSpell;

        public int quickSlotIndex;
        public SerializableQuickSlotItem quickSlotItem01;
        public SerializableQuickSlotItem quickSlotItem02;
        public SerializableQuickSlotItem quickSlotItem03;

        public SerializableRangedProjectile mainProjectile;
        public SerializableRangedProjectile secondaryProjectile;

        public int currentHealthFlaskRemaining = 3;
        public int currentFocusPointsFlaskRemaining = 2;

        [Header("Inventory")]
        public List<SerializableWeapon> weaponsInInventory;
        public List<SerializableRangedProjectile> projectilesInInventory;
        public List<SerializableQuickSlotItem> quickSlotItemsInInventory;
        public List<int> headEquipmentInInventory;
        public List<int> bodyEquipmentInInventory;
        public List<int> handEquipmentInInventory;
        public List<int> legEquipmentInInventory;

        [Header("Dialogue")]
        public int namelessKnightStageID = 0;
        public int blacksmithStageID = 0;
        public int blacksmithMenuStageID = 0;

        [Header("Crafting")]
        public List<Recipe> recipesLearnt;

        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();
            doorsOpened = new SerializableDictionary<int, bool>();
            shortcutsActivated = new SerializableDictionary<int, bool>();  

            weaponsInInventory = new List<SerializableWeapon>();
            projectilesInInventory = new List<SerializableRangedProjectile>();
            quickSlotItemsInInventory = new List<SerializableQuickSlotItem>();
            headEquipmentInInventory = new List<int>();
            bodyEquipmentInInventory = new List<int>();
            handEquipmentInInventory = new List<int>();
            legEquipmentInInventory = new List<int>();

        }
    }
}