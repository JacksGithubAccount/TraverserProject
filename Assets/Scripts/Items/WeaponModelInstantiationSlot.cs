using UnityEngine;

namespace TraverserProject
{

    public class WeaponModelInstantiationSlot : MonoBehaviour
    {
        public WeaponModelSlot weaponSlot;
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if (currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void PlaceWeaponModelIntoSlot(GameObject weaponModel)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
        }

        public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
        {
            currentWeaponModel = weaponModel;
            weaponModel.transform.parent = transform;

            switch (weaponClass)
            {

                case WeaponClass.StraightSword:
                    weaponModel.transform.localPosition = new Vector3(-0.131f, -0.018f, 0.193f);
                    weaponModel.transform.localRotation = Quaternion.Euler(-163.77f, 288.326f, 5.459f);
                    break;
                case WeaponClass.Spear:
                    weaponModel.transform.localPosition = new Vector3(0.064f, 0f, -0.06f);
                    weaponModel.transform.localRotation = Quaternion.Euler(194, 90, -0.22f);
                    break;
                case WeaponClass.MediumShield:
                    weaponModel.transform.localPosition = new Vector3(-0.029f, -0.034f, 0.017f);
                    weaponModel.transform.localRotation = Quaternion.Euler(-187.466f, 8.203995f, -168.213f);
                    break;
                default:
                    break;

            }
        }
    }
}