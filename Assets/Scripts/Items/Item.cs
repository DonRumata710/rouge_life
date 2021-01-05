using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Item")]
public class Item : ScriptableObject
{
	public enum Type
	{
		NONE = 0,
		HELMET = 1,
		PLATEBODY = 2,
		CLOAK = 4,
		GLOVES = 8,
		BELT = 16,
		PANTS = 32,
		SHOES = 64,
		SHIELD = 128,
		NECKLACE = 256,
		RING = 512,
		QUIVER = 1024,
		WEAPON = 2048,
        OTHER = 4096
	}

	public Type type;

	public string itemName;
	public Sprite icon;
}
