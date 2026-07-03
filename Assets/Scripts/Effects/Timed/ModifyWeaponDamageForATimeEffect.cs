using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Timed Effects/Modify Weapon/Weapon Buff")]
    public class ModifyWeaponDamageForATimeEffect : TimedCharacterEffect
    {
        [Header("Weapon Damage")]
        [SerializeField] public float weaponPhysicalDamageModifer = 0;
        [SerializeField] public float weaponMagicDamageModifer = 0;
        [SerializeField] public float weaponFireDamageModifer = 0;
        [SerializeField] public float weaponLightningDamageModifer = 0;
        [SerializeField] public float weaponHolyDamageModifer = 0;

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
                character.characterNetworkManager.weaponPhysicalDamageModifer.Value += weaponPhysicalDamageModifer;
                character.characterNetworkManager.weaponMagicDamageModifer.Value += weaponMagicDamageModifer;
                character.characterNetworkManager.weaponFireDamageModifer.Value += weaponFireDamageModifer;
                character.characterNetworkManager.weaponLightningDamageModifer.Value += weaponLightningDamageModifer;
                character.characterNetworkManager.weaponHolyDamageModifer.Value += weaponHolyDamageModifer;
            }
        }

        public override void RemoveEffect(CharacterManager character)
        {
            base.RemoveEffect(character);

            if (effectHasBeenInitialized)
            {
                //remove ui icon if implemented
                character.characterNetworkManager.weaponPhysicalDamageModifer.Value -= weaponPhysicalDamageModifer;
                character.characterNetworkManager.weaponMagicDamageModifer.Value -= weaponMagicDamageModifer;
                character.characterNetworkManager.weaponFireDamageModifer.Value -= weaponFireDamageModifer;
                character.characterNetworkManager.weaponLightningDamageModifer.Value -= weaponLightningDamageModifer;
                character.characterNetworkManager.weaponHolyDamageModifer.Value -= weaponHolyDamageModifer;
            }
        }
    }
}
