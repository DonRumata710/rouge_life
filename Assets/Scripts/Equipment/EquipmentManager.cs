using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private static EquipmentManager instance;

    private static string prefix = "PT_Medieval_";

    private Transform maleModel;
    private Transform femaleModel;

    private Transform maleBody;
    private Transform femaleBody;

    EquipmentManager()
    {
        if (instance != null)
            throw new System.Exception("Equipment manager has been already created");

        Debug.Log("Equipment manager was created");
        instance = this;
    }

    static public EquipmentManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        maleModel = transform.Find("MaleEquipmentModel");
        femaleModel = transform.Find("FemaleEquipmentModel");

        maleBody = transform.Find("EmptyMaleBody");
        femaleBody = transform.Find("EmptyFemaleBody");

        if (maleModel == null || femaleModel == null)
        {
            throw new System.Exception("Equipment management doesn't have full model set");
        }
    }

    public GameObject GetDefaultBody(bool male, Item.Type type, string suffix = "")
    {
        Transform targetBody = male ? maleBody : femaleBody;
        switch(type)
        {
        case Item.Type.HELMET:
            GameObject hair = targetBody.Find(prefix + getSex(male) + "_Armor_hair_" + suffix).gameObject; //PT_Medieval_Male_Armor_hair_01
            if (hair == null)
                return null;
            return hair;
        case Item.Type.PLATEBODY:
            GameObject body = targetBody.Find(prefix + getSex(male) + "_Armor_naked_" + suffix + "_body").gameObject;
            if (body == null)
                return targetBody.Find(prefix + getSex(male) + "_Armor_naked_body").gameObject;
            return body;
        case Item.Type.GLOVES:
             GameObject hands = targetBody.Find(prefix + getSex(male) + "_Armor_naked_" + suffix + "_gauntlets").gameObject;
             if (hands == null)
                 return targetBody.Find(prefix + getSex(male) + "_Armor_naked_gauntlets").gameObject;
             return hands;
         case Item.Type.PANTS:
             GameObject legs = targetBody.Find(prefix + getSex(male) + "_Armor_naked_" + suffix + "_legs").gameObject;
             if (legs == null)
                 return targetBody.Find(prefix + getSex(male) + "_Armor_naked_legs").gameObject;
             return legs;
         case Item.Type.SHOES:
             GameObject feet = targetBody.Find(prefix + getSex(male) + "_Armor_naked_" + suffix + "_boots").gameObject;
             if (feet == null)
                 return targetBody.Find(prefix + getSex(male) + "_Armor_naked_boots").gameObject;
             return feet;

        case Item.Type.QUIVER:
            break;
        case Item.Type.WEAPON:
            break;
        case Item.Type.SHIELD:
            break;
        case Item.Type.CLOAK:
            break;
        case Item.Type.NECKLACE:
            break;
        case Item.Type.RING:
            break;
        case Item.Type.BELT:
            break;
        case Item.Type.OTHER:
            break;
        case Item.Type.NONE:
            break;
        }

        return null;
    }

    public GameObject CreateEquipment(bool male, Item.Type type, string suffix)
    {
        Transform equipmentTransform;

        if (male)
            equipmentTransform = maleModel.Find(prefix + "Male_Armor_" + suffix + "_" + getEquipmentName(type));
        else
            equipmentTransform = femaleModel.Find(prefix + "Female_Armor_" + suffix + "_" + getEquipmentName(type));

        if (equipmentTransform != null)
            return equipmentTransform.gameObject;
        else
            return null;
    }

    string getSex(bool male)
    {
        return male ? "Male" : "Female";
    }

    string getEquipmentName(Item.Type type)
    {
        switch(type)
        {
            case Item.Type.HELMET:
                return "helmet";
            case Item.Type.PLATEBODY:
                return "body";
            case Item.Type.GLOVES:
                return "gauntlets";
            case Item.Type.PANTS:
                return "legs";
            case Item.Type.SHOES:
                return "boots";
            case Item.Type.CLOAK:
                return "cape";
            default:
                return "";
        }
    }
}
