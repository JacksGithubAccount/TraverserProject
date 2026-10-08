using TraverserProject;
using UnityEngine;

public class ConditionalCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int effectID;

    [Header("Icon")]
    public Sprite effectIcon;


    public virtual void ProcessEffect(CharacterManager character)
    {
            
    }

    public virtual void RemoveEffect(CharacterManager character)
    {

    }

    public virtual bool CheckIfMeetsConditionals(CharacterManager character)
    {
        return false;
    }
}
