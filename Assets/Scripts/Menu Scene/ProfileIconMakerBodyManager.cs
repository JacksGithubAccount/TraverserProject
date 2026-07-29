using UnityEngine;

namespace TraverserProject
{

    public class ProfileIconMakerBodyManager : PlayerBodyManager
    {
        [Header("Body")]
        public bool isMale = true;

        protected override void Awake()
        {

        }

        public void ChangeSex(bool isMale)
        {
            this.isMale = isMale;

            if (isMale)
            {
                femaleObject.SetActive(false);
                maleObject.SetActive(true);
            }
            else
            {
                maleObject.SetActive(false);
                femaleObject.SetActive(true);
            }
        }

        //int browStyle, int beardStyle, if needed
        public void ChangeHairOnDummy(int hairStyle, float hairColorRed, float hairColorGreen, float hairColorBlue)
        {
            foreach (var item in hairObjects)
            {
                item.SetActive(false);
            }

            hairObjects[hairStyle].SetActive(true);

            //male only objects
            if (isMale)
            {
                /*
                foreach(var item in maleEyebrows)
                {
                    item.set
                }
                */
            }
            //female only objects
            else
            {

            }

            //Set hair color(might want to do the same brows/beard/etc)
            Color32 hairColor;

            byte red = (byte)hairColorRed;
            byte green = (byte)hairColorGreen;
            byte blue = (byte)hairColorBlue;

            hairColor = new Color32(red, green, blue, 255);

            for (int i = 0; i < hairObjects.Length; i++)
            {
                SkinnedMeshRenderer skinMeshRenderer = hairObjects[i].GetComponent<SkinnedMeshRenderer>();

                if (skinMeshRenderer != null)
                    skinMeshRenderer.material.SetColor("_Color_Hair", hairColor);
            }
        }

        public void ChangeFacialFeaturesOnDummy(int headStyle)
        {
            //male only features
            if (isMale)
            {
                //do a for loop, disable all features
                //enable the specific features you have based on their saved id(ex: headsstyle7,8,9)
            }
            //female only features
            else
            {
                //do a for loop, disable all features
                //enable the specific features you have based on their saved id(ex: headsstyle7,8,9)
            }

            //apply skin color is applicable after features have been changed
        }
    }
}