using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class MortalEntity : MonoBehaviour
{
	private Collider2D _hitbox;

	public readonly UnityEvent<float> OnHealthChange = new UnityEvent<float>();

	[SerializeField] private float _maxHealth;
	
	public float MaxHealth { get => _maxHealth; }
	public float CurrentHealth { get; private set; }
	public Collider2D HitBox { get => _hitbox; }

	private void Start()
	{
		if (CurrentHealth == 0)
		{
			CurrentHealth = _maxHealth;
			OnHealthChange.Invoke(0);
		}

		_hitbox = GetComponent<Collider2D>();
		_hitbox.isTrigger = true;
	}

	public bool Hit(float damage)
	{
		CurrentHealth -= damage;
		
		OnHealthChange.Invoke(-damage);

		if (CurrentHealth <= 0)
			Destroy(gameObject);

		return true;
	}

	public bool Heal(float hp)
	{
		CurrentHealth += hp;
		
		OnHealthChange.Invoke(hp);

		if (CurrentHealth > _maxHealth)
			CurrentHealth = _maxHealth;

		return true;
	}
}