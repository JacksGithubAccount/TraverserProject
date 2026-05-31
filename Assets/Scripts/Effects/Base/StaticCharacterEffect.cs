using UnityEngine;

namespace TraverserProject {
    public class StaticCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int staticEffectID;

        [Header("Icon")]
        public Sprite effectIcon;

        public virtual void ProcessStaticEffect(CharacterManager character)
        {

        }
        public virtual void RemoveStaticEffect(CharacterManager character)
        {

        }
    } 
}
