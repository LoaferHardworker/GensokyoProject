using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fighter Input/Joseph")]
public class JosephFighterInput : FighterInput
{
	private Transform transform;

	public override Vector3? AimDirection()
	{
		return null;
	}

	public override bool DoAttack()
	{
		return false;
	}
}