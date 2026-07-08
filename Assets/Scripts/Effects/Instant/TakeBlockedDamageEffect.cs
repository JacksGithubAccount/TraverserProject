using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
    public class TakeBlockedDamageEffect : DamageEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Stamina")]
        public float staminaDamage = 0;
        public float finalStaminaDamage = 0;

        [Header("Animations")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
                return;

            Debug.Log("Hit was blocked");
            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            CalculateStaminaDamage(character);
            PlayDirectionalBasedBlockingAnimation(character);

            PlayDamageSFX(character);
            PlayDamageVFX(character);

            CheckForGuardBreak(character);

            CheckForDeathAnimation(character);
        }

        private void CalculateDamage(CharacterManager character)
        {

            if (characterCausingDamage != null)
            {

            }
            switch (physicalDamageType)
            {
                case PhysicalDamageType.Regular:
                    float physicalAbsorption = character.characterNetworkManager.armorPhysicalDamageAbsorption.Value / 100;
                    physicalDamage -= (physicalDamage * physicalAbsorption);
                    break;
                case PhysicalDamageType.Blunt:
                    float bluntAbsorption = character.characterNetworkManager.armorBluntDamageAbsorption.Value / 100;
                    physicalDamage -= (physicalDamage * bluntAbsorption);
                    break;
                case PhysicalDamageType.Pierce:
                    float pierceAbsorption = character.characterNetworkManager.armorPierceDamageAbsorption.Value / 100;
                    physicalDamage -= (physicalDamage * pierceAbsorption);
                    break;
                case PhysicalDamageType.Slash:
                    float slashAbsorption = character.characterNetworkManager.armorSlashDamageAbsorption.Value / 100;
                    physicalDamage -= (physicalDamage * slashAbsorption);
                    break;
                default:
                    float physicalAbsorption2 = character.characterNetworkManager.armorPhysicalDamageAbsorption.Value / 100;
                    physicalDamage -= (physicalDamage * physicalAbsorption2);
                    break;
            }
            if (physicalDamage < 0)
                physicalDamage = 0;

            float fireAbsorption = character.characterNetworkManager.armorFireDamageAbsorption.Value / 100;
            fireDamage -= (fireDamage * fireAbsorption);
            if (fireDamage < 0)
                fireDamage = 0;

            float magicAbsorption = character.characterNetworkManager.armorMagicDamageAbsorption.Value / 100;
            magicDamage -= (magicDamage * magicAbsorption);
            if (magicDamage < 0)
                magicDamage = 0;

            float lightningAbsorption = character.characterNetworkManager.armorLightningDamageAbsorption.Value / 100;
            lightningDamage -= (lightningDamage * lightningAbsorption);
            if (lightningDamage < 0)
                lightningDamage = 0;

            float holyAbsorption = character.characterNetworkManager.armorHolyDamageAbsorption.Value / 100;
            holyDamage -= (holyDamage * holyAbsorption);
            if (holyDamage < 0)
                holyDamage = 0;

            physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
            fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
            magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
            lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
            holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100));

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 0;
            }

            Debug.Log("Blocked Phys Damage: " + physicalDamage);

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;


            //Apply damage and poise damage if server
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

                if (character.characterNetworkManager.currentHealth.Value <= 0)
                {
                    character.isDeadLocal = true;
                    character.animator.SetBool("isDead", true);
                }

            }
            //predict damage and poise damage
            else
            {
                character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
                //update the HP bar to reflect projected health

                if (character.characterNetworkManager.currentHealth.Value <= 0)
                {
                    character.isDeadLocal = true;
                    character.animator.SetBool("isDead", true);
                }

            }


        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            finalStaminaDamage = staminaDamage;
            float staminaDamageAbsorption = finalStaminaDamage * (character.characterStatsManager.blockingStability / 100);
            float staminaDamageAfterAbsorption = finalStaminaDamage - staminaDamageAbsorption;

            character.characterNetworkManager.currentStamina.Value -= staminaDamageAfterAbsorption;
        }

        private void CheckForGuardBreak(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.currentStamina.Value <= 0)
            {
                character.characterAnimatorManager.PlayTargetActionAnimation("Guard_Break_01", true);
                character.characterNetworkManager.isBlocking.Value = false;


            }
        }

        private void PlayDamageVFX(CharacterManager character)
        {

        }

        private void PlayDamageSFX(CharacterManager character)
        {
            character.characterSoundFXManager.PlayBlockSoundFX();
        }

        private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
        {
            if (character.isDead.Value || character.isDeadLocal)
                return;

            DamageIntensity damageIntensity = WorldUtilityManager.Singleton.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

            switch (damageIntensity)
            {
                case DamageIntensity.Ping:
                    damageAnimation = "Block_Ping_01";
                    break;
                case DamageIntensity.Light:
                    damageAnimation = "Block_Light_01";
                    break;
                case DamageIntensity.Medium:
                    damageAnimation = "Block_Medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageAnimation = "Block_Heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageAnimation = "Block_Colossal_01";
                    break;
                default:
                    break;
            }


            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            character.characterAnimatorManager.PlayTargetLocalAnimationInstantly(damageAnimation, true);

        }

        private void CheckForDeathAnimation(CharacterManager character)
        {
            if (!character.isDead.Value && !character.isDeadLocal)
                return;

            character.characterCombatManager.CheckForDeathAnimation();
        }

    }
}