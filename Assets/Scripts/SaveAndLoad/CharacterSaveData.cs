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

        [Header("Body Type")]
        public bool isMale = true;

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

        [Header("Stats")]
        public int vitality;
        public int endurance;
        public int mind;

        [Header("Sites Of Grace")]
        public SerializableDictionary<int, bool> sitesOfGrace;

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened;
        public SerializableDictionary<int, bool> bossesDefeated;

        [Header("World Items")]
        public SerializableDictionary<int, bool> worldItemsLooted;

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

        public SerializableRangedProjectile mainProjectile;
        public SerializableRangedProjectile secondaryProjectile;

        public int currentSpell;


        public CharacterSaveData()
        {
            sitesOfGrace = new SerializableDictionary<int, bool>();
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
            worldItemsLooted = new SerializableDictionary<int, bool>();
        }
    }
}