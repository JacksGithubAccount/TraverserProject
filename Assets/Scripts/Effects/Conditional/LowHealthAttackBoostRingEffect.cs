using TraverserProject;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Conditional Effects/Low Health Attack Boost Ring")]
public class LowHealthAttackBoostRingEffect : ConditionalCharacterEffect
{
    [Header("Effect Calculated")]
    private bool effectHasBeenInitialized = false;

    [Header("Damage Modified")]
    public int physicalDamageGainedFromEffect;
    public int magicDamageGainedFromEffect;
    public int fireDamageGainedFromEffect;
    public int lightningDamageGainedFromEffect;
    public int holyDamageGainedFromEffect;


    [Header("Conditional")]
    public float AmountOfHealthLeftToTriggerEffect = 0;

    [Header("VFX")]
    public GameObject VFX;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (effectHasBeenInitialized)
            return;

        if (!CheckIfMeetsConditionals(character))
        {
            RemoveEffect(character);
            return;
        }

        effectHasBeenInitialized = true;

        if (!character.IsOwner)
            return;

        character.characterNetworkManager.physicalDamageModifier.Value += physicalDamageGainedFromEffect;
        character.characterNetworkManager.magicDamageModifier.Value += magicDamageGainedFromEffect;
        character.characterNetworkManager.fireDamageModifier.Value += fireDamageGainedFromEffect;
        character.characterNetworkManager.lightningDamageModifier.Value += lightningDamageGainedFromEffect;
        character.characterNetworkManager.holyDamageModifier.Value += holyDamageGainedFromEffect;

        VFX.SetActive(true);
        VFX = Instantiate(VFX);
        VFX.transform.position = character.characterEffectsManager.effectTransform.position;
        VFX.transform.root.rotation = Quaternion.identity;
    }

    public override void RemoveEffect(CharacterManager character)
    {
        base.RemoveEffect(character);

        if (!effectHasBeenInitialized)
            return;

        if (!character.IsOwner)
            return;

        character.characterNetworkManager.physicalDamageModifier.Value -= physicalDamageGainedFromEffect;
        character.characterNetworkManager.magicDamageModifier.Value -= magicDamageGainedFromEffect;
        character.characterNetworkManager.fireDamageModifier.Value -= fireDamageGainedFromEffect;
        character.characterNetworkManager.lightningDamageModifier.Value -= lightningDamageGainedFromEffect;
        character.characterNetworkManager.holyDamageModifier.Value -= holyDamageGainedFromEffect;

        VFX.SetActive(false);
    }
    public override bool CheckIfMeetsConditionals(CharacterManager character)
    {

        return AmountOfHealthLeftToTriggerEffect >= (character.characterNetworkManager.currentHealth.Value / character.characterNetworkManager.maxHealth.Value) * 100;
    }
}
