using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Weapon Buff Item")]
    public class WeaponBuffItem : QuickSlotItem
    {
        [Header("Damage Modifier")]
        public int physicalDamageWeaponModifier;
        public int magicDamageWeaponModifier;
        public int fireDamageWeaponModifier;
        public int lightningDamageWeaponModifier;
        public int holyDamageWeaponModifier;

        [Header("Buff Duration")]
        public int buffDuration = 180;

        [Header("VFX")]
        public WeaponBuffVFXType weaponBuffVFX;

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            

            if (physicalDamageWeaponModifier != 0 && magicDamageWeaponModifier != 0 && fireDamageWeaponModifier != 0 &&
                lightningDamageWeaponModifier != 0 && holyDamageWeaponModifier != 0)
            {
                ModifyWeaponDamageForATimeEffect weaponBuff = Instantiate(WorldCharacterEffectsManager.Singleton.weaponBuffEffect);
                WeaponManager weaponManager = null;
                if (player.playerNetworkManager.isTwoHandingLeftWeapon.Value)
                {
                    weaponBuff.weaponToBuff = player.playerInventoryManager.currentTwoHandWeapon;
                    weaponManager = player.playerEquipmentManager.leftWeaponManager;
                }
                else
                {
                    weaponBuff.weaponToBuff = player.playerInventoryManager.currentRightHandWeapon;
                    weaponManager = player.playerEquipmentManager.rightWeaponManager;
                }
                weaponBuff.weaponPhysicalDamageModifer = physicalDamageWeaponModifier;
                weaponBuff.weaponMagicDamageModifer = magicDamageWeaponModifier;
                weaponBuff.weaponFireDamageModifer = fireDamageWeaponModifier;
                weaponBuff.weaponLightningDamageModifer = lightningDamageWeaponModifier;
                weaponBuff.weaponHolyDamageModifer = holyDamageWeaponModifier;
                weaponBuff.defaultLengthOfEffect = buffDuration;

                switch (weaponBuffVFX)
                {
                    case WeaponBuffVFXType.Magic:
                        weaponBuff.weaponBuffVFX = weaponManager.magicWeaponBuffVFX;
                        break;
                    case WeaponBuffVFXType.Fire:
                        weaponBuff.weaponBuffVFX = weaponManager.fireWeaponBuffVFX;
                        break;
                    case WeaponBuffVFXType.Lightning:
                        weaponBuff.weaponBuffVFX = weaponManager.lightningWeaponBuffVFX;
                        break;
                    case WeaponBuffVFXType.Holy:
                        weaponBuff.weaponBuffVFX = weaponManager.holyWeaponBuffVFX;
                        break;
                    default:
                        break;
                }

                if (weaponBuff.weaponToBuff.weaponTimedEffect == null)
                {
                    weaponManager.AddTimedEffect(weaponBuff);
                    weaponBuff.weaponToBuff.weaponTimedEffect = weaponBuff;
                }
                else
                {
                    if (weaponManager.timedEffects.Find(x => x.effectID == WorldCharacterEffectsManager.Singleton.weaponBuffEffect.effectID))
                    {
                        TimedWeaponEffect effect = weaponManager.timedEffects.Find(x => x.effectID == WorldCharacterEffectsManager.Singleton.weaponBuffEffect.effectID);
                        weaponManager.RemoveTimedEffect(effect.effectID);
                    }
                    weaponManager.AddTimedEffect(weaponBuff);
                }

                player.playerStatsManager.CalculateAllWeaponAttackPower();
            }            
        }
    }
}
