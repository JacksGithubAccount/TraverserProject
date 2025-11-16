using UnityEngine;

namespace TraverserProject
{
    public class HealManager : SpellManager
    {
        [Header("Colliders")]
        public HealDamageCollider damageCollider;

        [Header("Instantiated FX")]
        private GameObject instantiatedDestructionFX;

        private bool hasCollided = false;
        public bool isFullyCharged = false;
        private Rigidbody fireBallRigidBody;
        private Coroutine destructionFXCoroutine;

        protected override void Awake()
        {
            base.Awake();

            fireBallRigidBody = GetComponent<Rigidbody>();
            //damageCollider = GetComponentInChildren<FireBallDamageCollider>();
        }
    }
}