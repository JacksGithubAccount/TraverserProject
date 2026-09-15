using UnityEngine;

namespace TraverserProject
{

    public class AIRangerEquipmentManager : CharacterEquipmentManager
    {
        AIRangerManager ranger;

        [Header("Bow")]
        public GameObject bowObject;    //used to hide weapon during specific actions(also hide this when "dropping" a version on death)
        public Animator bowAnimator;
        public RangedProjectileItem projectile;
        public Transform drawHand;

        [Header("Alternative Weapon")]
        [SerializeField] GameObject alternativeWeapon;

        protected override void Awake()
        {
            base.Awake();

            ranger = GetComponent<AIRangerManager>();
        }


        public void SwitchWeapon(bool alternative)
        {
            if (alternative)
            {
                bowObject.SetActive(false);
                alternativeWeapon.SetActive(true);
            }
            else
            {
                bowObject.SetActive(true);
                alternativeWeapon.SetActive(false);
            }

            if (ranger.IsOwner && !ranger.isPerformingAction)
                ranger.characterAnimatorManager.PlayTargetActionAnimation("Switch_Weapon_01", true);
        }
    }
}