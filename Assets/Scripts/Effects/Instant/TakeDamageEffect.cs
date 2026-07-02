using System;
using UnityEngine;

namespace TraverserProject
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]

    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        public PhysicalDamageType physicalDamageType = PhysicalDamageType.Regular;


        [Header("Final Damage")]
        protected int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

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

            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            CalculateDamage(character);
            PlayDirectionalBasedDamageAnimation(character);

            PlayDamageSFX(character);
            PlayDamageVFX(character);

            //run this after all other functions that would attempt to play an animation upon being damaged & after poise/stance is calculated
            CalculateStanceDamage(character);



        }

        protected virtual void CalculateDamage(CharacterManager character)
        {

            if (characterCausingDamage != null)
            {
                //Check for damage modifiers and modify base damage(damage buffs)
            }

            //Check for flat defenses here

            //check for percent defenses here			
            Debug.Log("Original Phys Damage: " + physicalDamage + " Type: " + physicalDamageType.ToString());

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


            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);
            Debug.Log("After Calculations Phys Damage: " + physicalDamage + " Type: " + physicalDamageType.ToString());
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            //Apply damage and poise damage if server
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

                character.characterNetworkManager.totalPoiseDamage.Value -= poiseDamage;

                //stores poise daamage taken for other interactions
                character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

                float remainingPoise = GetCurrentPoise(character);

                if (remainingPoise <= 0)
                    poiseIsBroken = true;

                character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime;
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

                int projectedPoise = Mathf.RoundToInt(GetCurrentPoise(character) - poiseDamage);

                if (projectedPoise <= 0)
                {
                    poiseIsBroken = true;
                }
                else
                {
                    poiseIsBroken = false;
                }
            }
        }

        protected void CalculateStanceDamage(CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            int stanceDamage = Mathf.RoundToInt(poiseDamage);

            if (aiCharacter != null)
            {
                aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
            }
        }

        protected void PlayDamageVFX(CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        protected void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.Singleton.ChooseRandomSFXFromArray(WorldSoundFXManager.Singleton.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGruntFX();
        }

        protected void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (character.isDead.Value || character.isDeadLocal)
                return;

            if (character.characterNetworkManager.isClimbingLadder.Value)
            {
                //eldenring's method - if hit twice within x time, fall off
                if (character.characterNetworkManager.canBeKnockedOffLadder.Value)
                {
                    KnockCharacterOffLadder(character);
                    return;
                }
                else
                {
                    character.characterLocomotionManager.EnableCanBeKnockedOffLadderForATime(character.characterLocomotionManager.knockOffLadderWindow);
                }


                //falls if poise is broken
                //if(poiseIsBroken)
                //{
                //	KnockCharacterOffLadder(character);
                //	return;
                //}
            }

            if (poiseIsBroken)
            {
                //front
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                //
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
                }
                //back
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
                }
                //left
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
                }
                //right
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
                }
            }
            else
            {
                //front
                if (angleHitFrom >= 145 && angleHitFrom <= 180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                //
                else if (angleHitFrom <= -145 && angleHitFrom >= -180)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Ping_Damage);
                }
                //back
                else if (angleHitFrom >= -45 && angleHitFrom <= 45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Ping_Damage);
                }
                //left
                else if (angleHitFrom >= -144 && angleHitFrom <= -45)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Ping_Damage);
                }
                //right
                else if (angleHitFrom >= 45 && angleHitFrom <= 144)
                {
                    damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Ping_Damage);
                }

            }

            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;

            if (poiseIsBroken)
            {
                character.characterAnimatorManager.PlayTargetLocalAnimation(damageAnimation, true);
                character.characterCombatManager.DestroyAllCurrentActionFX();
            }
            else
            {
                character.characterAnimatorManager.PlayTargetLocalAnimation(damageAnimation, false, false, true, true);
            }


        }

        protected void KnockCharacterOffLadder(CharacterManager character)
        {
            character.characterAnimatorManager.PlayTargetLocalAnimationInstantly("Ladder_Fall_Start_01", true);
            character.characterLocomotionManager.isExitingLadder = false;
        }

        private float GetCurrentPoise(CharacterManager character)
        {
            float currentPoise = character.characterNetworkManager.basePoiseDefense.Value + character.characterNetworkManager.offensivePoiseBonus.Value + character.characterNetworkManager.totalPoiseDamage.Value;
            return currentPoise;
        }

    }
}