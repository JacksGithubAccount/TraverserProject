using UnityEngine;

namespace TraverserProject
{

    public class ProfileIconMakerManager : MonoBehaviour
    {
        [HideInInspector] public ProfileIconMakerBodyManager profileIconMakerBodyManager;
        [HideInInspector] public ProfileIconMakerEquipmentManager profileIconMakerEquipmentManager;

        private void Awake()
        {
            profileIconMakerBodyManager = GetComponent<ProfileIconMakerBodyManager>();
            profileIconMakerEquipmentManager = GetComponent<ProfileIconMakerEquipmentManager>();
        }
    }
}