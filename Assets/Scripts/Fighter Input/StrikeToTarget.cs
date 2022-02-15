using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fighter Input/Strike to a target")]
public class StrikeToTarget : FighterInput
{
	public Transform target = null;

	private Transform transform;
	private bool ready;

	public override Vector3? AimDirection()
	{
		if (target == null) return null;
		return target.position;
	}

	public override bool DoAttack()
	{
		if (ready)
		{
			ready = false;
			return true;
		}

		return false;
	}

	public void SetReady()
	{
		ready = true;
	}
}