using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
	[SerializeField] private Weapon _weapon;
	[SerializeField] private FighterInput input;

	public Weapon weapon {
		get => _weapon;
		set => _weapon = value;
	}

	public FighterInput Input { get => input; set => input = value; }

	private void Start()
	{
		if (input == null)
			throw new NullReferenceException($"Fighter Input of {transform.ToString()} is missing");

		input = Instantiate(input);
		input.Init(transform);
	}

	public void FixedUpdate() {
		if (input == null)
			throw new NullReferenceException($"Fighter Input of {transform.ToString()} is missing");

		if (input.DoAttack()) Attack();

		Vector3? dir = input.AimDirection();
		if (dir != null) Aim((Vector3)dir);
	}

	private void Attack()
	{
		if (_weapon == null) return;

		_weapon.Attack();
	}

	private void Aim(Vector3 target)
	{
		if (_weapon == null) return;

		_weapon.Aim(target);
	}
}