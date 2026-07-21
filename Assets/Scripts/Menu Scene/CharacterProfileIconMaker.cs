using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace TraverserProject
{

    public class CharacterProfileIconMaker : MonoBehaviour
    {
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private Camera profileIconCamera;
        private string spriteIconName;
        // reference to a 'dummy' to equip with armor/hair etc

        public void CreateAllProfileIcons()
        {
            //take a snapshot of the dummy for each profile saved
            CreateCharacterProfileIcon("characterSlot01", TitleScreenManager.Singleton.characterProfileIcons[0], WorldSaveGameManager.Singleton.characterSlot01);
            CreateCharacterProfileIcon("characterSlot02", TitleScreenManager.Singleton.characterProfileIcons[1], WorldSaveGameManager.Singleton.characterSlot02);
            CreateCharacterProfileIcon("characterSlot03", TitleScreenManager.Singleton.characterProfileIcons[2], WorldSaveGameManager.Singleton.characterSlot03);
            CreateCharacterProfileIcon("characterSlot04", TitleScreenManager.Singleton.characterProfileIcons[3], WorldSaveGameManager.Singleton.characterSlot04);
            CreateCharacterProfileIcon("characterSlot05", TitleScreenManager.Singleton.characterProfileIcons[4], WorldSaveGameManager.Singleton.characterSlot05);
            CreateCharacterProfileIcon("characterSlot06", TitleScreenManager.Singleton.characterProfileIcons[5], WorldSaveGameManager.Singleton.characterSlot06);
            CreateCharacterProfileIcon("characterSlot07", TitleScreenManager.Singleton.characterProfileIcons[6], WorldSaveGameManager.Singleton.characterSlot07);
            CreateCharacterProfileIcon("characterSlot08", TitleScreenManager.Singleton.characterProfileIcons[7], WorldSaveGameManager.Singleton.characterSlot08);
            CreateCharacterProfileIcon("characterSlot09", TitleScreenManager.Singleton.characterProfileIcons[8], WorldSaveGameManager.Singleton.characterSlot09);
            CreateCharacterProfileIcon("characterSlot10", TitleScreenManager.Singleton.characterProfileIcons[9], WorldSaveGameManager.Singleton.characterSlot10);

        }

        private void EquipDummy(CharacterSaveData characterSaveData)
        {
            //equip dummy with gear before each screen shot
            if (characterSaveData == null)
                return;

        }

        private string GetIconSaveLocation()
        {
            string saveLocation = Application.streamingAssetsPath + "/Icons/";

            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }

            return saveLocation;
        }

        private void CreateCharacterProfileIcon(string iconName, Image characterLoadSlotIcon, CharacterSaveData characterSaveData)
	{		
		EquipDummy(characterSaveData);
		
		if(string.IsNullOrEmpty(spriteIconName))
			spriteIconName = iconName;
			
		string savePath = GetIconSaveLocation();
        savePath += spriteIconName;
		
		RenderTexture currentRenderTexture = new RenderTexture(renderTexture.width, renderTexture.height, renderTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        Texture2D imagePNG = new Texture2D(currentRenderTexture.width, currentRenderTexture.height, TextureFormat.ARGB32, true);
        currentRenderTexture.antiAliasing = renderTexture.antiAliasing;
		
		profileIconCamera.targetTexture = currentRenderTexture;
		profileIconCamera.Render();
		RenderTexture.active = currentRenderTexture;
		
		imagePNG.ReadPixels(new Rect(0,0, currentRenderTexture.width, currentRenderTexture.height), 0,0);
		imagePNG.Apply();
		
		RenderTexture.active = currentRenderTexture;
		byte[] bytesPNG = imagePNG.EncodeToPNG();
        System.IO.File.WriteAllBytes(savePath+".png", bytesPNG);
		
		Sprite newSprite = Sprite.Create(imagePNG, new Rect(0, 0, imagePNG.width, imagePNG.height), new Vector2(0, 0), 100f);
        characterLoadSlotIcon.sprite = newSprite;
	}


}
}