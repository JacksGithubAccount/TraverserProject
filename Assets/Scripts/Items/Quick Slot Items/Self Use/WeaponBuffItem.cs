using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Items/Consumables/Weapon Buff Item")]
    public class WeaponBuffItem : QuickSlotItem
    {
        [Header("Damage Modifier")]
        public float physicalDamageWeaponModifier;
        public float magicDamageWeaponModifier;
        public float fireDamageWeaponModifier;
        public float lightningDamageWeaponModifier;
        public float holyDamageWeaponModifier;

        [Header("Buff Duration")]
        public int buffDuration = 180;

        protected GameObject weaponBuffVFX;

        public override void SuccessfullyUseItem(PlayerManager player)
        {
            base.SuccessfullyUseItem(player);

            if (physicalDamageWeaponModifier != 0 && magicDamageWeaponModifier != 0 && fireDamageWeaponModifier != 0 &&
                lightningDamageWeaponModifier != 0 && holyDamageWeaponModifier != 0)
            {
                ModifyArmorAbsorptionForATimeEffect absorptionBuff = Instantiate(WorldCharacterEffectsManager.Singleton.itemAbsorptionBuffEffect);
                absorptionBuff.defaultLengthOfEffect = buffDuration;

                player.playerEffectsManager.AddTimedEffect(absorptionBuff);

                player.playerStatsManager.CalculateTotalArmorAbsorption();
            }


            weaponBuffVFX = Instantiate(WorldCharacterEffectsManager.Singleton.poisonCureVFX);
            weaponBuffVFX.transform.position = player.playerEffectsManager.effectTransform.position;
            weaponBuffVFX.transform.root.rotation = Quaternion.identity;
        }
    }
}
