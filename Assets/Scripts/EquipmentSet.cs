using UnityEngine;


[RequireComponent(typeof (Inventory))]
public class EquipmentSet : MonoBehaviour
{
	public EquipmentSlot rightHand = new EquipmentSlot();
	public EquipmentSlot leftHand = new EquipmentSlot();

	public EquipmentSlot helmet = new EquipmentSlot();
	public EquipmentSlot platebody = new EquipmentSlot();
	public EquipmentSlot cloak = new EquipmentSlot();
	public EquipmentSlot gloves = new EquipmentSlot();
    public EquipmentSlot belt = new EquipmentSlot();
    public EquipmentSlot pants = new EquipmentSlot();
    public EquipmentSlot shoes = new EquipmentSlot();
    public EquipmentSlot necklace = new EquipmentSlot();
    public EquipmentSlot ring1 = new EquipmentSlot();
    public EquipmentSlot ring2 = new EquipmentSlot();
    public EquipmentSlot quiver = new EquipmentSlot();
    


    public void CalcEquipment()
    {
        CharacterParametersSet param = ScriptableObject.CreateInstance<CharacterParametersSet>();

        Weapon weapon = rightHand.item as Weapon;
        if (weapon)
        {
            weapon = rightHand.item as Weapon;
            param.right_hand_action_distance = weapon.action_distance;
            param.right_hand_attack_frequency = weapon.attack_frequency;
            param.right_hand_damage = weapon.damage;
        }

        Armor armor = leftHand.item as Armor;
        weapon = leftHand.item as Weapon;
        if (armor)
            param.armor += armor.armor;
        else if (weapon)
        {
            weapon = leftHand.item as Weapon;
            param.left_hand_action_distance = weapon.action_distance;
            param.left_hand_attack_frequency = weapon.attack_frequency;
            param.left_hand_damage = weapon.damage;
        }

        armor = helmet.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = platebody.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = cloak.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = gloves.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = belt.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = pants.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = shoes.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = necklace.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = ring1.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = ring2.item as Armor;
        if (armor)
            param.armor += armor.armor;
        armor = quiver.item as Armor;
        if (armor)
            param.armor += armor.armor;

        CharacterParametersSet.EffectsSet effects = new CharacterParametersSet.EffectsSet();

        if (helmet.item)
            effects.ApplyEffects(helmet.item.effects);
        if (platebody.item)
             effects.ApplyEffects(platebody.item.effects);
        if (cloak.item)
             effects.ApplyEffects(cloak.item.effects);
        if (gloves.item)
             effects.ApplyEffects(gloves.item.effects);
        if (belt.item)
             effects.ApplyEffects(belt.item.effects);
        if (pants.item)
             effects.ApplyEffects(pants.item.effects);
        if (shoes.item)
             effects.ApplyEffects(shoes.item.effects);
        if (necklace.item)
             effects.ApplyEffects(necklace.item.effects);
        if (ring1.item)
             effects.ApplyEffects(ring1.item.effects);
        if (ring2.item)
             effects.ApplyEffects(ring2.item.effects);
        if (quiver.item)
             effects.ApplyEffects(quiver.item.effects);
        if (rightHand.item)
             effects.ApplyEffects(rightHand.item.effects);
        if (leftHand.item)
             effects.ApplyEffects(leftHand.item.effects);

        param.UseEffects(effects);

        Stats stats = GetComponent<CommonCharacterController>().Parameters;
        stats.EquipmentParameters = param;
    }


    void Awake()
    {
        rightHand.SetType(Item.Type.WEAPON);
        leftHand.SetType(Item.Type.WEAPON | Item.Type.SHIELD);

        helmet.SetType(Item.Type.HELMET);
        platebody.SetType(Item.Type.PLATEBODY);
        cloak.SetType(Item.Type.CLOAK);
        gloves.SetType(Item.Type.GLOVES);
        belt.SetType(Item.Type.BELT);
        pants.SetType(Item.Type.PANTS);
        shoes.SetType(Item.Type.SHOES);
        necklace.SetType(Item.Type.NECKLACE);
        ring1.SetType(Item.Type.RING);
        ring2.SetType(Item.Type.RING);
        quiver.SetType(Item.Type.QUIVER);

        rightHand.OnExchange += CalcEquipment;
		leftHand.OnExchange += CalcEquipment;

		helmet.OnExchange += CalcEquipment;
		platebody.OnExchange += CalcEquipment;
		cloak.OnExchange += CalcEquipment;
		gloves.OnExchange += CalcEquipment;
		belt.OnExchange += CalcEquipment;
		pants.OnExchange += CalcEquipment;
		shoes.OnExchange += CalcEquipment;
		necklace.OnExchange += CalcEquipment;
		ring1.OnExchange += CalcEquipment;
		ring2.OnExchange += CalcEquipment;
		quiver.OnExchange += CalcEquipment;
    }
}
