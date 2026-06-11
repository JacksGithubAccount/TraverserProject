using TraverserProject;
using UnityEngine;

public class UI_EquipmentSlot : UI_InventorySlot
{
    private void Awake()
    {
        highlightIcon.enabled = false;
    }
    public override void SelectSlot()
    {
        base.SelectSlot();
        PlayerUIManager.Singleton.playerUIInventoryManager.DispayItemDetail(currentItem);
    }
}
