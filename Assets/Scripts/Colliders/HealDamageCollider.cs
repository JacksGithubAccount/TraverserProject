using UnityEngine;

namespace TraverserProject
{
    public class HealDamageCollider : SpellPointBlankDamageCollider
    {
        private HealManager healManager;


        protected override void Awake()
        {
            base.Awake();
            healManager = GetComponentInParent<HealManager>();
        }
    }
}