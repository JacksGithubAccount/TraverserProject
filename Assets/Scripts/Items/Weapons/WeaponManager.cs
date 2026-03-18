using UnityEngine;

namespace TraverserProject
{

    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapon Trail")]
        //pick one, use both if crazy enough
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] ParticleSystem WeaponTrailVFX;

        [Header("Collider")]
        public MeleeWeaponDamageCollider meleeDamageCollider;


        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

        }


        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            if (meleeDamageCollider == null)
                return;

            int upgradeLevel = (int)weapon.upgradeLevel;
            int upgradeDamage = 0;

            for (int i = 0; i < upgradeLevel; i++)
            {
                if (i >= 1)
                    upgradeDamage += 11;
            }

            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;

            int physicalDamage = weapon.physicalDamage;
            if (physicalDamage > 0)
                physicalDamage += upgradeDamage;
            meleeDamageCollider.physicalDamage = physicalDamage;

            int magicDamage = weapon.magicDamage;
            if (magicDamage > 0)
                magicDamage += upgradeDamage;
            meleeDamageCollider.magicDamage = magicDamage;

            int fireDamage = weapon.fireDamage;
            if (fireDamage > 0)
                fireDamage += upgradeDamage;
            meleeDamageCollider.fireDamage = fireDamage;

            int lightningDamage = weapon.lightningDamage;
            if (lightningDamage > 0)
                lightningDamage += upgradeDamage;
            meleeDamageCollider.lightningDamage = lightningDamage;

            int holyDamage = weapon.holyDamage;
            if (holyDamage > 0)
                holyDamage += upgradeDamage;
            meleeDamageCollider.holyDamage = holyDamage;

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

            meleeDamageCollider.dual_Light_Attack_01_Modifier = weapon.dual_Light_Attack_01_Modifier;
            meleeDamageCollider.dual_Light_Attack_02_Modifier = weapon.dual_Light_Attack_02_Modifier;
            meleeDamageCollider.dual_Heavy_Attack_01_Modifier = weapon.dual_Heavy_Attack_01_Modifier;
            meleeDamageCollider.dual_Heavy_Attack_02_Modifier = weapon.dual_Heavy_Attack_02_Modifier;
            meleeDamageCollider.dual_Charge_Attack_01_Modifier = weapon.dual_Charge_Attack_01_Modifier;
            meleeDamageCollider.dual_Charge_Attack_02_Modifier = weapon.dual_Charge_Attack_02_Modifier;
            meleeDamageCollider.dual_Rolling_Light_Attack_01_Modifier = weapon.dual_Rolling_Light_Attack_01_Modifier;
            meleeDamageCollider.dual_Rolling_Heavy_Attack_01_Modifier = weapon.dual_Rolling_Heavy_Attack_01_Modifier;
            meleeDamageCollider.dual_Running_Light_Attack_01_Modifier = weapon.dual_Running_Light_Attack_01_Modifier;
            meleeDamageCollider.dual_Running_Heavy_Attack_01_Modifier = weapon.dual_Running_Heavy_Attack_01_Modifier;
            meleeDamageCollider.dual_Backstep_Light_Attack_01_Modifier = weapon.dual_Backstep_Light_Attack_01_Modifier;
            meleeDamageCollider.dual_Backstep_Heavy_Attack_01_Modifier = weapon.dual_Backstep_Heavy_Attack_01_Modifier;
            meleeDamageCollider.dual_Jumping_Light_Attack_01_Modifier = weapon.dual_Jumping_Light_Attack_01_Modifier;
            meleeDamageCollider.dual_Jumping_Heavy_Attack_01_Modifier = weapon.dual_Jumping_Heavy_Attack_01_Modifier;
        }

        public void ToggleWeaponTrail(bool status)
        {
            if (trailRenderer != null)
                trailRenderer.emitting = status;

            if (WeaponTrailVFX == null)
                return;

            if (status)
            {
                WeaponTrailVFX.Play();
            }
            else
            {
                WeaponTrailVFX.Stop();
            }
        }

    }
}