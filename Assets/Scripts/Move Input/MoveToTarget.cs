using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move Input/Moveing to target or staying")]
public class MoveToTarget : MoveInput
{
	private Transform transform;
	public Vector2 target = Vector2.positiveInfinity;

	public override void Init(Transform t) => transform = t;

	public override Vector2 UpdateDirection()
	{
		if (target.x == float.PositiveInfinity) return Vector2.zero;

		return target - (Vector2)transform.position;
	}
}