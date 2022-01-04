using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;
	List<GameObject> players;

	void Start ()
	{
		players = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Player"));
		setTarget(players[0]);
	}

	void FixedUpdate ()
	{
        if (target)
		    transform.position = new Vector3 (target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
	}

    public void ResetTarget()
    {
		players.Remove(target.gameObject);
        target = null;
		if (players.Count == 0)
			players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
		if (players.Count > 0)
			setTarget(players[0]);
    }

	void setTarget(GameObject player)
    {
		CommonCharacterController controller = player.GetComponent<CommonCharacterController>();
		controller.OnDeath += ResetTarget;
		PlayerManager playerManager = player.GetComponent<PlayerManager>();
		playerManager.SetCamera(GetComponent<Camera>());

		target = player.transform;
		transform.position = new Vector3(target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
	}
}