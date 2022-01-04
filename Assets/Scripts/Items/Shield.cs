using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Shield")]
public class Shield : Equipment
{
	public int armor = 1;
	public GameObject instance = null;
}
