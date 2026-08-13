using UnityEngine;

namespace TraverserProject
{

    public class AIRangerManager : AICharacterManager
    {
        [HideInInspector] public AIRangerCombatManager aiRangerCombatManager;
        [HideInInspector] public AIRangerEquipmentManager aiRangerEquipmentManager;

        protected override void Awake()
        {
            base.Awake();

            aiRangerCombatManager = GetComponent<AIRangerCombatManager>();
            aiRangerEquipmentManager = GetComponent<AIRangerEquipmentManager>();
        }
    }
}