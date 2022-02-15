using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class FighterInput : ScriptableObject
{
	public abstract bool DoAttack();
	public abstract Vector3? AimDirection();
	public virtual void Init(Transform t) {}
}