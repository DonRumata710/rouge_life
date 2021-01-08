using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Weapon")]
public class Weapon : Equipment
{
	public int damage = 1;
	public float attack_frequency = 1.0f;
	public float action_distance = 2.0f;
	public GameObject instance = null;
}
