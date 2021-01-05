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

	protected override void Start ()
	{
		base.Start ();
		anim = GetComponent<Animator> ();
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
}
