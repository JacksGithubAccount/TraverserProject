using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class AIRangerNetworkManager : AICharacterNetworkManager
    {
        AIRangerManager ranger;

        [Header("Weapons")]
        public NetworkVariable<bool> isUsingMeleeWeapon = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            ranger = GetComponent<AIRangerManager>();
        }

        public void OnIsUsingMeleeWeaponChanged(bool oldStatus, bool newStatus)
        {
            ranger.aiRangerEquipmentManager.SwitchWeapon(isUsingMeleeWeapon.Value);
            ranger.animator.SetBool("isUsingMeleeWeapon", isUsingMeleeWeapon.Value);

        }

    }
}