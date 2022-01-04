using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;

	int currentPlayer = 0;
	List<GameObject> players;

	int xMovement = 0;
	int zMovement = 0;

	void Start ()
	{
		ResetTarget();
		PlayerManager playerManager = FindObjectOfType<PlayerManager>();
		playerManager.SetCamera(GetComponent<Camera>());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			xMovement = 1;
		if (Input.GetKeyUp(KeyCode.LeftArrow))
			xMovement = 0;
		if (Input.GetKeyDown(KeyCode.RightArrow))
			xMovement = -1;
		if (Input.GetKeyUp(KeyCode.RightArrow))
			xMovement = 0;
		if (Input.GetKeyDown(KeyCode.DownArrow))
			zMovement = 1;
		if (Input.GetKeyUp(KeyCode.DownArrow))
			zMovement = 0;
		if (Input.GetKeyDown(KeyCode.UpArrow))
			zMovement = -1;
		if (Input.GetKeyUp(KeyCode.UpArrow))
			zMovement = 0;

		if (target)
		{
			transform.position = new Vector3(target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
		}
		else
		{
			transform.position = new Vector3(
				transform.position.x + xMovement * Time.deltaTime * 10.0f,
				transform.position.y,
				transform.position.z + zMovement * Time.deltaTime * 10.0f);
		}
	}

    public void ResetTarget()
    {
		players = new List<GameObject> (GameObject.FindGameObjectsWithTag ("Player"));
		if (players.Count > currentPlayer)
		{
			SetTarget(players[currentPlayer]);
		}
		else if (players.Count > 0)
		{
			currentPlayer = 0;
			SetTarget(players[currentPlayer]);
		}
		else
		{
			target = null;
		}
    }

	public void SwitchTarget()
	{
		++currentPlayer;

		if (target == null)
			currentPlayer = 0;

		if (players.Count <= currentPlayer)
			target = null;
		else
			SetTarget(players[currentPlayer]);
	}

	public void SetTarget(GameObject newTarget)
    {
		if (newTarget == null)
		{
			target = null;
			return;
		}

        CommonCharacterController controller = newTarget.GetComponent<CommonCharacterController>();
		controller.OnDeath += ResetTarget;

		target = newTarget.transform;
		transform.position = new Vector3(target.position.x, target.position.y + 30.0f, target.position.z + 30.0f);
	}
}