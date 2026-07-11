using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TraverserProject
{

    public class WeaponManager : MonoBehaviour
    {
        [Header("Weapon Trail")]
        //pick one, use both if crazy enough
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] ParticleSystem WeaponTrailVFX;

        [Header("Weapon Buff VFX")]
        [SerializeField] public GameObject magicWeaponBuffVFX;
        [SerializeField] public GameObject fireWeaponBuffVFX;        
        [SerializeField] public GameObject lightningWeaponBuffVFX;
        [SerializeField] public GameObject holyWeaponBuffVFX;        

        [Header("Collider")]
        public MeleeWeaponDamageCollider meleeDamageCollider;

        [Header("Flags")]
        public bool isMainHand = true; //used to power stance check physical damage types

        [Header("Timed Effects")]
        [SerializeField] protected float effectTickTimer = 0;
        [SerializeField] protected float defaultEffectTickTime = 1;
        public List<TimedWeaponEffect> timedEffects = new List<TimedWeaponEffect>();


        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

        }

        protected virtual void Update()
        {
            effectTickTimer -= Time.deltaTime;

            if (effectTickTimer <= 0)
            {
                effectTickTimer = defaultEffectTickTime;
                ProcessTimedEffects();
            }
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            if (meleeDamageCollider == null)
                return;

            //upgrade damage
            int upgradeLevel = (int)weapon.upgradeLevel;
            int upgradeDamage = 0;
            for (int i = 0; i <= upgradeLevel; i++)
            {
                if (i >= 1)
                    upgradeDamage += 11;
            }

            meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;

            int physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.physicalDamage = physicalDamage;

            int magicDamage = weapon.magicDamage;
            meleeDamageCollider.magicDamage = magicDamage;

            int fireDamage = weapon.fireDamage;
            meleeDamageCollider.fireDamage = fireDamage;

            int lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.lightningDamage = lightningDamage;

            int holyDamage = weapon.holyDamage;
            meleeDamageCollider.holyDamage = holyDamage;

            meleeDamageCollider.poiseDamage = weapon.poiseDamage;

            meleeDamageCollider.light_Attack_01_Modifier = weapon.light_Attack_01_Modifier;
            meleeDamageCollider.light_Attack_01_PhysicalDamageType = weapon.light_Attack_01_PhysicalDamageType;
            meleeDamageCollider.light_Attack_02_Modifier = weapon.light_Attack_02_Modifier;
            meleeDamageCollider.light_Attack_02_PhysicalDamageType = weapon.light_Attack_02_PhysicalDamageType;
            meleeDamageCollider.heavy_Attack_01_Modifier = weapon.heavy_Attack_01_Modifier;
            meleeDamageCollider.heavy_Attack_01_PhysicalDamageType = weapon.heavy_Attack_01_PhysicalDamageType;
            meleeDamageCollider.heavy_Attack_02_Modifier = weapon.heavy_Attack_02_Modifier;
            meleeDamageCollider.heavy_Attack_02_PhysicalDamageType = weapon.heavy_Attack_02_PhysicalDamageType;
            meleeDamageCollider.charge_Attack_01_Modifier = weapon.charge_Attack_01_Modifier;
            meleeDamageCollider.charge_Attack_01_PhysicalDamageType = weapon.charge_Attack_01_PhysicalDamageType;
            meleeDamageCollider.charge_Attack_02_Modifier = weapon.charge_Attack_02_Modifier;
            meleeDamageCollider.charge_Attack_02_PhysicalDamageType = weapon.charge_Attack_02_PhysicalDamageType;
            meleeDamageCollider.rolling_Light_Attack_01_Modifier = weapon.rolling_Light_Attack_01_Modifier;
            meleeDamageCollider.rolling_Light_Attack_01_PhysicalDamageType = weapon.rolling_Light_Attack_01_PhysicalDamageType;
            meleeDamageCollider.rolling_Heavy_Attack_01_Modifier = weapon.rolling_Heavy_Attack_01_Modifier;
            meleeDamageCollider.rolling_Heavy_Attack_01_PhysicalDamageType = weapon.rolling_Heavy_Attack_01_PhysicalDamageType;
            meleeDamageCollider.running_Light_Attack_01_Modifier = weapon.running_Light_Attack_01_Modifier;
            meleeDamageCollider.running_Light_Attack_01_PhysicalDamageType = weapon.running_Light_Attack_01_PhysicalDamageType;
            meleeDamageCollider.running_Heavy_Attack_01_Modifier = weapon.running_Heavy_Attack_01_Modifier;
            meleeDamageCollider.running_Heavy_Attack_01_PhysicalDamageType = weapon.running_Heavy_Attack_01_PhysicalDamageType;
            meleeDamageCollider.backstep_Light_Attack_01_Modifier = weapon.backstep_Light_Attack_01_Modifier;
            meleeDamageCollider.backstep_Light_Attack_01_PhysicalDamageType = weapon.backstep_Light_Attack_01_PhysicalDamageType;
            meleeDamageCollider.backstep_Heavy_Attack_01_Modifier = weapon.backstep_Heavy_Attack_01_Modifier;
            meleeDamageCollider.backstep_Heavy_Attack_01_PhysicalDamageType = weapon.backstep_Heavy_Attack_01_PhysicalDamageType;
            meleeDamageCollider.jumping_Light_Attack_01_Modifier = weapon.jumping_Light_Attack_01_Modifier;
            meleeDamageCollider.jumping_Light_Attack_01_PhysicalDamageType = weapon.jumping_Light_Attack_01_PhysicalDamageType;
            meleeDamageCollider.jumping_Heavy_Attack_01_Modifier = weapon.jumping_Heavy_Attack_01_Modifier;
            meleeDamageCollider.jumping_Heavy_Attack_01_PhysicalDamageType = weapon.jumping_Heavy_Attack_01_PhysicalDamageType;

            if (isMainHand)
            {
                meleeDamageCollider.dual_Light_Attack_01_Modifier = weapon.dual_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Light_Attack_01_PhysicalDamageType = weapon.dual_Light_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Light_Attack_02_Modifier = weapon.dual_Light_Attack_02_Modifier;
                meleeDamageCollider.dual_Light_Attack_02_PhysicalDamageType = weapon.dual_Light_Attack_Main_02_PhysicalDamageType;
                meleeDamageCollider.dual_Heavy_Attack_01_Modifier = weapon.dual_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Heavy_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Heavy_Attack_02_Modifier = weapon.dual_Heavy_Attack_02_Modifier;
                meleeDamageCollider.dual_Heavy_Attack_02_PhysicalDamageType = weapon.dual_Heavy_Attack_Main_02_PhysicalDamageType;
                meleeDamageCollider.dual_Charge_Attack_01_Modifier = weapon.dual_Charge_Attack_01_Modifier;
                meleeDamageCollider.dual_Charge_Attack_01_PhysicalDamageType = weapon.dual_Charge_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Charge_Attack_02_Modifier = weapon.dual_Charge_Attack_02_Modifier;
                meleeDamageCollider.dual_Charge_Attack_02_PhysicalDamageType = weapon.dual_Charge_Attack_Main_02_PhysicalDamageType;
                meleeDamageCollider.dual_Rolling_Light_Attack_01_Modifier = weapon.dual_Rolling_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Rolling_Light_Attack_01_PhysicalDamageType = weapon.dual_Rolling_Light_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Rolling_Heavy_Attack_01_Modifier = weapon.dual_Rolling_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Rolling_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Rolling_Heavy_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Running_Light_Attack_01_Modifier = weapon.dual_Running_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Running_Light_Attack_01_PhysicalDamageType = weapon.dual_Running_Light_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Running_Heavy_Attack_01_Modifier = weapon.dual_Running_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Running_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Running_Heavy_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Backstep_Light_Attack_01_Modifier = weapon.dual_Backstep_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Backstep_Light_Attack_01_PhysicalDamageType = weapon.dual_Backstep_Light_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Backstep_Heavy_Attack_01_Modifier = weapon.dual_Backstep_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Backstep_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Backstep_Heavy_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Jumping_Light_Attack_01_Modifier = weapon.dual_Jumping_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Jumping_Light_Attack_01_PhysicalDamageType = weapon.dual_Jumping_Light_Attack_Main_01_PhysicalDamageType;
                meleeDamageCollider.dual_Jumping_Heavy_Attack_01_Modifier = weapon.dual_Jumping_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Jumping_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Jumping_Heavy_Attack_Main_01_PhysicalDamageType;
            }
            else
            {
                meleeDamageCollider.dual_Light_Attack_01_Modifier = weapon.dual_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Light_Attack_01_PhysicalDamageType = weapon.dual_Light_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Light_Attack_02_Modifier = weapon.dual_Light_Attack_02_Modifier;
                meleeDamageCollider.dual_Light_Attack_02_PhysicalDamageType = weapon.dual_Light_Attack_Off_02_PhysicalDamageType;
                meleeDamageCollider.dual_Heavy_Attack_01_Modifier = weapon.dual_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Heavy_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Heavy_Attack_02_Modifier = weapon.dual_Heavy_Attack_02_Modifier;
                meleeDamageCollider.dual_Heavy_Attack_02_PhysicalDamageType = weapon.dual_Heavy_Attack_Off_02_PhysicalDamageType;
                meleeDamageCollider.dual_Charge_Attack_01_Modifier = weapon.dual_Charge_Attack_01_Modifier;
                meleeDamageCollider.dual_Charge_Attack_01_PhysicalDamageType = weapon.dual_Charge_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Charge_Attack_02_Modifier = weapon.dual_Charge_Attack_02_Modifier;
                meleeDamageCollider.dual_Charge_Attack_02_PhysicalDamageType = weapon.dual_Charge_Attack_Off_02_PhysicalDamageType;
                meleeDamageCollider.dual_Rolling_Light_Attack_01_Modifier = weapon.dual_Rolling_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Rolling_Light_Attack_01_PhysicalDamageType = weapon.dual_Rolling_Light_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Rolling_Heavy_Attack_01_Modifier = weapon.dual_Rolling_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Rolling_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Rolling_Heavy_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Running_Light_Attack_01_Modifier = weapon.dual_Running_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Running_Light_Attack_01_PhysicalDamageType = weapon.dual_Running_Light_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Running_Heavy_Attack_01_Modifier = weapon.dual_Running_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Running_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Running_Heavy_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Backstep_Light_Attack_01_Modifier = weapon.dual_Backstep_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Backstep_Light_Attack_01_PhysicalDamageType = weapon.dual_Backstep_Light_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Backstep_Heavy_Attack_01_Modifier = weapon.dual_Backstep_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Backstep_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Backstep_Heavy_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Jumping_Light_Attack_01_Modifier = weapon.dual_Jumping_Light_Attack_01_Modifier;
                meleeDamageCollider.dual_Jumping_Light_Attack_01_PhysicalDamageType = weapon.dual_Jumping_Light_Attack_Off_01_PhysicalDamageType;
                meleeDamageCollider.dual_Jumping_Heavy_Attack_01_Modifier = weapon.dual_Jumping_Heavy_Attack_01_Modifier;
                meleeDamageCollider.dual_Jumping_Heavy_Attack_01_PhysicalDamageType = weapon.dual_Jumping_Heavy_Attack_Off_01_PhysicalDamageType;
            }
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

        public void ProcessTimedEffects()
        {
            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;

                timedEffects[i].ProcessEffect(this);
            }
        }

        public void AddTimedEffect(TimedWeaponEffect effect)
        {
            bool effectIsAlreadyOnCharacter = false;

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    continue;
                if (timedEffects[i].effectID == effect.effectID)
                {
                    effectIsAlreadyOnCharacter = true;
                    timedEffects[i].timeRemainingOnEffect = timedEffects[i].defaultLengthOfEffect;
                }
            }

            if (!effectIsAlreadyOnCharacter)
            {
                timedEffects.Add(effect);
                effect.timeRemainingOnEffect = effect.defaultLengthOfEffect;

                effect.ProcessEffect(this);

                if (effect.effectIcon != null)
                    PlayerUIManager.Singleton.playerUIHudManager.AddEffectIcon(effect.effectIcon);
            }
        }

        public void RemoveTimedEffect(int effectID)
        {

            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i] == null)
                    return;

                if (timedEffects[i].effectID == effectID)
                {
                    TimedWeaponEffect effect = timedEffects[i];
                    effect.RemoveEffect(this);
                    timedEffects.RemoveAt(i);

                    if (effect.effectIcon != null)
                        PlayerUIManager.Singleton.playerUIHudManager.RemoveEffectIcon(effect.effectIcon);
                }
            }
        }

        public TimedWeaponEffect CheckForTimedEffect(int effectID)
        {
            TimedWeaponEffect timedEffect = null;



            for (int i = 0; i < timedEffects.Count; i++)
            {
                if (timedEffects[i].effectID == effectID)
                {
                    timedEffect = timedEffects[i];
                    break;
                }
            }
            return timedEffect;
        }

    }
}