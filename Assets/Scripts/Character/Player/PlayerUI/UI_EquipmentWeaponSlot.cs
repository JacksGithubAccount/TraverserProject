using TraverserProject;
using UnityEngine;

public class UI_EquipmentWeaponSlot : UI_InventorySlot
{
    public override void SelectSlot()
    {
        base.SelectSlot();
        PlayerUIManager.Singleton.playerUIInventoryManager.DispayItemDetail(currentItem);
    }
}
