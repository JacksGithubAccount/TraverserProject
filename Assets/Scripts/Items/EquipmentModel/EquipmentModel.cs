using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        public EquipmentModelType equipmentModelType;
        public string maleEquipmentName;
        public string femaleEquipmentName;


        public void LoadModel(PlayerEquipmentManager equipmentManager, bool isMale) //, Material equipmentMaterial
        {
            if (isMale)
            {
                LoadMaleModel(equipmentManager);
            }
            else
            {
                LoadFemaleModel(equipmentManager);
            }
        }

        private void LoadMaleModel(PlayerEquipmentManager equipmentManager)
        {
            switch (equipmentModelType)
            {
                case EquipmentModelType.FullHelmet:
                    foreach (var model in equipmentManager.maleHeadFullHelmets)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hat:
                    foreach (var model in equipmentManager.hats)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hood:
                    foreach (var model in equipmentManager.hoods)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.HelmetAccessories:
                    foreach (var model in equipmentManager.helmetAccessories)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.FaceCover:
                    foreach (var model in equipmentManager.faceCovers)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Torso:
                    foreach (var model in equipmentManager.maleBodies)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Back:
                    foreach (var model in equipmentManager.backAccessories)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightShoulder:
                    foreach (var model in equipmentManager.rightShoulder)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightUpperArm:
                    foreach (var model in equipmentManager.maleRightUpperArms)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightElbow:
                    foreach (var model in equipmentManager.rightElbow)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightLowerArm:
                    foreach (var model in equipmentManager.maleRightLowerArms)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightHand:
                    foreach (var model in equipmentManager.maleRightHands)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftShoulder:
                    foreach (var model in equipmentManager.leftShoulder)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftUpperArm:
                    foreach (var model in equipmentManager.maleLeftUpperArms)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftElbow:
                    foreach (var model in equipmentManager.leftElbow)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftLowerArm:
                    foreach (var model in equipmentManager.maleLeftLowerArms)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftHand:
                    foreach (var model in equipmentManager.maleLeftHands)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hips:
                    foreach (var model in equipmentManager.maleHips)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.HipsAttachment:
                    foreach (var model in equipmentManager.hipAccessories)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightLeg:
                    foreach (var model in equipmentManager.maleRightLegs)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightKnee:
                    foreach (var model in equipmentManager.rightKnee)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftLeg:
                    foreach (var model in equipmentManager.maleLeftLegs)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftKnee:
                    foreach (var model in equipmentManager.leftKnee)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadFemaleModel(PlayerEquipmentManager equipmentManager)
        {
            switch (equipmentModelType)
            {
                case EquipmentModelType.FullHelmet:
                    foreach (var model in equipmentManager.femaleHeadFullHelmets)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hat:
                    foreach (var model in equipmentManager.hats)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hood:
                    foreach (var model in equipmentManager.hoods)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.HelmetAccessories:
                    foreach (var model in equipmentManager.helmetAccessories)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.FaceCover:
                    foreach (var model in equipmentManager.faceCovers)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Torso:
                    foreach (var model in equipmentManager.femaleBodies)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Back:
                    foreach (var model in equipmentManager.backAccessories)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightShoulder:
                    foreach (var model in equipmentManager.rightShoulder)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightUpperArm:
                    foreach (var model in equipmentManager.femaleRightUpperArms)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightElbow:
                    foreach (var model in equipmentManager.rightElbow)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightLowerArm:
                    foreach (var model in equipmentManager.femaleRightLowerArms)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightHand:
                    foreach (var model in equipmentManager.femaleRightHands)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftShoulder:
                    foreach (var model in equipmentManager.leftShoulder)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftUpperArm:
                    foreach (var model in equipmentManager.femaleLeftUpperArms)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftElbow:
                    foreach (var model in equipmentManager.leftElbow)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftLowerArm:
                    foreach (var model in equipmentManager.femaleLeftLowerArms)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftHand:
                    foreach (var model in equipmentManager.femaleLeftHands)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hips:
                    foreach (var model in equipmentManager.femaleHips)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.HipsAttachment:
                    foreach (var model in equipmentManager.hipAccessories)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightLeg:
                    foreach (var model in equipmentManager.femaleRightLegs)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightKnee:
                    foreach (var model in equipmentManager.rightKnee)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftLeg:
                    foreach (var model in equipmentManager.femaleLeftLegs)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.LeftKnee:
                    foreach (var model in equipmentManager.leftKnee)
                    {
                        if (model.gameObject.name == femaleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}