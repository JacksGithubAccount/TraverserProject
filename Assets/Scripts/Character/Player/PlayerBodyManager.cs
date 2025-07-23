using UnityEngine;

namespace TraverserProject
{

    public class PlayerBodyManager : MonoBehaviour
    {
        [Header("Hair")]
        [SerializeField] public GameObject hair;
        [SerializeField] public GameObject facialHair;

        [Header("Male")]
        [SerializeField] public GameObject maleHead;
        [SerializeField] public GameObject[] maleBody;
        [SerializeField] public GameObject[] maleArms;
        [SerializeField] public GameObject[] maleLegs;
        [SerializeField] public GameObject maleEyebrows;


        [Header("Female")]
        [SerializeField] public GameObject femaleHead;
        [SerializeField] public GameObject[] femaleBody;
        [SerializeField] public GameObject[] femaleArms;
        [SerializeField] public GameObject[] femaleLegs;
        [SerializeField] public GameObject femaleEyebrows;

        public void EnableHead()
        {
            maleHead.SetActive(true);
            femaleHead.SetActive(true);

            maleEyebrows.SetActive(true);
            femaleEyebrows.SetActive(true);
        }

        public void DisableHead()
        {
            maleHead.SetActive(false);
            femaleHead.SetActive(false);

            maleEyebrows.SetActive(false);
            femaleEyebrows.SetActive(false);
        }

        public void EnableHair()
        {
            hair.SetActive(true);
        }

        public void DisableHair()
        {
            hair.SetActive(false);
        }

        public void EnableFacialHair()
        {
            facialHair.SetActive(true);
        }

        public void DisableFacialHair()
        {
            facialHair.SetActive(false);
        }

        public void EnableBody()
        {
            foreach (var model in maleBody)
            {
                model.SetActive(true);
            }

            foreach (var model in femaleBody)
            {
                model.SetActive(true);
            }
        }

    }
}