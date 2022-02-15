using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MoveInput : ScriptableObject
{
	public abstract Vector2 UpdateDirection();

	public virtual void Init(Transform t) {}
}