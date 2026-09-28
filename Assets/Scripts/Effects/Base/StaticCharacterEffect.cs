using NUnit.Framework;
using UnityEngine;

namespace TraverserProject {
    public class StaticCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int staticEffectID;

        [Header("Icon")]
        public Sprite effectIcon;

        [Header("Flags")]
        public bool hasConditionals = false;
        public bool hasEffectApplied = false;

        public virtual void ProcessStaticEffect(CharacterManager character)
        {

        }
        public virtual void RemoveStaticEffect(CharacterManager character)
        {

        }
        public virtual bool PassConditionals(CharacterManager character)
        {
            if (!hasConditionals)
                return true;

            return false;
        }
    } 
}
