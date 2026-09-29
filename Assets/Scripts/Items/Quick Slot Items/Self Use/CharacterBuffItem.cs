using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Character Buff Item")]
    public class CharacterBuffItem : QuickSlotItem
    {
        [Header("VFX")]
        [SerializeField] GameObject visualFX;

        [Header("Timed Effects")]
        [SerializeField] TimedCharacterEffect[] characterEffects;

        public override void AttemptToUseItem(PlayerManager player)
        {
            if (!CanIUseThisItem(player))
                return;


            if (player.IsOwner)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true);
                player.playerNetworkManager.HideWeaponsServerRpc();
            }

            Destroy(player.playerEffectsManager.activeQuickSlotItemFX);
            GameObject bag = Instantiate(itemModel, player.playerEquipmentManager.rightHandWeaponSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFX = bag;
        }

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            Instantiate(visualFX, player.transform);

            for (int i = 0; i < characterEffects.Length; i++)
            {
                if (characterEffects[i] == null)
                    continue;

                player.playerEffectsManager.AddTimedEffect(Instantiate(characterEffects[i]));
            }
        }


    }
}