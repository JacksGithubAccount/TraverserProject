using UnityEngine;

namespace TraverserProject
{

    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

    }
}