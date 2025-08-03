using UnityEngine;

namespace TraverserProject
{

    public class AshOfWar : Item
    {
        [Header("Ash of War information")]
        public WeaponClass[] usableWeaponClasses;
        [Header("Costs")]
        public int focusPointCost = 20;
        public int staminaCost = 20;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)
        {
            Debug.Log("Performed As of War");
        }

        public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            return false;
        }

        protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)
        {
            playerPerformingAction.playerNetworkManager.currentStamina.Value -= staminaCost;
        }

        protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)
        {

        }

    }
}