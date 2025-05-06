using UnityEngine;

namespace TraverserProject
{
    public class CharacterSaveData
    {
            
        [Header("Character Name")]
        public string characterName;

        [Header("Time Played")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}
