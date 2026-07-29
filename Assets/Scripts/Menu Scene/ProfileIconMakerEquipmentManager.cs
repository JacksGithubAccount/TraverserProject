using System.Collections.Generic;
using UnityEngine;

namespace TraverserProject
{

    public class ProfileIconMakerEquipmentManager : PlayerEquipmentManager
    {
        ProfileIconMakerManager profileIconMaker;
        [Header("Equipment")]
        public HeadEquipmentItem headEquipment;
        public BodyEquipmentItem bodyEquipment;
        public HandEquipmentItem handEquipment;
        public LegEquipmentItem legEquipment;

        protected override void Awake()
        {
            InitializeArmorModels();
            profileIconMaker = GetComponent<ProfileIconMakerManager>();
        }
        protected override void Start()
        {

        }

        private void InitializeArmorModels()
        {
            //unisex equipment
            List<GameObject> hatsList = new List<GameObject>();
            foreach (Transform child in hatsObject.transform)
            {
                hatsList.Add(child.gameObject);
            }
            hats = hatsList.ToArray();

            List<GameObject> hoodsList = new List<GameObject>();
            foreach (Transform child in hoodsObject.transform)
            {
                hoodsList.Add(child.gameObject);
            }
            hoods = hoodsList.ToArray();

            List<GameObject> faceCoversList = new List<GameObject>();
            foreach (Transform child in faceCoversObject.transform)
            {
                faceCoversList.Add(child.gameObject);
            }
            faceCovers = faceCoversList.ToArray();

            List<GameObject> helmetAccessoriesList = new List<GameObject>();
            foreach (Transform child in helmetAccessoriesObject.transform)
            {
                helmetAccessoriesList.Add(child.gameObject);
            }
            helmetAccessories = helmetAccessoriesList.ToArray();

            List<GameObject> backAccessoriesList = new List<GameObject>();
            foreach (Transform child in backAccessoriesObject.transform)
            {
                backAccessoriesList.Add(child.gameObject);
            }
            backAccessories = backAccessoriesList.ToArray();

            List<GameObject> hipAccessoriesList = new List<GameObject>();
            foreach (Transform child in hipAccessoriesObject.transform)
            {
                hipAccessoriesList.Add(child.gameObject);
            }
            hipAccessories = hipAccessoriesList.ToArray();

            List<GameObject> rightShoulderList = new List<GameObject>();
            foreach (Transform child in rightShoulderObject.transform)
            {
                rightShoulderList.Add(child.gameObject);
            }
            rightShoulder = rightShoulderList.ToArray();

            List<GameObject> rightElbowList = new List<GameObject>();
            foreach (Transform child in rightElbowObject.transform)
            {
                rightElbowList.Add(child.gameObject);
            }
            rightElbow = rightElbowList.ToArray();

            List<GameObject> rightKneeList = new List<GameObject>();
            foreach (Transform child in rightKneeObject.transform)
            {
                rightKneeList.Add(child.gameObject);
            }
            rightKnee = rightKneeList.ToArray();

            List<GameObject> leftShoulderList = new List<GameObject>();
            foreach (Transform child in leftShoulderObject.transform)
            {
                leftShoulderList.Add(child.gameObject);
            }
            leftShoulder = leftShoulderList.ToArray();

            List<GameObject> leftElbowList = new List<GameObject>();
            foreach (Transform child in leftElbowObject.transform)
            {
                leftElbowList.Add(child.gameObject);
            }
            leftElbow = leftElbowList.ToArray();

            List<GameObject> leftKneeList = new List<GameObject>();
            foreach (Transform child in leftKneeObject.transform)
            {
                leftKneeList.Add(child.gameObject);
            }
            leftKnee = leftKneeList.ToArray();

            //male equipment
            List<GameObject> maleFullHelmetsList = new List<GameObject>();
            foreach (Transform child in maleFullHelmetObject.transform)
            {
                maleFullHelmetsList.Add(child.gameObject);
            }
            maleHeadFullHelmets = maleFullHelmetsList.ToArray();

            List<GameObject> maleBodiesList = new List<GameObject>();
            foreach (Transform child in maleFullBodyObject.transform)
            {
                maleBodiesList.Add(child.gameObject);
            }
            maleBodies = maleBodiesList.ToArray();

            List<GameObject> maleRightUpperArmsList = new List<GameObject>();
            foreach (Transform child in maleRightUpperArmObject.transform)
            {
                maleRightUpperArmsList.Add(child.gameObject);
            }
            maleRightUpperArms = maleRightUpperArmsList.ToArray();

            List<GameObject> maleRightLowerArmsList = new List<GameObject>();
            foreach (Transform child in maleRightLowerArmObject.transform)
            {
                maleRightLowerArmsList.Add(child.gameObject);
            }
            maleRightLowerArms = maleRightLowerArmsList.ToArray();

            List<GameObject> maleRightHandsList = new List<GameObject>();
            foreach (Transform child in maleRightHandObject.transform)
            {
                maleRightHandsList.Add(child.gameObject);
            }
            maleRightHands = maleRightHandsList.ToArray();

            List<GameObject> maleLeftUpperArmsList = new List<GameObject>();
            foreach (Transform child in maleLeftUpperArmObject.transform)
            {
                maleLeftUpperArmsList.Add(child.gameObject);
            }
            maleLeftUpperArms = maleLeftUpperArmsList.ToArray();

            List<GameObject> maleLeftLowerArmsList = new List<GameObject>();
            foreach (Transform child in maleLeftLowerArmObject.transform)
            {
                maleLeftLowerArmsList.Add(child.gameObject);
            }
            maleLeftLowerArms = maleLeftLowerArmsList.ToArray();

            List<GameObject> maleLeftHandsList = new List<GameObject>();
            foreach (Transform child in maleLeftHandObject.transform)
            {
                maleLeftHandsList.Add(child.gameObject);
            }
            maleLeftHands = maleLeftHandsList.ToArray();

            List<GameObject> maleHipsList = new List<GameObject>();
            foreach (Transform child in maleHipsObject.transform)
            {
                maleHipsList.Add(child.gameObject);
            }
            maleHips = maleHipsList.ToArray();

            List<GameObject> maleRightLegsList = new List<GameObject>();
            foreach (Transform child in maleRightLegObject.transform)
            {
                maleRightLegsList.Add(child.gameObject);
            }
            maleRightLegs = maleRightLegsList.ToArray();

            List<GameObject> maleLeftLegsList = new List<GameObject>();
            foreach (Transform child in maleLeftLegObject.transform)
            {
                maleLeftLegsList.Add(child.gameObject);
            }
            maleLeftLegs = maleLeftLegsList.ToArray();

            //female equipment
            List<GameObject> femaleFullHelmetsList = new List<GameObject>();
            foreach (Transform child in femaleFullHelmetObject.transform)
            {
                femaleFullHelmetsList.Add(child.gameObject);
            }
            femaleHeadFullHelmets = femaleFullHelmetsList.ToArray();

            List<GameObject> femaleBodiesList = new List<GameObject>();
            foreach (Transform child in femaleFullBodyObject.transform)
            {
                femaleBodiesList.Add(child.gameObject);
            }
            femaleBodies = femaleBodiesList.ToArray();

            List<GameObject> femaleRightUpperArmsList = new List<GameObject>();
            foreach (Transform child in femaleRightUpperArmObject.transform)
            {
                femaleRightUpperArmsList.Add(child.gameObject);
            }
            femaleRightUpperArms = femaleRightUpperArmsList.ToArray();

            List<GameObject> femaleRightLowerArmsList = new List<GameObject>();
            foreach (Transform child in femaleRightLowerArmObject.transform)
            {
                femaleRightLowerArmsList.Add(child.gameObject);
            }
            femaleRightLowerArms = femaleRightLowerArmsList.ToArray();

            List<GameObject> femaleRightHandsList = new List<GameObject>();
            foreach (Transform child in femaleRightHandObject.transform)
            {
                femaleRightHandsList.Add(child.gameObject);
            }
            femaleRightHands = femaleRightHandsList.ToArray();

            List<GameObject> femaleLeftUpperArmsList = new List<GameObject>();
            foreach (Transform child in femaleLeftUpperArmObject.transform)
            {
                femaleLeftUpperArmsList.Add(child.gameObject);
            }
            femaleLeftUpperArms = femaleLeftUpperArmsList.ToArray();

            List<GameObject> femaleLeftLowerArmsList = new List<GameObject>();
            foreach (Transform child in femaleLeftLowerArmObject.transform)
            {
                femaleLeftLowerArmsList.Add(child.gameObject);
            }
            femaleLeftLowerArms = femaleLeftLowerArmsList.ToArray();

            List<GameObject> femaleLeftHandsList = new List<GameObject>();
            foreach (Transform child in femaleLeftHandObject.transform)
            {
                femaleLeftHandsList.Add(child.gameObject);
            }
            femaleLeftHands = femaleLeftHandsList.ToArray();

            List<GameObject> femaleHipsList = new List<GameObject>();
            foreach (Transform child in femaleHipsObject.transform)
            {
                femaleHipsList.Add(child.gameObject);
            }
            femaleHips = femaleHipsList.ToArray();

            List<GameObject> femaleRightLegsList = new List<GameObject>();
            foreach (Transform child in femaleRightLegObject.transform)
            {
                femaleRightLegsList.Add(child.gameObject);
            }
            femaleRightLegs = femaleRightLegsList.ToArray();

            List<GameObject> femaleLeftLegsList = new List<GameObject>();
            foreach (Transform child in femaleLeftLegObject.transform)
            {
                femaleLeftLegsList.Add(child.gameObject);
            }
            femaleLeftLegs = femaleLeftLegsList.ToArray();
        }

        protected override void UnloadHeadEquipmentModels()
        {
            foreach (var model in maleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in hats)
            {
                model.SetActive(false);
            }

            foreach (var model in hoods)
            {
                model.SetActive(false);
            }

            foreach (var model in faceCovers)
            {
                model.SetActive(false);
            }

            foreach (var model in helmetAccessories)
            {
                model.SetActive(false);
            }


            profileIconMaker.profileIconMakerBodyManager.EnableHead();
            profileIconMaker.profileIconMakerBodyManager.EnableHair();
        }

        public override void LoadHeadEquipment(HeadEquipmentItem equipment)
        {
            UnloadHeadEquipmentModels();
            if (equipment == null)
            {
                profileIconMaker.profileIconMakerBodyManager.EnableHead();
                profileIconMaker.profileIconMakerBodyManager.EnableHair();
                return;
            }

            switch (equipment.headEquipmentType)
            {

                case HeadEquipmentType.FullHelmet:
                    profileIconMaker.profileIconMakerBodyManager.DisableHair();
                    profileIconMaker.profileIconMakerBodyManager.DisableHead();
                    break;
                case HeadEquipmentType.Hat:
                    break;
                case HeadEquipmentType.Hood:
                    profileIconMaker.profileIconMakerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.FaceCover:
                    profileIconMaker.profileIconMakerBodyManager.DisableFacialHair();
                    break;
                default:
                    break;
            }


            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(this, profileIconMaker.profileIconMakerBodyManager.isMale);
            }

        }

        public override void LoadBodyEquipment(BodyEquipmentItem equipment)
        {
            UnloadBodyEquipmentModels();

            if (equipment == null)
            {
                profileIconMaker.profileIconMakerBodyManager.EnableBody();
                return;
            }
            else
            {
                foreach (var model in equipment.equipmentModels)
                {
                    model.LoadModel(this, profileIconMaker.profileIconMakerBodyManager.isMale);
                }
            }
        }

        protected override void UnloadBodyEquipmentModels()
        {
            //unisex
            foreach (var model in rightShoulder)
            {
                model.SetActive(false);
            }
            foreach (var model in rightElbow)
            {
                model.SetActive(false);
            }
            foreach (var model in leftShoulder)
            {
                model.SetActive(false);
            }
            foreach (var model in leftElbow)
            {
                model.SetActive(false);
            }
            foreach (var model in hipAccessories)
            {
                model.SetActive(false);
            }
            foreach (var model in backAccessories)
            {
                model.SetActive(false);
            }

            //male
            foreach (var model in maleBodies)
            {
                model.SetActive(false);
            }
            foreach (var model in maleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in maleLeftUpperArms)
            {
                model.SetActive(false);
            }

            //female
            foreach (var model in femaleBodies)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLeftUpperArms)
            {
                model.SetActive(false);
            }

            profileIconMaker.profileIconMakerBodyManager.EnableBody();
        }

    }
}