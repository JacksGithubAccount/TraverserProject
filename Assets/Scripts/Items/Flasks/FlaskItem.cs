using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : QuickSlotItem
    {
        [Header("EmptyItem")]
        [SerializeField] GameObject emptyFlaskItem;

	public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;

            player.playerEffectsManager.activeQuickSlotItemFX = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);

            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true, false, true, true, false);
        }
    }
}