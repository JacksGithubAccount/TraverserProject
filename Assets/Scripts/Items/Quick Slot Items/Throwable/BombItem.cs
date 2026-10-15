using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Bomb Item")]
    public class BombItem : QuickSlotItem
    {
        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;


            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
                player.playerNetworkManager.HideWeaponsServerRpc(); //optionall, only hide the throw arm
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject bomb = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = bomb;
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (player.playerEffectsManager.activeQuickSlotItemFX == null)
                return;

            BombManager bomb = player.playerEffectsManager.activeQuickSlotItemFX.GetComponentInChildren<BombManager>();

            if (bomb == null)
                return;

            bomb.ThrowBomb(player);
        }

    }
}