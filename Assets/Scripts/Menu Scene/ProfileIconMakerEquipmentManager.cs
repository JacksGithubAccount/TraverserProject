using UnityEngine;

namespace TraverserProject
{

    public class ProfileIconMakerEquipmentManager : PlayerEquipmentManager
    {
        [Header("Body")]
        public bool isMale = true;

        [Header("Equipment")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public HandEquipmentItem handEquipment;
        public LegEquipmentItem legEquipment;

        protected override void Awake()
        {

        }
        protected override void Start()
        {

        }

    }
}