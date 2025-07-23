using UnityEngine;

namespace TraverserProject
{
    [CreateAssetMenu(menuName = "Equipment Model")]
    public class EquipmentModel : ScriptableObject
    {
        public EquipmentModelType equipmentModelType;
        public string maleEquipmentName;
        public string femaleEquipmentName;


        public void LoadModel(PlayerManager player, bool isMale) //, Material equipmentMaterial
        {
            if (isMale)
            {
                LoadMaleModel(player);
            }
            else
            {
                LoadFemaleModel(player);
            }
        }

        private void LoadMaleModel(PlayerManager player)
        {
            switch (equipmentModelType)
            {
                case EquipmentModelType.FullHelmet:
                    foreach (var model in player.playerEquipmentManager.maleHeadFullHelmets)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hat:
                    foreach (var model in player.playerEquipmentManager.hats)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Hood:
                    foreach (var model in player.playerEquipmentManager.hoods)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.HelmetAccessories:
                    foreach (var model in player.playerEquipmentManager.helmetAccessories)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.FaceCover:
                    foreach (var model in player.playerEquipmentManager.faceCovers)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Torso:
                    foreach (var model in player.playerEquipmentManager.maleBodies)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.Back:
                    foreach (var model in player.playerEquipmentManager.backAccessories)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightShoulder:
                    foreach (var model in player.playerEquipmentManager.rightShoulder)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightUpperArm:
                    foreach (var model in player.playerEquipmentManager.maleRightUpperArms)
                    {
                        if (model.gameObject.name == maleEquipmentName)
                        {
                            model.gameObject.SetActive(true);
                            //model.gameObject.GetComponent<Renderer>().material = Instantiate(equipmentMaterial);
                        }
                    }
                    break;
                case EquipmentModelType.RightElbow:

                    break;
                case EquipmentModelType.RightLowerArm:

                    break;
                case EquipmentModelType.RightHand:

                    break;
                case EquipmentModelType.LeftShoulder:

                    break;
                case EquipmentModelType.LeftUpperArm:

                    break;
                case EquipmentModelType.LeftElbow:

                    break;
                case EquipmentModelType.LeftLowerArm:

                    break;
                case EquipmentModelType.LeftHand:

                    break;
                case EquipmentModelType.Hips:

                    break;
                case EquipmentModelType.HipsAttachment:

                    break;
                case EquipmentModelType.RightLeg:

                    break;
                case EquipmentModelType.RightKnee:

                    break;
                case EquipmentModelType.LeftLeg:

                    break;
                case EquipmentModelType.LeftKnee:

                    break;
                default:
                    break;
            }
        }

        private void LoadFemaleModel(PlayerManager player)
        {

        }
    }
}