using UnityEngine;

namespace TraverserProject
{

    public class AIRangerEquipmentManager : CharacterEquipmentManager
    {
        [Header("Bow")]
        public GameObject bowObject;    //used to hide weapon during specific actions(also hide this when "dropping" a version on death)
        public Animator bowAnimator;
        public RangedProjectileItem projectile;
        public Transform drawHand;


    }
}