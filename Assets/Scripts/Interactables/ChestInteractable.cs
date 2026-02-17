using UnityEngine;
using System.Collections;
using Steamworks.Data;
using Unity.VisualScripting;

namespace TraverserProject
{
    public class ChestInteractable : Interactable
    {
        Animator animator;
        public Transform playerStandingPosition;

        [Header("Item in Chest")]
        public GameObject itemInteractable;
        public Item itemInChest;
        public int itemAmount;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }
        public override void Interact(PlayerManager player)
        {
            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
            PlayerUIManager.Singleton.playerUIPopUpManager.CloseAllPopUpWindows();

            //turns player to face interactable object
            Vector3 rotationDirection = transform.position - player.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, 300 * Time.deltaTime);
            player.transform.rotation = targetRotation;

            player.transform.position = playerStandingPosition.transform.position;
            player.playerAnimatorManager.PlayTargetActionAnimation("Open_Chest_01", true);
            animator.Play("Chest_Open_01");

            StartCoroutine(SpawnItemInChest());

            PickUpItemInteractable item = itemInteractable.GetComponent<PickUpItemInteractable>();
            item.itemID.Value = itemInChest.itemID;
            item.item = itemInChest;
            item.itemAmount = itemAmount;
            SphereCollider collider = item.interactableCollider.GetComponent<SphereCollider>();
            collider.radius = 6;
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Transform itemTransform = new GameObject().transform;
            itemTransform.position = new Vector3(transform.position.x,transform.position.y + .8f,transform.position.z);
            //itemTransform.position = new Vector3(0, .8f, 0);
            Instantiate(itemInteractable, itemTransform);
        }
    }
}
