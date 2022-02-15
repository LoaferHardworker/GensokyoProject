using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleGun : Weapon
{
	[SerializeField] private Projectile _projectile;
	[SerializeField] private Transform _projectileSpawnPoint;

	[Range(0, 10)][SerializeField] private float cooldown = 1f;
	[Range(0, 10)][SerializeField] private float strength = 1f;
	[Range(0, 10)][SerializeField] private float damage = 1f;

	public override float Cooldown { get => cooldown; }

	private bool canShoot = true;

	private void Start()
	{
		if (_projectileSpawnPoint == null)
			_projectileSpawnPoint = transform.Find("projectileSpawnPoint");

		if (_projectileSpawnPoint == null)
			_projectileSpawnPoint = transform;
	}

	public override void Attack()
	{
		if (!canShoot) return;

		Projectile instance = Instantiate(_projectile.gameObject,
				_projectileSpawnPoint.position,
				transform.rotation).GetComponent<Projectile>();

		instance.gameObject.tag = gameObject.tag;
		instance.Strength = strength;
		instance.Damage = damage;

		StartCoroutine(Reload());
	}

	private IEnumerator Reload()
	{
		canShoot = false;
		yield return new WaitForSeconds(cooldown);
		canShoot = true;
	}
}