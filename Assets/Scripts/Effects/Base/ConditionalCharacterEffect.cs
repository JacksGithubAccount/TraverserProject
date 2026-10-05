using TraverserProject;
using UnityEngine;

public class ConditionalCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int effectID;

    [Header("Icon")]
    public Sprite effectIcon;

    [Header("Conditional")]
    public float AmountOfHealthLeftToTriggerEffect = 0;
    public float AboveAmountOfHealthToTriggerEffect = 0;

    public virtual void ProcessEffect(CharacterManager character)
    {
            character.characterEffectsManager.RemoveTimedEffect(effectID);
    }

    public virtual void RemoveEffect(CharacterManager character)
    {

    }

    public virtual void CheckConditionals()
    {

    }
}
