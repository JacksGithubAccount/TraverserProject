using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager Singleton;

        [Header("VFX")]
        public GameObject bloodSplatterVFX;
        public GameObject criticalBloodSplatterVFX;
        public GameObject healingFlaskVFX;
        public GameObject deadSpotVFX;
        public GameObject healedVFX;
        public GameObject poisonedVFX;
        public GameObject bloodLossVFX;
        public GameObject frostbiteVFX;
        public GameObject poisonCureVFX;
        public GameObject bloodLossCureVFX;
        public GameObject frostbiteCureVFX;
        public GameObject weaponBuffVFX;

        [Header("Swings")]
        public GameObject weaponSwingVFX;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;
        public TakeBlockedDamageEffect takeBlockedDamageEffect;
        public TakeCriticalDamageEffect takeCriticalDamageEffect;

        [Header("Frostbite")]
        public ModifyStaminaRegenerationForATimeEffect frostbiteStaminaRegenerationEffect;
        public ModifyArmorAbsorptionForATimeEffect frostbiteAbsorptionDebuffEffect;

        [Header("Item Status Buff")]
        public ModifyStaminaRegenerationForATimeEffect itemStaminaRegenerationEffect;
        public ModifyArmorAbsorptionForATimeEffect itemAbsorptionBuffEffect;

        [Header("Weapon Buff")]
        public ModifyWeaponDamageForATimeEffect weaponBuffEffect;

        [Header("Recovery")]
        public TakeRecoveryEffect takeRecoveryEffect;

        [Header("Status Effects")]
        public PoisonedEffect poisonedEffect;
        public BloodLossEffect bloodLossEffect;
        public FrostbiteEffect frostbiteEffect;

        [Header("Take Build Ups")]
        public TakeBuildUpEffect takePoisonBuildUpEffect;
        public TakeBuildUpEffect takeBleedBuildUpEffect;
        public TakeBuildUpEffect takeFrostBuildUpEffect;

        [Header("Degrade Build Ups")]
        public BuildUpEffect degradePoisonBuildUpEffect;
        public BuildUpEffect degradeBleedBuildUpEffect;
        public BuildUpEffect degradeFrostBuildUpEffect;

        [Header("Two Hand")]
        public TwoHandingEffect twoHandingEffect;

        [Header("Accessory")]
        public LifeRingEffect lifeRingEffect;

        [Header("Instant Effects")]
        [SerializeField] List<InstantCharacterEffect> instantEffects;

        [Header("Static Effects")]
        [SerializeField] List<StaticCharacterEffect> staticEffects;

        [Header("Timed Effects")]
        [SerializeField] List<TimedCharacterEffect> timedEffects;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }

            for (int i = 0; i < staticEffects.Count; i++)
            {
                staticEffects[i].staticEffectID = i;
            }

            for (int i = 0; i < timedEffects.Count; i++)
            {
                timedEffects[i].effectID = i;
            }
        }

        public StaticCharacterEffect RetrieveAccessoryStaticEffect(AccessoryEquipmentItem item)
        {
            if (item.itemName == "Life Ring")
            {
                return lifeRingEffect;
            }

            return null;
        }

        public TimedCharacterEffect GetTimedEffectFromSerializedData(SerializableTimedEffect serializedTimeEffect)
        {
            TimedCharacterEffect timedCharacterEffect = null;
            if (GetTimedEffectByID(serializedTimeEffect.effectID))
            {
                timedCharacterEffect = Instantiate(GetTimedEffectByID(serializedTimeEffect.effectID));
                timedCharacterEffect.timeRemainingOnEffect = serializedTimeEffect.timeRemainingOnEffect;
            }

            return timedCharacterEffect;
        }

        public StaticCharacterEffect GetStaticEffectByID(int ID)
        {
            return staticEffects.Find(effect => effect.staticEffectID == ID);
        }
        public TimedCharacterEffect GetTimedEffectByID(int ID)
        {
            return timedEffects.Find(effect => effect.effectID == ID);
        }

    }
}