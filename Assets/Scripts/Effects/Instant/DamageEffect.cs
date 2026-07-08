using UnityEngine;

namespace TraverserProject
{

    public class DamageEffect : InstantCharacterEffect
    {
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public PhysicalDamageType physicalDamageType = PhysicalDamageType.Regular;

        [Header("Final Damage")]
        protected int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

    }
}