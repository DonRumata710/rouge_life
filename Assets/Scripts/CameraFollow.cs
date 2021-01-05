using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;

	void Start ()
	{
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetCamera (GetComponent<Camera> ());
        controller.OnDeath += ResetTarget;

        target = player.transform;
		transform.position = new Vector3 (target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
	}

	void FixedUpdate ()
	{
        if (target)
		    transform.position = new Vector3 (target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
	}

    public void ResetTarget()
    {
        target = null;
    }
}