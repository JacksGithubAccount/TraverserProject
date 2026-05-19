using UnityEngine;

namespace TraverserProject
{

    public class SpellItem : Item
    {
        [Header("Spell Class")]
        public SpellClass spellClass;

        [Header("Description")]
        [TextArea] public string itemEffect;

        [Header("Spell Modifiers")]
        public float fullChargeEffectMultiplier = 2;

        [Header("Spell Costs")]
        public int spellSlotsUsed = 1;
        public int staminaCost = 22;
        public int focusPointCost = 11;

        [Header("Spell Requirement")]
        public int strengthREQ = 0;
        public int dexterityREQ = 0;
        public int intelligenceREQ = 0;
        public int faithREQ = 0;

        [Header("Spell FX")]
        [SerializeField] protected GameObject spellCastWarmUpFX;
        [SerializeField] protected GameObject spellCastChargeFX;
        [SerializeField] protected GameObject spellCastReleaseFX;
        [SerializeField] protected GameObject spellCastReleaseFullChargeFX;

        [Header("Animations")]
        [SerializeField] protected string mainHandSpellAnimation;
        [SerializeField] protected string offHandSpellAnimation;

        [Header("Sound FX")]
        public AudioClip warmUpSoundFX;
        public AudioClip releaseSoundFX;

        public virtual void AttemptToCastSpell(PlayerManager player)
        {

        }

        public virtual void SuccessfullyCastSpell(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= focusPointCost;
                player.playerNetworkManager.currentStamina.Value -= staminaCost;
            }
        }

        public virtual void SuccessfullyChargeSpell(PlayerManager player)
        {

        }

        public virtual void SuccessfullyCastSpellFullCharge(PlayerManager player)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.currentFocusPoints.Value -= Mathf.RoundToInt(focusPointCost * fullChargeEffectMultiplier);
                player.playerNetworkManager.currentStamina.Value -= staminaCost;
            }
        }

        public virtual void InstantiateWarmUpSpellFX(PlayerManager player)
        {

        }

        public virtual void InstantiateReleaseFX(PlayerManager player)
        {

        }

        public virtual bool CanICastThisSpell(PlayerManager player)
        {
            if(player.playerNetworkManager.currentFocusPoints.Value <= focusPointCost)
                return false;

            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return false;

            if (player.isPerformingAction)
                return false;

            if (player.playerNetworkManager.isJumping.Value)
                return false;

            return true;
        }

    }
}