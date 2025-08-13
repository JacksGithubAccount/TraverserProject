using UnityEngine;

public class Enums : MonoBehaviour
{

}
//character saving
public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}
//process damage and character targeting
public enum CharacterGroup
{
    Team01,
    Team02,
    Team03
}
//is tag for each weapon model instantiation slot
public enum WeaponModelSlot
{
    RightHand,
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    RightHipSlot,
    LeftHipSlot,
    BackSlot
}
//where to instantiate model based on model type
public enum WeaponModelType
{
    Weapon,
    Shield
}
//information specific to weapon class
public enum WeaponClass
{
    StraightSword,
    Spear,
    MediumShield,
    Fist,
    Axe,
    LightShield,
    Bow
}

//determines which catalyst is used to cast spell
public enum SpellClass
{
    Incantation,
    Sorcery
}

//determines which range weapon can fire this ammo
public enum ProjectileClass
{
    Arrow,
    Bolt
}


public enum ProjectileSlot
{
    Main,
    Secondary
}

//is used to tag equipment models with specific body parts that the equipment will cover
public enum EquipmentModelType
{
    FullHelmet,
    Hat,
    Hood,
    HelmetAccessories,
    FaceCover,
    Torso,
    Back,
    RightShoulder,
    RightUpperArm,
    RightElbow,
    RightLowerArm,
    RightHand,
    LeftShoulder,
    LeftUpperArm,
    LeftElbow,
    LeftLowerArm,
    LeftHand,
    Hips,
    HipsAttachment,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee
}
//determines with equipment slot is currently selected
public enum EquipmentType
{
    RightWeapon01,
    RightWeapon02,
    RightWeapon03,
    LeftWeapon01,
    LeftWeapon02,
    LeftWeapon03,
    Head,
    Body,
    Hands,
    Legs

}
// tags helmetsfor specific head portions to cover
public enum HeadEquipmentType
{
    FullHelmet,
    Hat,
    Hood,
    FaceCover
}

//used to calc damage
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
    RunningLightAttack01,
    RunningHeavyAttack01,
    RollingLightAttack01,
    RollingHeavyAttack01,
    BackstepLightAttack01,
    BackstepHeavyAttack01,
    LightJumpingAttack01,
    HeavyJumpingAttack01
}
//calculate damage animation intensity
public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}
//determines item pick up type
public enum ItemPickUpType
{
    WorldSpawn,
    CharacterDrop
}