using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Weapon/Weapon Buff")]
    public class ModifyWeaponDamageForATimeEffect : TimedWeaponEffect
    {
        [Header("Weapon Damage")]
        [SerializeField] public WeaponItem weaponToBuff;
        [SerializeField] public int weaponPhysicalDamageModifer = 0;
        [SerializeField] public int weaponMagicDamageModifer = 0;
        [SerializeField] public int weaponFireDamageModifer = 0;
        [SerializeField] public int weaponLightningDamageModifer = 0;
        [SerializeField] public int weaponHolyDamageModifer = 0;

        [Header("Weapon VFX")]
        [SerializeField] public GameObject weaponBuffVFX;

        [Header("Effect Processed")]
        private bool effectHasBeenInitialized = false;

        public override void ProcessEffect(WeaponManager weapon)
        {
            base.ProcessEffect(weapon);

            if (!effectHasBeenInitialized)
            {
                effectHasBeenInitialized = true;
                weaponToBuff.physicalDamageModifier += weaponPhysicalDamageModifer;
                weaponToBuff.magicDamageModifier += weaponMagicDamageModifer;
                weaponToBuff.fireDamageModifier += weaponFireDamageModifer;
                weaponToBuff.lightningDamageModifier += weaponLightningDamageModifer;
                weaponToBuff.holyDamageModifier += weaponHolyDamageModifer;
                if (weaponBuffVFX != null)
                    weaponBuffVFX.SetActive(true);
            }
        }

        public override void RemoveEffect(WeaponManager weapon)
        {
            base.RemoveEffect(weapon);

            if (effectHasBeenInitialized)
            {
                //remove ui icon if implemented
                weaponToBuff.physicalDamageModifier -= weaponPhysicalDamageModifer;
                weaponToBuff.magicDamageModifier -= weaponMagicDamageModifer;
                weaponToBuff.fireDamageModifier -= weaponFireDamageModifer;
                weaponToBuff.lightningDamageModifier -= weaponLightningDamageModifer;
                weaponToBuff.holyDamageModifier -= weaponHolyDamageModifer;
                if(weaponBuffVFX != null)
                    weaponBuffVFX.SetActive(false);
            }
        }
    }
}
