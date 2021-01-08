using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : CommonCharacterController
{
	public void SetCamera (Camera cam)
	{
		camera = cam;
	}


    public Dictionary<string, string> plotVariables = new Dictionary<string, string>()
    {
        { "AGGRESSIVE", "false" }
    };
    
	float cam_ray_length = 100.0f;
	new Camera camera;

	BoneCombiner boneCombiner;
	GameObject helmet;
	GameObject armor;
	GameObject gauntlets;
	GameObject pants;
	GameObject cloak;
	GameObject boots;

	protected override void Start ()
	{
		base.Start ();
		anim = GetComponent<Animator> ();

		equipment.helmet.OnExchange += UpdateHelmet;
		equipment.platebody.OnExchange += UpdateArmor;
		equipment.gloves.OnExchange += UpdateGauntlets;
		equipment.pants.OnExchange += UpdatePants;
		equipment.cloak.OnExchange += UpdateCloak;
		equipment.shoes.OnExchange += UpdateBoots;

		boneCombiner = new BoneCombiner(gameObject);
	}

	protected override void Update ()
	{
		if (!EventSystem.current.IsPointerOverGameObject ())
		{
			if (Input.GetMouseButton (0))
			{
				Ray ray = camera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, cam_ray_length))
				{
					FindTarget (hit);
				}
			}

			if (Input.GetButtonDown ("Fire2"))
			{
				Ray ray = camera.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, cam_ray_length))
				{
					if (hit.transform.tag == "Enemy")
					{
						SetTarget (hit.transform.gameObject);
						Motor.FollowTarget (hit.transform.gameObject, ActionDistance);
					}
					else if (hit.transform.tag == "NPC")
					{
						SetTarget (hit.transform.gameObject);
						Motor.FollowTarget (hit.transform.gameObject, ActionDistance);
					}
				}
			}
		}

		base.Update ();
	}


	void FindTarget (RaycastHit hit)
	{
		if (hit.transform.tag == "Enemy")
		{
			SetTarget (hit.transform.gameObject);
			if (action == Action.ATTACK)
				Motor.FollowTarget (hit.transform.gameObject, ActionDistance);
		}
		else if (hit.transform.tag == "NPC")
		{
			SetTarget (hit.transform.gameObject);
			if (action != Action.NONE)
				Motor.FollowTarget (hit.transform.gameObject, ActionDistance);
		}
		else if (hit.transform.tag == "Floor")
		{
			action = Action.NONE;
			Motor.MoveToPoint (hit.point);
		}
	}

	void UpdateHelmet()
	{
		GameObject newHelmet;
		if (equipment.helmet.item == null)
		{
			newHelmet = null;
		}
		else
		{
			newHelmet = EquipmentManager.getInstance().CreateEquipment(true, equipment.helmet.item.type, (equipment.helmet.item as Armor).objectName);
		}

		Transform newEquipment = UseEquipment(newHelmet);

		Destroy(helmet);
		if (newEquipment)
			helmet = newEquipment.gameObject;
	}

	void UpdateArmor()
	{
		GameObject newArmor;
		if (equipment.platebody.item == null)
		{
			newArmor = null;
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
			newPants = null;
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
			newCloak = null;
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
			newGauntlets = null;
		}
		else
		{
			newGauntlets = EquipmentManager.getInstance().CreateEquipment(false, equipment.gloves.item.type, (equipment.gloves.item as Armor).objectName);
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
			newBoots = null;
		}
		else
		{
			newBoots = EquipmentManager.getInstance().CreateEquipment(false, equipment.shoes.item.type, (equipment.shoes.item as Armor).objectName);
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
