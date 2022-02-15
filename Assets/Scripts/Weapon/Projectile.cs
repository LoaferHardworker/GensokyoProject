using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DestroyWithTime))]
public class Projectile : MonoBehaviour
{
	private Rigidbody2D rb;

	[SerializeField] private int rebounds = 0;
	[SerializeField] private float speed = 10;
	private float strength = 1;
	private float damage = 1;

	public float Strength
	{
		get => strength;
		set
		{
			strength = value;
			if (rb != null) rb.velocity = transform.right * speed * strength;
		}
	}

	public float Damage { get; set; }

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.right * speed * strength;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(gameObject.tag)) return;

		MortalEntity mortal = other.GetComponent<MortalEntity>();

		if (mortal == null) return;

		if(mortal.Hit(damage))
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (rebounds-- <= 0) Destroy(gameObject);
	}
}