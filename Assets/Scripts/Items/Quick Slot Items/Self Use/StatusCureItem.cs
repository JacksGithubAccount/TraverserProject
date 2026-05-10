using TraverserProject;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Status Cure Item")]
public class StatusCureItem : QuickSlotItem
{
    [Header("Build Up Type to cure")]
    public BuildUp buildUp;

    protected GameObject statusCureVFX;


    public override void SuccessfullyUseItem(PlayerManager player)
    {
        base.SuccessfullyUseItem(player);

        if (player.IsOwner)
        {
            currentItemAmount--;
            player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex].currentItemAmount--;
            PlayerUIManager.Singleton.playerUIHudManager.SetQuickSlotItemQuickSlotIcon(player.playerInventoryManager.currentQuickSlotItem);

            //if out of items, remove from quickslot and current item
            if (currentItemAmount <= 0)
            {
                player.playerInventoryManager.quickSlotItemsInQuickSlots[player.playerInventoryManager.quickSlotItemIndex] = null;
                player.playerNetworkManager.currentQuickSlotItemID.Value = -1;
            }

        }

        

        switch (buildUp)
        {
            case BuildUp.Poison:
                statusCureVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
                player.playerNetworkManager.poisonBuildUp.Value = 0;
                player.playerNetworkManager.isPoisoned.Value = false;
                break;
            case BuildUp.Bleed:
                statusCureVFX = Instantiate(WorldCharacterEffectsManager.Singleton.bloodLossCureVFX);
                player.playerNetworkManager.bleedBuildUp.Value = 0;
                player.playerNetworkManager.isBloodLoss.Value = false;
                break;
            case BuildUp.Frost:
                statusCureVFX = Instantiate(WorldCharacterEffectsManager.Singleton.frostbiteCureVFX);
                player.playerNetworkManager.frostBuildUp.Value = 0;
                player.playerNetworkManager.isFrostbite.Value = false;
                break;
            default:
                break;
        }

        statusCureVFX.transform.position = player.playerEffectsManager.effectTransform.position;
        statusCureVFX.transform.root.rotation = Quaternion.identity;




    }

}
