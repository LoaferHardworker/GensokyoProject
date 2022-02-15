using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public abstract float Cooldown { get; }

	public abstract void Attack();

	public virtual void Aim(Vector3 target)
	{
		float angle = Vector2.Angle(Vector2.right, target - transform.position);
		
		transform.eulerAngles = new Vector3(0, 0, (target.y < transform.position.y ? -1 : 1) * angle);
	}
}