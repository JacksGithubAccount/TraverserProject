using Unity.Netcode;
using UnityEngine;

namespace TraverserProject
{

    public class ActivateOtherInteractableInteractable : Interactable
    {
        public NetworkVariable<bool> leverHasBeenPulled = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Interactable")]
        [SerializeField] Interactable interactableObject;

        [Header("Use Once")]
        [SerializeField] bool useOnce = true;

        [Header("Animator")]
        [SerializeField] Animator animator;
        [SerializeField] string pullLeverAnimation;
        [SerializeField] string releaseLeverAnimation;
        [SerializeField] string pulledLeverAnimation;

        public override void Interact(PlayerManager player)
        {
            PullLeverServerRpc();

            WorldSaveGameManager.Singleton.SaveGame();

            if (interactableObject == null)
                return;

            interactableObject.Interact(player);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (leverHasBeenPulled.Value)
                animator.Play(pulledLeverAnimation);
        }

        [ServerRpc(RequireOwnership = false)]
        private void PullLeverServerRpc()
        {
            if (IsServer)
            {
                PullLeverClientRpc();
            }


        }
        [ClientRpc]
        private void PullLeverClientRpc()
        {
            if (interactableCollider != null)
                interactableCollider.enabled = false;
            PlayerUIManager.Singleton.localPlayer.playerInteractionManager.RemoveInteractionFromList(this);
            animator.Play(pullLeverAnimation);

            if (IsOwner)
                leverHasBeenPulled.Value = true;

        }



    }
}