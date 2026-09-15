using UnityEngine;

namespace TraverserProject
{

    public class AIRangerManager : AICharacterManager
    {
        [HideInInspector] public AIRangerNetworkManager aiRangerNetworkManager;
        [HideInInspector] public AIRangerCombatManager aiRangerCombatManager;
        [HideInInspector] public AIRangerEquipmentManager aiRangerEquipmentManager;

        protected override void Awake()
        {
            base.Awake();

            aiRangerNetworkManager = GetComponent<AIRangerNetworkManager>();
            aiRangerCombatManager = GetComponent<AIRangerCombatManager>();
            aiRangerEquipmentManager = GetComponent<AIRangerEquipmentManager>();

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            aiRangerNetworkManager.isUsingMeleeWeapon.OnValueChanged += aiRangerNetworkManager.OnIsUsingMeleeWeaponChanged;

            if (!IsOwner)
                aiRangerNetworkManager.OnIsUsingMeleeWeaponChanged(false, aiRangerNetworkManager.isUsingMeleeWeapon.Value);
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            aiRangerNetworkManager.isUsingMeleeWeapon.OnValueChanged -= aiRangerNetworkManager.OnIsUsingMeleeWeaponChanged;
        }
    }
}