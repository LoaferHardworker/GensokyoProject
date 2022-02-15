using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fighter Input/Player")]
public class PlayerFighterInput : FighterInput
{
	public override bool DoAttack() {
		return Input.GetButton("Fire1");
	}

	public override Vector3? AimDirection() {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}