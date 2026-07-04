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

        protected GameObject weaponBuffVFX;

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (physicalDamageWeaponModifier != 0 && magicDamageWeaponModifier != 0 && fireDamageWeaponModifier != 0 &&
                lightningDamageWeaponModifier != 0 && holyDamageWeaponModifier != 0)
            {
                ModifyWeaponDamageForATimeEffect weaponBuff = Instantiate(WorldCharacterEffectsManager.Singleton.weaponBuffEffect);
                if (player.playerNetworkManager.isTwoHandingWeapon.Value)
                {
                    weaponBuff.weaponToBuff = player.playerInventoryManager.currentTwoHandWeapon;
                }
                else
                {
                    weaponBuff.weaponToBuff = player.playerInventoryManager.currentRightHandWeapon;
                }
                weaponBuff.weaponPhysicalDamageModifer = physicalDamageWeaponModifier;
                weaponBuff.weaponMagicDamageModifer = magicDamageWeaponModifier;
                weaponBuff.weaponFireDamageModifer = fireDamageWeaponModifier;
                weaponBuff.weaponLightningDamageModifer = lightningDamageWeaponModifier;
                weaponBuff.weaponHolyDamageModifer = holyDamageWeaponModifier;
                weaponBuff.defaultLengthOfEffect = buffDuration;

                player.playerEffectsManager.AddTimedEffect(weaponBuff);

                player.playerStatsManager.CalculateTotalArmorAbsorption();
            }


            weaponBuffVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
            weaponBuffVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            weaponBuffVFX.transform.root.rotation = Quaternion.identity;
        }
    }
}
