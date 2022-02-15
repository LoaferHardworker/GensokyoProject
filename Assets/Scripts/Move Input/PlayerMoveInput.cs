using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move Input/Player")]
public class PlayerMoveInput : MoveInput
{
	public override Vector2 UpdateDirection()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}
}