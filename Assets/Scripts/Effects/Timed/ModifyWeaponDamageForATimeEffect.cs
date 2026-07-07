using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Weapon/Weapon Buff")]
    public class ModifyWeaponDamageForATimeEffect : TimedCharacterEffect
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

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (!effectHasBeenInitialized)
            {
                //toggle some UI icon on character HP bar or player hud if is owner
                if (!character.IsOwner)
                    return;

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

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

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
