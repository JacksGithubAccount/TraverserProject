using UnityEngine;

namespace TraverserProject
{

    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;


        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            if (meleeDamageCollider == null)
                return;

            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;
            meleeDamageCollider.poiseDamage = weapon.poiseDamage;

            meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
            meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;
            meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
            meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;
            meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
            meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;
            meleeDamageCollider.rolling_Light_Attack_01_Modifier = weapon.rolling_Light_Attack_01_Modifier;
            meleeDamageCollider.rolling_Heavy_Attack_01_Modifier = weapon.rolling_Heavy_Attack_01_Modifier;
            meleeDamageCollider.running_Light_Attack_01_Modifier = weapon.running_Light_Attack_01_Modifier;
            meleeDamageCollider.running_Heavy_Attack_01_Modifier = weapon.running_Heavy_Attack_01_Modifier;
            meleeDamageCollider.backstep_Light_Attack_01_Modifier = weapon.backstep_Light_Attack_01_Modifier;
            meleeDamageCollider.backstep_Heavy_Attack_01_Modifier = weapon.backstep_Heavy_Attack_01_Modifier;
            meleeDamageCollider.jumping_Light_Attack_01_Modifier = weapon.jumping_Light_Attack_01_Modifier;
            meleeDamageCollider.jumping_Heavy_Attack_01_Modifier = weapon.jumping_Heavy_Attack_01_Modifier;
        }

    }
}