using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move Input/Moveing in the direction or staying")]
public class MoveInDirection : MoveInput
{
	private Transform transform;
	public Vector2 direction = Vector2.positiveInfinity;

	public override void Init(Transform t) => transform = t;

	public override Vector2 UpdateDirection()
	{
		if (direction.x == float.PositiveInfinity) return Vector2.zero;

		return direction;
	}
}