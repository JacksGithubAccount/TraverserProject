using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;

namespace TraverserProject
{

    public class CallElevatorLeverInteractable : CallElevatorInteractable
    {
        [Header("Animation")]
        [SerializeField] Animator animator;
        [SerializeField] string pullLeverAnimation;
        [SerializeField] string releaseLeverAnimation;
        [SerializeField] float timeToWaitAfterPullingLeverToMoveElevator = 1f;


        [Header("Elevator")]
        [SerializeField] float minimumButtonReleaseTime = 2f;
        public NetworkVariable<bool> leverHasBeenPulled = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        private Coroutine elevatorLeverCoroutine;

        public override void Interact(PlayerManager player)
        {
            ActivateElevatorWithLever();
        }

        private void ActivateElevatorWithLever()
        {
            if (elevator.lowDestinationRecall is CallElevatorLeverInteractable)
            {
                CallElevatorLeverInteractable lever = elevator.lowDestinationRecall as CallElevatorLeverInteractable;

                if (lever.leverHasBeenPulled.Value)
                    return;
            }

            if (elevator.highDestinationRecall is CallElevatorLeverInteractable)
            {
                CallElevatorLeverInteractable lever = elevator.highDestinationRecall as CallElevatorLeverInteractable;

                if (lever.leverHasBeenPulled.Value)
                    return;
            }

            if (elevator.elevatorIsDescending.Value || elevator.elevatorIsRising.Value)
                return;

            PullLeverServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PullLeverServerRpc()
        {
            if (IsServer)
                PullLeverClientRpc();
        }

        [ClientRpc]
        private void PullLeverClientRpc()
        {
            StartCoroutine(WaitForLeverAnimationThenMoveElevator());
		}
		
		
		private IEnumerator WaitForLeverAnimationThenMoveElevator()
        {
            if (IsOwner)
                leverHasBeenPulled.Value = true;

            RemoveInteractionFromPlayers();

            animator.Play(pullLeverAnimation);

            yield return new WaitForSeconds(timeToWaitAfterPullingLeverToMoveElevator);

            if (IsOwner)
                elevator.ActivateElevatorServerRpc();


            //wait for elevator button to be released
            if (elevatorLeverCoroutine != null)
                StopCoroutine(elevatorLeverCoroutine);

            elevatorLeverCoroutine = StartCoroutine(WaitForElevatorLeverToRelease());
        }

        private IEnumerator WaitForElevatorLeverToRelease()
        {
            while (elevator.elevatorIsDescending.Value || elevator.elevatorIsRising.Value)
            {
                yield return null;
            }

            yield return new WaitForSeconds(minimumButtonReleaseTime);

            if (IsOwner)
                leverHasBeenPulled.Value = false;

            animator.Play(releaseLeverAnimation);
        }
    }
}