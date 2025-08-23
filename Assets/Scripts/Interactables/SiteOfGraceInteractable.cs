using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TraverserProject
{

    public class SiteOfGraceInteractable : Interactable
    {
        [Header("Site Of Grace Info")]
        public int siteOfGraceID;
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("VFX")]
        [SerializeField] GameObject activatedParticles;

        [Header("Interaction Text")]
        [SerializeField] string unactivatedInteractionText = "Restore Site Of Grace";
        [SerializeField] string activatedInteractionText = "Rest";

        [Header("Teleport Transform")]
        [SerializeField] Transform teleportTransform;


        protected override void Start()
        {
            base.Start();

            if (IsOwner)
            {
                if (WorldSaveGameManager.Singleton.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                {
                    isActivated.Value = WorldSaveGameManager.Singleton.currentCharacterData.sitesOfGrace[siteOfGraceID];
                }
                else
                {
                    isActivated.Value = false;
                }
            }

            if (isActivated.Value)
            {
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                OnIsActivatedChanged(false, isActivated.Value);

            isActivated.OnValueChanged += OnIsActivatedChanged;

            WorldObjectManager.Singleton.AddSiteOfGraceToList(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isActivated.OnValueChanged -= OnIsActivatedChanged;
        }

        private void RestoreSiteOfGrace(PlayerManager player)
        {
            isActivated.Value = true;

            if (WorldSaveGameManager.Singleton.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
                WorldSaveGameManager.Singleton.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);

            WorldSaveGameManager.Singleton.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

            player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);

            PlayerUIManager.Singleton.playerUIPopUpManager.SendGraceRestoredPopUp("SITE OF GRACE RESTORED");

            StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());

        }

        private void RestAtSiteOfGrace(PlayerManager player)
        {
            PlayerUIManager.Singleton.playerUISiteOfGraceManager.OpenSiteOfGraceManagerMenu();
            //temp
            interactableCollider.enabled = true;
            player.playerNetworkManager.currentHealth.Value = player.playerNetworkManager.maxHealth.Value;
            player.playerNetworkManager.currentStamina.Value = player.playerNetworkManager.maxStamina.Value;



            WorldAIManager.Singleton.ResetAllCharacters();
        }

        private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()
        {
            yield return new WaitForSeconds(2);
            interactableCollider.enabled = true;
        }

        private void OnIsActivatedChanged(bool oldStatus, bool newStatus)
        {
            if (isActivated.Value)
            {
                activatedParticles.SetActive(true);
                interactableText = activatedInteractionText;
            }
            else
            {
                interactableText = unactivatedInteractionText;
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (player.isPerformingAction)
                return;

            if (player.playerCombatManager.isUsingItem)
                return;

            if (!isActivated.Value)
            {
                RestoreSiteOfGrace(player);
            }
            else
            {
                RestAtSiteOfGrace(player);
            }
        }

        public void TeleportToSiteOfGrace()
        {
            PlayerManager player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();

            PlayerUIManager.Singleton.playerUILoadingScreenManager.ActivateLoadingScreen();

            player.transform.position = teleportTransform.position;
            Physics.SyncTransforms();

            PlayerUIManager.Singleton.playerUILoadingScreenManager.DeactivateLoadingScreen(1);
        }

    }
}