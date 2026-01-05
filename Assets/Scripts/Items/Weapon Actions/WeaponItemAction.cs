using UnityEngine;
using Unity.Netcode;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value = weaponPerformingAction.itemID;
            }

            playerPerformingAction.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, actionID, weaponPerformingAction.itemID);

        }

        protected virtual void PlayWeaponSwingVFX(WeaponItem weaponPerformingAction, PlayerManager playerPerformingAction, float startDelay, Vector3 localPosition, Vector3 rotation)
        {
            if (WorldCharacterEffectsManager.Singleton.weaponSwingVFX != null)
            {
                weaponPerformingAction.weaponSwingVFX = Instantiate(WorldCharacterEffectsManager.Singleton.weaponSwingVFX);
                ParticleSystem ps = weaponPerformingAction.weaponSwingVFX.GetComponentInChildren<ParticleSystem>();
                var psmain = ps.main;
                psmain.startDelay = startDelay;
                ps.transform.localPosition = localPosition;
                weaponPerformingAction.weaponSwingVFX.transform.position = playerPerformingAction.playerEffectsManager.effectTransform.position;
                weaponPerformingAction.weaponSwingVFX.transform.forward = playerPerformingAction.transform.forward;
                weaponPerformingAction.weaponSwingVFX.transform.eulerAngles += rotation;
            }
        }

    }
}