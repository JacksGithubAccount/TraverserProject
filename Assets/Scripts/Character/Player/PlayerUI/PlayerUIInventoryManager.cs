using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{
    public class PlayerUIInventoryManager : PlayerUIMenu
    {
        [Header("Inventory")]
        [SerializeField] GameObject inventoryWindow;
        [SerializeField] GameObject inventorySlotPrefab;
        [SerializeField] Transform inventoryContentWindow;
        [SerializeField] Item currentlySelectedItem;
        [HideInInspector] List<GameObject> inventorySlotPrefabs = new List<GameObject>();
        [SerializeField] TextMeshProUGUI categoryNameText;

        [Header("Inventory Category Select")]
        [SerializeField] Scrollbar inventoryCategorySelectScrollbar;
        public int inventoryCategorySelectScrollbarIndex;
        public ItemType currentSelectedInventoryCategorySelectSlot;
        public List<GameObject> inventoryCategorySelectSlotPrefabs = new List<GameObject>();

        [Header("InventorySelectionMenu")]
        [SerializeField] GameObject inventorySelectionMenuWindow;
        [SerializeField] GameObject closeSubmenuWindow;
        [SerializeField] Button inventorySelectionMenuUseTextButton;
        private int selectedInventorySelectionMenuButton; // 0 none, 1 use, 2 drop, 3 discard

        [Header("InventorySelectionAmountMenu")]
        [SerializeField] GameObject inventorySelectionAmountMenuWindow;
        [SerializeField] Slider inventorySelectionAmountSlider;        
        [SerializeField] TextMeshProUGUI inventorySelectionAmountText;

        [Header("Inventory Detail Menu")]
        [SerializeField] GameObject inventoryDetailWindow;
        private Vector3 inventoryDetailWindowPosition = new Vector3(1000, 540, 0);
        [SerializeField] TextMeshProUGUI inventoryDetailItemNameText;
        [SerializeField] Image inventoryDetailImage;
        [SerializeField] TextMeshProUGUI inventoryDetailItemDescriptionText;
        [SerializeField] TextMeshProUGUI inventoryDetailItemSkillDescriptionText;

        [Header("Inventory WeaponItem Stats Menu")]
        [SerializeField] GameObject weaponItemStatsWindow;
        [SerializeField] TextMeshProUGUI weaponItemClassText;
        [SerializeField] TextMeshProUGUI weaponItemPhysicalDamageTypeText;
        [SerializeField] TextMeshProUGUI weaponItemAshOfWarNameText;
        [SerializeField] TextMeshProUGUI weaponItemAshOfWarFPCostText;
        [SerializeField] TextMeshProUGUI weaponItemWeightText;
        [SerializeField] TextMeshProUGUI weaponItemPhysicalAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemMagicAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemFireAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemLightningAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemHolyAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemPhysicalScalingAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemMagicScalingAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemFireScalingAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemLightningScalingAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemHolyScalingAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemCriticalAttackPowerText;
        [SerializeField] TextMeshProUGUI weaponItemPhysicalGuardNegationText;
        [SerializeField] TextMeshProUGUI weaponItemMagicGuardNegationText;
        [SerializeField] TextMeshProUGUI weaponItemFireGuardNegationText;
        [SerializeField] TextMeshProUGUI weaponItemLightningGuardNegationText;
        [SerializeField] TextMeshProUGUI weaponItemHolyGuardNegationText;
        [SerializeField] TextMeshProUGUI weaponItemStabilityText;
        [SerializeField] TextMeshProUGUI weaponItemSTRAttributeREQText;
        [SerializeField] TextMeshProUGUI weaponItemDEXAttributeREQText;
        [SerializeField] TextMeshProUGUI weaponItemINTAttributeREQText;
        [SerializeField] TextMeshProUGUI weaponItemFAIAttributeREQText;
        [SerializeField] TextMeshProUGUI weaponItemSTRAttributeScalingText;
        [SerializeField] TextMeshProUGUI weaponItemDEXAttributeScalingText;
        [SerializeField] TextMeshProUGUI weaponItemINTAttributeScalingText;
        [SerializeField] TextMeshProUGUI weaponItemFAIAttributeScalingText;
        [SerializeField] TextMeshProUGUI weaponItemPassive1Text;
        [SerializeField] TextMeshProUGUI weaponItemPassive2Text;
        [SerializeField] TextMeshProUGUI weaponItemPassive3Text;

        [Header("Inventory ArmorItem Stats Menu")]
        [SerializeField] GameObject armorItemStatsWindow;
        [SerializeField] TextMeshProUGUI armorItemClassText;
        [SerializeField] TextMeshProUGUI armorItemWeightText;
        [SerializeField] TextMeshProUGUI armorItemPhysicalDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemBluntDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemPierceDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemSlashDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemMagicDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemFireDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemLightningDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemHolyDamageNegationText;
        [SerializeField] TextMeshProUGUI armorItemImmunityResistanceText;
        [SerializeField] TextMeshProUGUI armorItemRobustnessResistanceText;
        [SerializeField] TextMeshProUGUI armorItemFocusResistanceText;
        [SerializeField] TextMeshProUGUI armorItemVitalityResistanceText;
        [SerializeField] TextMeshProUGUI armorItemPoiseText;

        [Header("Inventory AccessoryItem Stats Menu")]
        [SerializeField] GameObject accessoryItemStatsWindow;
        [SerializeField] TextMeshProUGUI accessoryItemTypeText;
        [SerializeField] TextMeshProUGUI accessoryItemWeightText;
        [SerializeField] TextMeshProUGUI accessoryItemEffectText;

        [Header("Inventory ToolItem Stats Menu")]
        [SerializeField] GameObject toolItemStatsWindow;
        [SerializeField] TextMeshProUGUI toolItemTypeText;
        [SerializeField] TextMeshProUGUI toolItemNumberHeldText;
        [SerializeField] TextMeshProUGUI toolItemStoredText;
        [SerializeField] TextMeshProUGUI toolItemFPCostText;
        [SerializeField] TextMeshProUGUI toolItemEffectText;
        [SerializeField] TextMeshProUGUI toolItemSTRAttributeScalingText;
        [SerializeField] TextMeshProUGUI toolItemDEXAttributeScalingText;
        [SerializeField] TextMeshProUGUI toolItemINTAttributeScalingText;
        [SerializeField] TextMeshProUGUI toolItemFAIAttributeScalingText;

        [Header("Inventory CraftingItem Stats Menu")]
        [SerializeField] GameObject craftingItemStatsWindow;
        [SerializeField] TextMeshProUGUI craftingItemTypeText;
        [SerializeField] TextMeshProUGUI craftingItemNumberHeldText;
        [SerializeField] TextMeshProUGUI craftingItemStoredText;
        [SerializeField] TextMeshProUGUI craftingItemEffectText;
        [SerializeField] TextMeshProUGUI craftingItemObtainedText;

        [Header("Inventory UpgradeItem Stats Menu")]
        [SerializeField] GameObject upgradeItemStatsWindow;
        [SerializeField] TextMeshProUGUI upgradeItemTypeText;
        [SerializeField] TextMeshProUGUI upgradeItemNumberHeldText;
        [SerializeField] TextMeshProUGUI upgradeItemStoredText;
        [SerializeField] TextMeshProUGUI upgradeItemEffectText;

        [Header("Inventory SpellItem Stats Menu")]
        [SerializeField] GameObject spellItemStatsWindow;
        [SerializeField] TextMeshProUGUI spellItemTypeText;
        [SerializeField] TextMeshProUGUI spellItemNumberHeldText;
        [SerializeField] TextMeshProUGUI spellItemStoredText;
        [SerializeField] TextMeshProUGUI spellItemFPCostText;
        [SerializeField] TextMeshProUGUI spellItemSlotsUsedText;
        [SerializeField] TextMeshProUGUI spellItemEffectText;
        [SerializeField] TextMeshProUGUI spellItemSTRAttributeREQText;
        [SerializeField] TextMeshProUGUI spellItemDEXAttributeREQText;
        [SerializeField] TextMeshProUGUI spellItemINTAttributeREQText;
        [SerializeField] TextMeshProUGUI spellItemFAIAttributeREQText;

        [Header("Inventory AshOfWarItem Stats Menu")]
        [SerializeField] GameObject ashOfWarItemStatsWindow;
        [SerializeField] TextMeshProUGUI ashOfWarItemTypeText;
        [SerializeField] TextMeshProUGUI ashOfWarItemFPCostText;
        [SerializeField] TextMeshProUGUI ashOfWarItemEffectText;
        [SerializeField] TextMeshProUGUI ashOfWarItemUsableOnText;

        [Header("Inventory RangedProjectileItem Stats Menu")]
        [SerializeField] GameObject rangedProjectileItemStatsWindow;
        [SerializeField] TextMeshProUGUI rangedProjectileItemClassText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemPhysicalDamageTypeText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemNumberHeldText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemStoredText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemPhysicalAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemMagicAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemFireAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemLightningAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemHolyAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemCriticalAttackPowerText;
        [SerializeField] TextMeshProUGUI rangedProjectileItemPassive1Text;
        [SerializeField] TextMeshProUGUI rangedProjectileItemPassive2Text;
        [SerializeField] TextMeshProUGUI rangedProjectileItemPassive3Text;


        private void Awake()
        {
            inventoryDetailWindow.SetActive(false);
        }
        public override void OpenMenu()
        {
            base.OpenMenu();

            ToggleInventoryButtons(true);
            PlayerUIManager.Singleton.CloseAllSubMenuWindows();
            //RefreshMenu();      
            inventoryDetailWindow.SetActive(true);
            inventoryDetailWindow.transform.position = inventoryDetailWindowPosition;
            LoadRecentItemsInventory();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
            inventoryDetailWindow.SetActive(false);
        }

        public override void CloseSubMenu()
        {
            base.CloseSubMenu();
            closeSubmenuWindow.SetActive(false);
            ToggleGameObjectPrefabs(inventorySlotPrefabs, true);
            ToggleGameObjectPrefabs(inventoryCategorySelectSlotPrefabs, true);            
        }

        public void ToggleInventoryButtons(bool isEnabled)
        {
            foreach (var gameObject in inventorySlotPrefabs)
            {
                gameObject.SetActive(isEnabled);
            }

        }

        private void DisableAllStatsWindows()
        {
            weaponItemStatsWindow.SetActive(false);
            armorItemStatsWindow.SetActive(false);
            accessoryItemStatsWindow.SetActive(false);
            toolItemStatsWindow.SetActive(false);
            craftingItemStatsWindow.SetActive(false);
            upgradeItemStatsWindow.SetActive(false);
            spellItemStatsWindow.SetActive(false) ;
            ashOfWarItemStatsWindow.SetActive(false);
            rangedProjectileItemStatsWindow.SetActive(false);
        }
        public void DispayItemDetail(Item item)
        {
            if (item == null)
                return;

            inventoryDetailItemNameText.text = item.itemName;
            inventoryDetailImage.sprite = item.itemIcon;
            inventoryDetailItemDescriptionText.text = item.itemDescription;
            if (item as WeaponItem)
            {
                DisableAllStatsWindows();
                weaponItemStatsWindow.SetActive(true);
                
                WeaponItem weaponItem = (WeaponItem)item;
                if (weaponItem.upgradeLevel != UpgradeLevel.Zero)
                    inventoryDetailItemNameText.text = item.itemName + " +" + (int)weaponItem.upgradeLevel;

                inventoryDetailItemSkillDescriptionText.text = weaponItem.ashOfWarAction.ashOfWarDescription;

                weaponItemClassText.text = weaponItem.weaponClass.ToString();
                List<PhysicalDamageType> physicalDamageTypes = new List<PhysicalDamageType>();
                physicalDamageTypes.Add(weaponItem.light_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.light_Attack_02_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.heavy_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.heavy_Attack_02_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.charge_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.charge_Attack_02_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.running_Light_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.running_Heavy_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.rolling_Light_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.rolling_Heavy_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.backstep_Light_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.backstep_Heavy_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.jumping_Light_Attack_01_PhysicalDamageType);
                physicalDamageTypes.Add(weaponItem.jumping_Heavy_Attack_01_PhysicalDamageType);

                List<PhysicalDamageType> exclusivePDT = new List<PhysicalDamageType>();

                foreach(var physicalDamageType in physicalDamageTypes)
                {
                    if(!exclusivePDT.Contains(physicalDamageType))
                    {
                        exclusivePDT.Add(physicalDamageType);
                    }
                }
                string pdtString = "";
                for(int i = 0; i < exclusivePDT.Count; i++)
                {
                    pdtString += exclusivePDT[i].ToString();
                    if(i != exclusivePDT.Count -1)
                    {
                        pdtString += "\\";
                    }
                }

                weaponItemPhysicalDamageTypeText.text = pdtString;
                weaponItemAshOfWarNameText.text = weaponItem.ashOfWarAction.itemName;
                weaponItemAshOfWarFPCostText.text = weaponItem.ashOfWarAction.focusPointCost.ToString();
                weaponItemWeightText.text = weaponItem.itemWeight.ToString();

                weaponItemPhysicalAttackPowerText.text = (weaponItem.physicalBaseDamage + weaponItem.physicalUpgradeDamage).ToString();
                weaponItemMagicAttackPowerText.text = (weaponItem.magicBaseDamage + weaponItem.magicUpgradeDamage).ToString();
                weaponItemFireAttackPowerText.text = (weaponItem.fireBaseDamage + weaponItem.fireUpgradeDamage).ToString();
                weaponItemLightningAttackPowerText.text = (weaponItem.lightningBaseDamage + weaponItem.lightningUpgradeDamage).ToString();
                weaponItemHolyAttackPowerText.text = (weaponItem.holyBaseDamage + weaponItem.holyUpgradeDamage).ToString();
                weaponItemCriticalAttackPowerText.text = weaponItem.CriticalModifier.ToString();

                if (weaponItem.physicalBaseDamage > 0)
                {
                    weaponItemPhysicalScalingAttackPowerText.text = "+" + (weaponItem.physicalScalingDamage + weaponItem.physicalDamageModifier);
                }else if (weaponItem.physicalDamageModifier > 0)
                {
                    weaponItemPhysicalScalingAttackPowerText.text = "+" + weaponItem.physicalDamageModifier;
                }
                else
                {
                    weaponItemPhysicalScalingAttackPowerText.text = "";
                }
                if (weaponItem.magicBaseDamage > 0)
                {
                    weaponItemMagicScalingAttackPowerText.text = "+" + (weaponItem.magicScalingDamage + weaponItem.magicDamageModifier);
                }else if (weaponItem.magicDamageModifier > 0)
                {
                    weaponItemMagicScalingAttackPowerText.text = "+" + weaponItem.magicDamageModifier;
                }
                else
                {
                    weaponItemMagicScalingAttackPowerText.text = "";
                }
                if (weaponItem.fireBaseDamage > 0)
                {
                    weaponItemFireScalingAttackPowerText.text = "+" + (weaponItem.fireScalingDamage + weaponItem.fireDamageModifier);
                }
                else if (weaponItem.fireDamageModifier > 0)
                {
                    weaponItemFireScalingAttackPowerText.text = "+" + weaponItem.fireDamageModifier;
                }
                else
                {
                    weaponItemFireScalingAttackPowerText.text = "";
                }
                if (weaponItem.lightningBaseDamage > 0)
                {
                    weaponItemLightningScalingAttackPowerText.text = "+" + (weaponItem.lightningScalingDamage + weaponItem.lightningDamageModifier);
                }
                else if (weaponItem.lightningDamageModifier > 0)
                {
                    weaponItemLightningScalingAttackPowerText.text = "+" + weaponItem.lightningDamageModifier;
                }
                else
                {
                    weaponItemLightningScalingAttackPowerText.text = "";
                }
                if (weaponItem.holyBaseDamage > 0)
                {
                    weaponItemHolyScalingAttackPowerText.text = "+" + (weaponItem.holyScalingDamage + weaponItem.holyDamageModifier);
                }
                else if (weaponItem.holyDamageModifier > 0)
                {
                    weaponItemHolyScalingAttackPowerText.text = "+" + weaponItem.holyDamageModifier;
                }
                else
                {
                    weaponItemHolyScalingAttackPowerText.text = "";
                }

                weaponItemPhysicalGuardNegationText.text = weaponItem.physicalBaseDamageAbsorption.ToString();
                weaponItemMagicGuardNegationText.text = weaponItem.magicBaseDamageAbsorption.ToString(); 
                weaponItemFireGuardNegationText.text = weaponItem.fireBaseDamageAbsorption.ToString(); 
                weaponItemLightningGuardNegationText.text = weaponItem.lightningBaseDamageAbsorption.ToString(); 
                weaponItemHolyGuardNegationText.text = weaponItem.holyBaseDamageAbsorption.ToString(); 
                weaponItemStabilityText.text = weaponItem.stability.ToString(); ;

                weaponItemSTRAttributeREQText.text = weaponItem.strengthREQ.ToString();
                weaponItemDEXAttributeREQText.text = weaponItem.dexterityREQ.ToString(); 
                weaponItemINTAttributeREQText.text = weaponItem.intelligenceREQ.ToString(); 
                weaponItemFAIAttributeREQText.text = weaponItem.faithREQ.ToString();

                weaponItemSTRAttributeScalingText.text = weaponItem.strengthScaling.ToString();
                weaponItemDEXAttributeScalingText.text = weaponItem.dexterityScaling.ToString(); 
                weaponItemINTAttributeScalingText.text = weaponItem.intelligenceScaling.ToString(); 
                weaponItemFAIAttributeScalingText.text = weaponItem.faithScaling.ToString(); 
            }
            else if(item as ArmorItem)
            {
                DisableAllStatsWindows();
                armorItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                ArmorItem armorItem = (ArmorItem)item;
                armorItemClassText.text = armorItem.itemType.ToString();
                armorItemWeightText.text = armorItem.itemWeight.ToString();

                armorItemPhysicalDamageNegationText.text = armorItem.physicalDamageAbsorption.ToString();
                armorItemBluntDamageNegationText.text = armorItem.bluntDamageAbsorption.ToString();
                armorItemPierceDamageNegationText.text = armorItem.pierceDamageAbsorption.ToString();
                armorItemSlashDamageNegationText.text = armorItem.slashDamageAbsorption.ToString();
                armorItemMagicDamageNegationText.text = armorItem.magicDamageAbsorption.ToString();
                armorItemFireDamageNegationText.text = armorItem.fireDamageAbsorption.ToString();
                armorItemLightningDamageNegationText.text = armorItem.lightningDamageAbsorption.ToString();
                armorItemHolyDamageNegationText.text = armorItem.holyDamageAbsorption.ToString();

                armorItemImmunityResistanceText.text = armorItem.immunity.ToString();
                armorItemRobustnessResistanceText.text = armorItem.robustness.ToString();
                armorItemFocusResistanceText.text = armorItem.focus.ToString();
                armorItemVitalityResistanceText.text = armorItem.vitality.ToString();
                armorItemPoiseText.text = armorItem.poise.ToString();
            }
            else if(item as AccessoryEquipmentItem)
            {
                DisableAllStatsWindows();
                accessoryItemStatsWindow.SetActive(true);
                
                inventoryDetailItemSkillDescriptionText.text = "";
                
                AccessoryEquipmentItem accessoryItem = (AccessoryEquipmentItem)item;
                accessoryItemTypeText.text = accessoryItem.itemType.ToString();
                accessoryItemWeightText.text = accessoryItem.itemWeight.ToString();
                accessoryItemEffectText.text = accessoryItem.itemEffect;
            }
            else if(item as QuickSlotItem)
            {
                DisableAllStatsWindows();
                toolItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                QuickSlotItem quickSlotItem = (QuickSlotItem)item;

                toolItemTypeText.text = quickSlotItem.itemType.ToString();
                toolItemNumberHeldText.text = quickSlotItem.currentItemAmount.ToString() + "/" + quickSlotItem.maxItemAmount.ToString();
                Item itemInStorage = PlayerUIManager.Singleton.localPlayer.playerInventoryManager.GetItemFromStorage(item.itemID);
                int storageitemamount = 0;
                if (itemInStorage != null)
                {
                    storageitemamount = itemInStorage.currentItemAmount;
                }
                toolItemStoredText.text = storageitemamount + "/" + quickSlotItem.maxStorageAmount;
                toolItemFPCostText.text = quickSlotItem.FPCost.ToString();
                toolItemEffectText.text = quickSlotItem.itemEffect;

                toolItemSTRAttributeScalingText.text = quickSlotItem.strengthScaling.ToString();
                toolItemDEXAttributeScalingText.text = quickSlotItem.dexterityScaling.ToString();
                toolItemINTAttributeScalingText.text = quickSlotItem.intelligenceScaling.ToString();
                toolItemFAIAttributeScalingText.text = quickSlotItem.faithScaling.ToString();
            }
            else if(item as CraftingMaterial)
            {
                DisableAllStatsWindows();
                craftingItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                CraftingMaterial craftingMaterial = (CraftingMaterial)item;

                craftingItemTypeText.text = craftingMaterial.itemType.ToString();
                craftingItemNumberHeldText.text = craftingMaterial.currentItemAmount.ToString() + "/" + craftingMaterial.maxItemAmount.ToString();
                craftingItemStoredText.text = "Placeholder";
                craftingItemEffectText.text = craftingMaterial.itemEffect;
                craftingItemObtainedText.text = craftingMaterial.itemObtained;
            }
            else if (item as UpgradeMaterial)
            {
                DisableAllStatsWindows();
                upgradeItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                UpgradeMaterial upgradeMaterial = (UpgradeMaterial)item;

                upgradeItemTypeText.text = upgradeMaterial.itemType.ToString();
                upgradeItemNumberHeldText.text = upgradeMaterial.currentItemAmount.ToString() + "/" + upgradeMaterial.maxItemAmount.ToString();
                upgradeItemStoredText.text = "Placeholder";
                upgradeItemEffectText.text = upgradeMaterial.itemEffect;
            }
            else if (item as KeyItem)
            {
                DisableAllStatsWindows();
                upgradeItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                KeyItem KeyItem = (KeyItem)item;

                upgradeItemTypeText.text = KeyItem.itemType.ToString();
                upgradeItemNumberHeldText.text = KeyItem.currentItemAmount.ToString() + "/" + KeyItem.maxItemAmount.ToString();
                upgradeItemStoredText.text = "Placeholder";
                upgradeItemEffectText.text = KeyItem.itemEffect;
            }
            else if (item as SpellItem)
            {
                DisableAllStatsWindows();
                spellItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                SpellItem spellItem = (SpellItem)item;

                spellItemTypeText.text = spellItem.spellClass.ToString();
                spellItemNumberHeldText.text = spellItem.currentItemAmount.ToString() + "/" + spellItem.maxItemAmount.ToString();
                spellItemStoredText.text = "Placeholder";
                spellItemFPCostText.text = spellItem.focusPointCost.ToString();
                spellItemSlotsUsedText.text = spellItem.spellSlotsUsed.ToString();
                spellItemEffectText.text = spellItem.itemEffect;

                spellItemSTRAttributeREQText.text = spellItem.strengthREQ.ToString();
                spellItemDEXAttributeREQText.text = spellItem.dexterityREQ.ToString();
                spellItemINTAttributeREQText.text = spellItem.intelligenceREQ.ToString();
                spellItemFAIAttributeREQText.text = spellItem.faithREQ.ToString();
            }
            else if (item as AshOfWar)
            {
                DisableAllStatsWindows();
                ashOfWarItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                AshOfWar aowItem = (AshOfWar)item;

                ashOfWarItemTypeText.text = aowItem.itemType.ToString();
                ashOfWarItemFPCostText.text = aowItem.focusPointCost.ToString();
                ashOfWarItemEffectText.text = aowItem.itemEffect;

                ashOfWarItemUsableOnText.text = "";
                for(int i = 0; i < aowItem.usableWeaponClasses.Length; i++)
                {
                    ashOfWarItemUsableOnText.text += aowItem.usableWeaponClasses[i].ToString();

                    if(i <  aowItem.usableWeaponClasses.Length - 1)
                    {
                        ashOfWarItemUsableOnText.text += ", ";
                    }

                }
            }
            if (item as RangedProjectileItem)
            {
                DisableAllStatsWindows();
                rangedProjectileItemStatsWindow.SetActive(true);

                inventoryDetailItemSkillDescriptionText.text = "";

                RangedProjectileItem rangedProjectileItem = (RangedProjectileItem)item;

                rangedProjectileItemClassText.text = rangedProjectileItem.itemType.ToString();
                rangedProjectileItemPhysicalDamageTypeText.text = rangedProjectileItem.physicalDamageType.ToString();
                rangedProjectileItemNumberHeldText.text = rangedProjectileItem.currentAmmoAmount.ToString() + "/" + rangedProjectileItem.maxAmmoAmount.ToString();
                rangedProjectileItemStoredText.text = "Placeholder";

                rangedProjectileItemPhysicalAttackPowerText.text = rangedProjectileItem.physicalDamage.ToString();
                rangedProjectileItemMagicAttackPowerText.text = rangedProjectileItem.magicDamage.ToString();
                rangedProjectileItemFireAttackPowerText.text = rangedProjectileItem.fireDamage.ToString();
                rangedProjectileItemLightningAttackPowerText.text = rangedProjectileItem.lightningDamage.ToString();
                rangedProjectileItemHolyAttackPowerText.text = rangedProjectileItem.holyDamage.ToString();
                rangedProjectileItemCriticalAttackPowerText.text = rangedProjectileItem.CriticalModifier.ToString();
            }
            else
            {
                inventoryDetailItemSkillDescriptionText.text = "";
            }
        }

        //Inventory Category Select
        public void LoadRecentItemsInventory()
        {
            ClearInventorySlotPrefabs();
            categoryNameText.text = "Recent Items";
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<Item> itemsInInventory = new List<Item>();

            for (int i = player.playerInventoryManager.itemsInInventory.Count - 1; i > 0; i--)
            {
                Item item = player.playerInventoryManager.itemsInInventory[i];

                if (item != null)
                    itemsInInventory.Add(item);
            }

            if (itemsInInventory.Count <= 0)
            {
                inventoryWindow.SetActive(false);
                ToggleInventoryButtons(true);
                //RefreshMenu();
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(inventorySlotPrefab, inventoryContentWindow);
                UI_PlayerInventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_PlayerInventorySlot>();
                inventorySlot.AddItem(itemsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                inventorySlot.CurrentItemAmountText.enabled = false;
                if (inventorySlot.currentItem.maxItemAmount > 1)
                {
                    inventorySlot.CurrentItemAmountText.text = "x" + inventorySlot.currentItem.currentItemAmount;
                    inventorySlot.CurrentItemAmountText.enabled = true;
                }

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }

        public void LoadInventoryBasedOnItemType(ItemType itemType)
        {
            ClearInventorySlotPrefabs();
            categoryNameText.text = itemType.ToString();
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            List<Item> itemsInInventory = new List<Item>();

            for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
            {                
                Item item = player.playerInventoryManager.itemsInInventory[i];

                if (item == null)
                    continue;

                if(item.itemType == itemType)
                    itemsInInventory.Add(item);
            }

            if (itemsInInventory.Count <= 0)
            {
                return;
            }

            bool hasSelectedFirstInventorySlot = false;

            for (int i = 0; i < itemsInInventory.Count; i++)
            {
                GameObject inventorySlotGameObject = Instantiate(inventorySlotPrefab, inventoryContentWindow);
                UI_PlayerInventorySlot inventorySlot = inventorySlotGameObject.GetComponent<UI_PlayerInventorySlot>();
                inventorySlot.AddItem(itemsInInventory[i]);
                inventorySlotPrefabs.Add(inventorySlot.gameObject);

                inventorySlot.CurrentItemAmountText.enabled = false;
                if (inventorySlot.currentItem.currentItemAmount > 1)
                {
                    inventorySlot.CurrentItemAmountText.text = "x" + inventorySlot.currentItem.currentItemAmount;
                    inventorySlot.CurrentItemAmountText.enabled = true;
                }

                if (!hasSelectedFirstInventorySlot)
                {
                    hasSelectedFirstInventorySlot = true;
                    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                    inventorySlotButton.Select();
                    inventorySlotButton.OnSelect(null);

                }
            }
        }


        public void SelectInventoryCategorySelectSlot(int slotNumber)
        {
            currentSelectedInventoryCategorySelectSlot = (ItemType)slotNumber;            
        }

        public void ChangeSelectedInventoryCategorySelectSlot(int slotNumber)
        {
            Button button = inventoryCategorySelectSlotPrefabs[slotNumber].GetComponent<Button>();
            button.Select();
            button.OnSelect(null);
        }

        private void ClearInventorySlotPrefabs()
        {
            foreach (GameObject item in inventorySlotPrefabs)
            {
                Destroy(item);
            }
            inventorySlotPrefabs.Clear();
        }

        public void OpenInventorySelectionMenu(UI_PlayerInventorySlot itemSlot)
        {
            currentlySelectedItem = itemSlot.currentItem;

            if (currentlySelectedItem.itemType != ItemType.Tool)
                inventorySelectionMenuUseTextButton.interactable = false;
            else
                inventorySelectionMenuUseTextButton.interactable = true;

            closeSubmenuWindow.SetActive(true);
            OpenSubMenu(inventorySelectionMenuWindow);
            ToggleGameObjectPrefabs(inventorySlotPrefabs, false);
            ToggleGameObjectPrefabs(inventoryCategorySelectSlotPrefabs, false);
            foreach(var slot in inventorySlotPrefabs)
            {
                UI_PlayerInventorySlot islot = slot.GetComponent<UI_PlayerInventorySlot>();
                islot.greyedOutIcon.enabled = false;            
            }
            itemSlot.GlowIcon.enabled = true;

            RectTransform imageRectTransform = itemSlot.GetComponent<RectTransform>();
            
            RectTransform menuWindowRectTransform = inventorySelectionMenuWindow.GetComponent<RectTransform>();
            inventorySelectionMenuWindow.transform.position = new Vector3(imageRectTransform.transform.position.x + imageRectTransform.rect.width * 2, imageRectTransform.transform.position.y, inventorySelectionMenuWindow.transform.position.z);

            
        }

        public void AttemptToOpenInventorySelectionAmountMenu()
        {
            //consider usable souls
            if (selectedInventorySelectionMenuButton == 1)
            {
                if (currentlySelectedItem.GetType() != typeof(BubblesItem))
                {
                    ConfirmInventorySelectionAmount();
                    return;
                }
            }

            OpenSubMenu(inventorySelectionAmountMenuWindow);

            RectTransform imageRectTransform = inventorySelectionMenuWindow.GetComponent<RectTransform>();
            RectTransform menuWindowRectTransform = inventorySelectionAmountMenuWindow.GetComponent<RectTransform>();
            inventorySelectionAmountMenuWindow.transform.position = new Vector3(inventorySelectionMenuWindow.transform.position.x + imageRectTransform.rect.width, imageRectTransform.transform.position.y, inventorySelectionAmountMenuWindow.transform.position.z);

            inventorySelectionAmountSlider.value = 1;
            inventorySelectionAmountSlider.maxValue = currentlySelectedItem.currentItemAmount;
        }

        public void SelectInventorySelectionMenuButton(int number)
        {
            selectedInventorySelectionMenuButton = number;
        }

        public void ConfirmInventorySelectionAmount()
        {
            Item item = Instantiate(currentlySelectedItem);
            item.currentItemAmount = (int)inventorySelectionAmountSlider.value;
            

            if (selectedInventorySelectionMenuButton == 0)
                return;
            else if (selectedInventorySelectionMenuButton == 1)
                UseSelectedItem();
            else if (selectedInventorySelectionMenuButton == 2)
                DropSelectedItem(item);
            else if (selectedInventorySelectionMenuButton == 3)
                DiscardSelectedItem(item);
        }

        public void UpdateSliderValue()
        {
            PlayerManager player = PlayerUIManager.Singleton.localPlayer;

            inventorySelectionAmountText.text = "x" + inventorySelectionAmountSlider.value.ToString();

            //if(inventorySelectionAmountSlider.value > currentlySelectedItem.currentItemAmount)
            //    inventorySelectionAmountSlider.value = currentlySelectedItem.currentItemAmount;
        }



        public void UseSelectedItem()
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            QuickSlotItem qsItem = currentlySelectedItem as QuickSlotItem;
            if (qsItem == null)
                return;

            qsItem.numberOfItemsToUse = (int)inventorySelectionAmountSlider.value;
            player.playerInventoryManager.menuSelectedQuickSlotItem = qsItem;

            qsItem.AttemptToUseItem(player);
            player.playerNetworkManager.NotifyTheServerOfQuickSlotItemActionServerRpc(NetworkManager.Singleton.LocalClientId, qsItem.itemID);
        }

        public void DropSelectedItem(Item item)
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerInventoryManager.DropItemFromInventory(item);
        }

        public void DiscardSelectedItem(Item item)
        {
            CloseSubMenu();
            PlayerUIManager.Singleton.CloseAllMenuWindows();

            PlayerManager player = PlayerUIManager.Singleton.localPlayer;
            player.playerInventoryManager.RemoveItemFromQuickSlotOrInventory(item);
        }

    }

}