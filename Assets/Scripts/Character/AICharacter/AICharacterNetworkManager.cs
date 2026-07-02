using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace TraverserProject
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        AICharacterManager aiCharacter;

        [Header("Sleep")]
        public NetworkVariable<bool> isAwake = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<FixedString64Bytes> sleepingAnimation = new NetworkVariable<FixedString64Bytes>("Sleep_01", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<FixedString64Bytes> wakingAnimation = new NetworkVariable<FixedString64Bytes>("Wake_01", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stance")]
        public NetworkVariable<float> currentStance = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void OnIsDeadChanged(bool oldStatus, bool newStatus)
        {
            base.OnIsDeadChanged(oldStatus, newStatus);

            if (aiCharacter.isDead.Value)
            {
                aiCharacter.aiCharacterInventoryManager.DropItem();
                aiCharacter.aiCharacterCombatManager.AwardRunesOnDeath(PlayerUIManager.Singleton.localPlayer);
            }
        }

        public override void OnLockOnTargetIDChange(ulong oldID, ulong newID)
        {
            base.OnLockOnTargetIDChange(oldID, newID);

            //if your character has a target, disable the interactable collider
            if (aiCharacter.aiCharacterCombatManager.currentTarget != null && aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject != null)
                aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject.SetActive(false);

            //optionally re enable it when target is gone
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null && aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject != null)
                aiCharacter.aiCharacterSoundFXManager.interactableDialogueObject.SetActive(true);
        }
    }
}