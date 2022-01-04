using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EquipmentSet))]
public class HumanoidController : CommonCharacterController
{
    public EquipmentSet equipment
    {
        private set;
        get;
    }

    BoneCombiner boneCombiner;
    GameObject helmet;
    GameObject armor;
    GameObject gauntlets;
    GameObject pants;
    GameObject cloak;
    GameObject boots;

    GameObject beard;

    Transform rightHand;
    Transform leftHand;
    Transform largeShieldSlot;
    Transform mediumShieldSlot;
    Transform smallShieldSlot;


    protected override void Start()
    {
        base.Start();

        equipment = GetComponent<EquipmentSet>();
        equipment.CalcEquipment();

        equipment.rightHand.OnExchange += UpdateRightHand;
        equipment.leftHand.OnExchange += UpdateLeftHand;

        equipment.helmet.OnExchange += UpdateHelmet;
        equipment.platebody.OnExchange += UpdateArmor;
        equipment.gloves.OnExchange += UpdateGauntlets;
        equipment.pants.OnExchange += UpdatePants;
        equipment.cloak.OnExchange += UpdateCloak;
        equipment.shoes.OnExchange += UpdateBoots;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            if (child.name.Contains("hair"))
                helmet = child.gameObject;
            else if (child.name.Contains("beard"))
                beard = child.gameObject;
            else if (child.name.Contains("body"))
                armor = child.gameObject;
            else if (child.name.Contains("gauntlets"))
                gauntlets = child.gameObject;
            else if (child.name.Contains("legs"))
                pants = child.gameObject;
            else if (child.name.Contains("boots"))
                boots = child.gameObject;
        }

        boneCombiner = new BoneCombiner(gameObject);

        string path = "PT_Medieval_Male_Armor_Hips/PT_Medieval_Male_Armor_Spine/PT_Medieval_Male_Armor_Spine2/PT_Medieval_Male_Armor_Spine3/";
        string rightHandPath = path + "PT_Medieval_Male_Armor_RightShoulder/PT_Medieval_Male_Armor_RightArm/PT_Medieval_Male_Armor_RightForeArm/PT_Medieval_Male_Armor_RightHand/";
        string leftHandPath = path + "PT_Medieval_Male_Armor_LeftShoulder/PT_Medieval_Male_Armor_LeftArm/PT_Medieval_Male_Armor_LeftForeArm/";

        rightHand = transform.Find(rightHandPath + "PT_Medieval_Male_Armor_Right_Hand_Weapon_slot");
        leftHand = transform.Find(leftHandPath + "PT_Medieval_Male_Armor_LeftHand/PT_Medieval_Male_Armor_Left_Hand_Weapon_slot");
        largeShieldSlot = transform.Find(leftHandPath + "PT_Medieval_Male_Armor_Large_Shield_slot");
        mediumShieldSlot = transform.Find(leftHandPath + "PT_Medieval_Male_Armor_Medium_Shield_slot");
        smallShieldSlot = transform.Find(leftHandPath + "PT_Medieval_Male_Armor_Small_Shield_slot");
    }


    public void AddWeaponToRightHand(Transform weapon)
    {
        rightHand.DetachChildren();
        Instantiate(weapon, rightHand);
    }

    public void AddWeaponToLeftHand(Transform weapon)
    {
        leftHand.DetachChildren();
        largeShieldSlot.DetachChildren();
        Instantiate(weapon, leftHand);
    }

    public void AddLargeShield(Transform shield)
    {
        AddShiled(largeShieldSlot, shield);
    }

    public void AddMediumShield(Transform shield)
    {
        AddShiled(mediumShieldSlot, shield);
    }

    public void AddSmallShield(Transform shield)
    {
        AddShiled(smallShieldSlot, shield);
    }

    private void AddShiled(Transform shieldSlot, Transform shield)
    {
        leftHand.DetachChildren();
        largeShieldSlot.DetachChildren();
        Instantiate(shield, shieldSlot);
    }

    protected override void ProvideAttack()
    {
        if (right_hand_attack_timeout <= 0.0f)
        {
            anim.SetTrigger("attack");
            Target.MakeDamage(Parameters.RightHandDamage, this);
            right_hand_attack_timeout = 1.0f / Parameters.RightHandFrequency;
        }
        if (left_hand_attack_timeout <= 0.0f && (equipment.rightHand.item is Weapon || (equipment.leftHand.item == null && equipment.rightHand.item == null)) && Target)
        {
            anim.SetTrigger("left hand attack");
            Target.MakeDamage(Parameters.LeftHandDamage, this);
            left_hand_attack_timeout = 1.0f / Parameters.LeftHandFrequency;
        }
    }


    void UpdateRightHand()
    {
        Weapon weapon = equipment.rightHand.item as Weapon;
        if (weapon != null)
            AddWeaponToRightHand(weapon.instance.transform);
    }

    void UpdateLeftHand()
    {
        Weapon weapon = equipment.leftHand.item as Weapon;
        if (weapon != null)
        {
            AddWeaponToLeftHand(weapon.instance.transform);
        }
        else
        {
            Shield shield = equipment.leftHand.item as Shield;
            if (shield)
                AddLargeShield(shield.instance.transform);
        }
    }

    void UpdateHelmet()
    {
        GameObject newHelmet;
        if (equipment.helmet.item == null)
        {
            newHelmet = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.HELMET, "01");
        }
        else
        {
            newHelmet = EquipmentManager.getInstance().CreateEquipment(true, equipment.helmet.item.type, (equipment.helmet.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newHelmet);

        Destroy(helmet);
        if (newEquipment != null)
            helmet = newEquipment.gameObject;
    }

    void UpdateArmor()
    {
        GameObject newArmor;
        if (equipment.platebody.item == null)
        {
            newArmor = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.PLATEBODY, "00");
        }
        else
        {
            newArmor = EquipmentManager.getInstance().CreateEquipment(true, equipment.platebody.item.type, (equipment.platebody.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newArmor);

        Destroy(armor);
        if (newEquipment)
            armor = newEquipment.gameObject;
    }

    void UpdatePants()
    {
        GameObject newPants;
        if (equipment.pants.item == null)
        {
            newPants = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.PANTS, "00");
        }
        else
        {
            newPants = EquipmentManager.getInstance().CreateEquipment(true, equipment.pants.item.type, (equipment.pants.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newPants);

        Destroy(pants);
        if (newEquipment)
            pants = newEquipment.gameObject;
    }

    void UpdateCloak()
    {
        GameObject newCloak;
        if (equipment.cloak.item == null)
        {
            newCloak = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.CLOAK, "00");
        }
        else
        {
            newCloak = EquipmentManager.getInstance().CreateEquipment(true, equipment.cloak.item.type, (equipment.cloak.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newCloak);

        Destroy(cloak);
        if (newEquipment)
            cloak = newEquipment.gameObject;
    }

    void UpdateGauntlets()
    {
        GameObject newGauntlets;
        if (equipment.gloves.item == null)
        {
            newGauntlets = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.GLOVES, "00");
        }
        else
        {
            newGauntlets = EquipmentManager.getInstance().CreateEquipment(true, equipment.gloves.item.type, (equipment.gloves.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newGauntlets);

        Destroy(gauntlets);
        if (newEquipment)
            gauntlets = newEquipment.gameObject;
    }

    void UpdateBoots()
    {
        GameObject newBoots;
        if (equipment.shoes.item == null)
        {
            newBoots = EquipmentManager.getInstance().GetDefaultBody(true, Item.Type.SHOES, "00");
        }
        else
        {
            newBoots = EquipmentManager.getInstance().CreateEquipment(true, equipment.shoes.item.type, (equipment.shoes.item as Armor).objectName);
        }

        Transform newEquipment = UseEquipment(newBoots);

        Destroy(boots);
        if (newEquipment)
            boots = newEquipment.gameObject;
    }

    Transform UseEquipment(GameObject equipment)
    {
        if (equipment == null)
            return null;
        if (!equipment.GetComponent<SkinnedMeshRenderer>())
            return null;

        var renderer = equipment.GetComponent<SkinnedMeshRenderer>();
        var bones = renderer.bones;

        List<string> boneNames = new List<string>(bones.Length);
        foreach (var t in bones)
        {
            boneNames.Add(t.name);
        }

        return boneCombiner.AddLimb(equipment, boneNames);
    }
}
