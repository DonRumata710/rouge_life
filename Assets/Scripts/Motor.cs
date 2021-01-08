using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Motor : MonoBehaviour
{
	UnityEngine.AI.NavMeshAgent agent;
	Animator anim;
	GameObject target;
	float smooth_time = 0.1f;

	void Start ()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		anim = GetComponent<Animator> ();
	}

	void Update ()
    {
        if (!enabled)
            return;

        if (target != null)
		{
			agent.SetDestination (target.transform.position);
			FaceTarget ();
		}

		float speed_percent = agent.velocity.magnitude / agent.speed;
		anim.SetFloat ("speed percent", speed_percent, smooth_time, Time.deltaTime);
	}
	
	public void MoveToPoint(Vector3 point)
    {
        if (!enabled)
            return;

        ResetTarget();
		agent.SetDestination (point);
	}

	public void Stop ()
    {
        if (!enabled)
            return;

        ResetTarget();
        agent.SetDestination (transform.position);
	}

	public void FollowTarget(GameObject targ, float distance)
    {
        if (!enabled)
            return;

        agent.stoppingDistance = distance;
		agent.updateRotation = false;
		target = targ;
	}

	public void ResetTarget()
    {
        if (!enabled)
            return;

        agent.stoppingDistance = 0.0f;
		agent.updateRotation = true;
		target = null;
	}

	public bool IsOnPlace()
	{
        if (!enabled || agent.pathPending)
            return false;

		return agent.remainingDistance <= agent.stoppingDistance + float.Epsilon;
	}

    public void OnDisable()
    {
        if (agent)
            agent.enabled = false;
    }

    public void OnEnable()
    {
        if (agent)
            agent.enabled = true;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation (new Vector3 (direction.x, 0f, direction.z));
		transform.rotation = Quaternion.Slerp (transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
}
