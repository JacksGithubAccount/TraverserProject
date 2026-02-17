using Unity.VisualScripting;
using UnityEngine;

namespace TraverserProject
{

    public class IllusoryWallObject : MonoBehaviour
    {
        public bool wallHasBeenHit;
        public Material illusoryWallMaterial;
        public MeshRenderer meshRenderer;
        public float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;

        public AudioSource audioSource;
        

        private void Awake()
        {
            illusoryWallMaterial = Instantiate(illusoryWallMaterial);
            meshRenderer.material = illusoryWallMaterial;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(wallHasBeenHit)
            {
                FadeIllusoryWall();
            }    
        }

        private void OnTriggerEnter(Collider other)
        {
            AICharacterManager aiCharacter = other.GetComponent<AICharacterManager>();

            if (aiCharacter != null)
                wallHasBeenHit = true;

            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player != null)
            {
                if (player.playerNetworkManager.isJumping.Value || player.playerNetworkManager.isRolling.Value)
                    wallHasBeenHit = true;
            }

            DamageCollider damageCollider = other.GetComponent<DamageCollider>();

            if (damageCollider != null)
                wallHasBeenHit = true;
        }

        public void FadeIllusoryWall()
        {
            //alpha = illusoryWallMaterial.GetColor("_BaseColor").a;
            alpha = illusoryWallMaterial.color.a;
            alpha = alpha - Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1,1,1,alpha);
            illusoryWallMaterial.color = fadedWallColor;
            //meshRenderer.material.SetColor("_BaseColor", fadedWallColor);

            if (wallCollider.enabled)
            {
                wallCollider.enabled = false;  
                audioSource.PlayOneShot(WorldSoundFXManager.Singleton.illusoryWallSFX);
            }

            if(alpha <= 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
