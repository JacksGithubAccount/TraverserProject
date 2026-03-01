using UnityEngine;
using Unity.Netcode;
using System.Collections;

namespace TraverserProject
{
    public class ShortcutInteractable : Interactable
    {
        [Header("Animation")]
        [SerializeField] Animator animator;
        [SerializeField] string activateShortcutAnimation;
        [SerializeField] string activatedShortcutAnimation;

        [Header("Shortcut")]
        public NetworkVariable<bool> isActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public int shortcutID;

        [Header("Nonshortcut Interactable")]
        [SerializeField] Interactable nonShortcutInteractable;
        [SerializeField] Interactable[] nonShortcutInteractables;

        [Header("SFX")]
        [SerializeField] AudioSource shortcutAudioSource;
        [SerializeField] AudioClip shortcutActivatingSFX;
        [SerializeField] AudioClip shortcutActivatedSFX;
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (player.IsOwner)
                ActivateShortcutServerRpc();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (WorldSaveGameManager.Singleton.currentCharacterData.shortcutsActivated.ContainsKey(shortcutID))
            {
                isActivated.Value = WorldSaveGameManager.Singleton.currentCharacterData.shortcutsActivated[shortcutID];
            }
            else
            {
                isActivated.Value = false;
            }

            if (isActivated.Value)
            {
                animator.Play(activatedShortcutAnimation);
            }

            if (nonShortcutInteractable != null)
            {
                nonShortcutInteractable.interactableCollider.enabled = false;
            }

            nonShortcutInteractables = GetComponentsInChildren<Interactable>();
            foreach (var i in nonShortcutInteractables)
            {
                if (i.interactableCollider == interactableCollider)
                    continue;

                i.interactableCollider.enabled = false;
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        private void ActivateShortcut()
        {
            StartCoroutine(ActivateShortcutCoroutine());
        }

        private IEnumerator ActivateShortcutCoroutine()
        {
            interactableCollider.enabled = false;
            isActivated.Value = true;

            if (WorldSaveGameManager.Singleton.currentCharacterData.shortcutsActivated.ContainsKey(shortcutID))
                WorldSaveGameManager.Singleton.currentCharacterData.shortcutsActivated.Remove(shortcutID);

            WorldSaveGameManager.Singleton.currentCharacterData.shortcutsActivated.Add(shortcutID, true);



            //SFX
            shortcutAudioSource.clip = shortcutActivatingSFX;
            shortcutAudioSource.Play();

            //moves the door
            animator.Play(activateShortcutAnimation);


            //stops movement SFX
            shortcutAudioSource.Stop();
            //plays stopped SFX
            shortcutAudioSource.clip = shortcutActivatedSFX;
            //shortcutAudioSource.PlayOneShot(WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(doorClosingSFX));

            //if animating elevator, stop animation here

            if (interactableCollider != null)
                interactableCollider.enabled = false;

            foreach (var i in nonShortcutInteractables)
            {
                if (i.interactableCollider == interactableCollider)
                    continue;

                i.interactableCollider.enabled = true;
            }

            yield return null;
        }


        [ServerRpc(RequireOwnership = false)]
        public void ActivateShortcutServerRpc()
        {
            if (IsServer)
                ActivateShortcutClientRpc();
        }

        [ClientRpc]
        private void ActivateShortcutClientRpc()
        {
            ActivateShortcut();            
        }
    }
}