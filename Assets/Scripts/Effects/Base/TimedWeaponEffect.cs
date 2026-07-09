using UnityEngine;


namespace TraverserProject
{

    public class TimedWeaponEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int effectID;

        [Header("Icon")]
        public Sprite effectIcon;

        [Header("Time")]
        public float defaultLengthOfEffect;
        public float timeRemainingOnEffect;

        public virtual void ProcessEffect(WeaponManager weapon)
        {
            timeRemainingOnEffect -= 1;

            if (timeRemainingOnEffect <= 0)
                weapon.RemoveTimedEffect(effectID);
        }

        public virtual void RemoveEffect(WeaponManager weapon)
        {

        }

    }
}