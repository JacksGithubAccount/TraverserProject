using UnityEngine;

namespace TraverserProject
{

    public class DurkClubDamageCollider : DamageCollider
    {
        [SerializeField] AIBossCharacterManager bossCharacter;


        protected override void Awake()
        {
            base.Awake();

            damageCollider = GetComponent<Collider>();
            bossCharacter = GetComponentInParent<AIBossCharacterManager>();
        }
        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Singleton.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(bossCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);



            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (damageTarget.IsOwner)
            {
                damageTarget.characterNetworkManager.NofityTheServerOfCharacterDamageServerRpc(damageTarget.NetworkObjectId, bossCharacter.NetworkObjectId,
                    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.fireDamage, damageEffect.lightningDamage, damageEffect.holyDamage, damageEffect.poiseDamage,
                    damageEffect.angleHitFrom, damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
            }
        }

    }
}