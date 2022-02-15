using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private MoveInput input;
	[SerializeField] private float speed;

	public float Speed { get => speed; }
	public MoveInput Input { get => input; set => input = value; }

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		if (input == null)
			throw new NullReferenceException($"Move Input of {transform.ToString()} is missing");

		input = Instantiate(input);
		input.Init(transform);
	}
	
	private void FixedUpdate()
	{
		if (input == null)
			throw new NullReferenceException($"Move Input of {transform.ToString()} is missing");

		rb.velocity = input.UpdateDirection().normalized * speed;
	}

	//private void InitInput() => input.Init(transform);
}