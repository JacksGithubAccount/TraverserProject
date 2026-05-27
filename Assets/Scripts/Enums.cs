using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    Team01, //player
    Team02, //enemies
    Team03  //npc
}
//used to tag sliders for level up ui
public enum CharacterAttribute
{
    Vigor,
    Mind,
    Endurance,
    Strength,
    Dexterity,
    Intelligence,
    Faith,
    Luck
}
//used to give characters proper dialogue sets
public enum CharacterDialogueID
{
    NoDialogueID,
    NamelessKnightDialogueID,
    BlacksmithDialogueID,
    ShopkeeperDialogueID
}

//used for determining dialogue from selecting talk in the menus
public enum CharacterMenuDialogueID
{
    NoDialogueID,
    BlacksmithTalkDialogueID
}

public enum DialogueEndEvents
{
    None,
    Blacksmith,
    NPCWindow 	//sometimes NPC's will have a window open where you can buy/sell/talk, if you don't want to jump straight to a merchant window, use this

}

//Used to determine which ship inventory should be loaded
public enum Shops
{
    None,
    TutorialShop
}

public enum ShopBuyOrSell
{
    Buying,
    Selling
}

//determines build up status effect type
public enum BuildUp
{
    Poison,
    Bleed,
    Frost
}

public enum PhysicalDamageType
{
    Regular,
    Blunt,
    Pierce,
    Slash
}

public enum DoorState
{
    Open,
    Locked,
    CantOpenFromThisSide
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

//determines upgrade level of an item
public enum UpgradeLevel
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten
}

public enum UpgradeStone
{
    Small,
    Medium,
    Large,
    Slab
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
    Legs,
    MainProjectile,
    SecondaryProjectile,
    QuickSlot01,
    QuickSlot02,
    QuickSlot03,
    Accessory01,
    Accessory02,
    Accessory03,
    Accessory04
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
    HeavyJumpingAttack01,
    DualLightAttack01,
    DualLightAttack02,
    DualHeavyAttack01,
    DualHeavyAttack02,
    DualChargedAttack01,
    DualChargedAttack02,
    DualRunningLightAttack01,
    DualRunningHeavyAttack01,
    DualRollingLightAttack01,
    DualRollingHeavyAttack01,
    DualBackstepLightAttack01,
    DualBackstepHeavyAttack01,
    DualJumpingLightAttack01,
    DualJumpingHeavyAttack01

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

//determines projectile impact
public enum ThrowableType
{
    Destructible,
    Lingering,
    Persistant
}

public enum ItemCategory
{
    None,
    Plant,
    Rock,
    Wood,
    Flammable,
    Fuel
}

public enum ItemType
{
    None,
    Tool,
    CraftingMaterial,
    UpgradeMaterial,
    KeyItem,
    Sorcery,
    Incantation,
    Pyromancy,
    AshesOfWar,
    MeleeWeapon,
    RangedWeaponAndCatalyst,
    ArrowAndBolt,
    Shield,
    HeadEquipment,
    ChestEquipment,
    ArmEquipment,
    LegEquipment,
    Accessory,
    Info,
    Gestures
}
//AI States
public enum IdleStateMode
{
    Idle,
    Patrol,
    Sleep
}

//scenes
public enum WorldSceneLocation
{
    Area01_Subarea00,
    Area01_Subarea01,
    Area01_Subarea02,
    Area01_Subarea03,
    Area01_Subarea04,
    Area01_Subarea05
}