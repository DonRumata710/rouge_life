using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCController : CommonCharacterController
{
    public void Move(Vector3 point)
    {
        Motor.MoveToPoint(point);
        ResetTarget();
    }

	protected override void Start ()
	{
		base.Start ();
        OnAttacked += AttackReaction;
    }

	protected override void Update ()
	{
		if (Motor.IsOnPlace () && action == Action.NONE)
            NPCManager.instance.GetNewTarget(this, new NPCManager.GameEvent(NPCManager.GameEvent.Type.TARGET_ACHIEVED, TargetObject));

		base.Update ();
	}

	protected override void ReactOnEnemyDeath ()
    {
        NPCManager.instance.GetNewTarget(this, new NPCManager.GameEvent(NPCManager.GameEvent.Type.TARGET_ACHIEVED, TargetObject));
	}

    protected void AttackReaction(CommonCharacterController enemy)
    {
        if (Target == null && enemy != null)
        {
            Target = enemy;
            action = Action.ATTACK;
            enemy.OnDeath += ResetTarget;
            enemy.OnDeath += ReactOnEnemyDeath;
            anim.SetBool("is_aggressive", true);
        }
    }
}
