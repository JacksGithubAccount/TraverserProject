using UnityEngine;
using System.Linq;

namespace TraverserProject
{

    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager Singleton;

        [Header("Weapon Item Actions")]
        public WeaponItemAction[] weaponItemActions;

        public void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
            for (int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemActionByID(int ID)
        {
            return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
        }

    }
}