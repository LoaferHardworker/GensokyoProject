using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperJoseph : MonoBehaviour
{
	public MortalEntity mortal  { get; private set; }
	public Fighter fighter { get; private set; }
	public Movable movable { get; private set; }
	public Vision vision { get; private set; }

	private MoveInDirection directionMoveInput;
	private StrikeToTarget fighterInput;

	[Header("Movement settings")]
	[SerializeField] private Vector2 rangeTimeToStay = new Vector2(0.2f, 1f);
	[SerializeField] private Vector2 rangeTimeToMove = new Vector2(0.2f, 1f);
	[SerializeField][Range(0f, 1f)] private float distanceFromWalls = 0.3f;
	[SerializeField][Range(0f, 1f)] private float distanceFromOthers = 0.5f;
	[SerializeField] private bool DontApproachToWalls = true;

	[Header("Attack settings")]
	[SerializeField] private Vector2 targetSwapFreq = new Vector2(0.1f, 0.5f);
	[SerializeField] private Vector2 factorWaitTime = new Vector2(1.1f, 1.5f);
	[SerializeField] private float aimTime = 0.4f;

	private IEnumerator moveCoroutine;
	private IEnumerator aimCoroutine;
	private IEnumerator attackCoroutine;

	private void Start()
	{
		mortal = GetComponent<MortalEntity>();
		fighter = GetComponent<Fighter>();
		movable = GetComponent<Movable>();
		vision = GetComponent<Vision>();

		moveCoroutine = RunMovementCycle();
		aimCoroutine = RunAimCycle();
		attackCoroutine = RunFireCycle();

		StartCoroutine(moveCoroutine);
		StartCoroutine(aimCoroutine);
		StartCoroutine(attackCoroutine);
	}

	private IEnumerator RunMovementCycle()
	{
		yield return new WaitForSeconds(0.1f);
		directionMoveInput = (MoveInDirection)movable.Input;

		Func<float, float> dirFactorWalls;
		Func<float, float> dirFactorOthers = dist => dist - vision.Visiblity * distanceFromOthers;

		if (DontApproachToWalls) dirFactorWalls = dist => Math.Min(0, dist - vision.Visiblity * distanceFromWalls);
		else dirFactorWalls = dist => dist - vision.Visiblity * distanceFromWalls;

		while (true)
		{
			Vector2 direction = new Vector2();
			Vector2 pos2d = (Vector2)transform.position;
			RaycastHit2D[] hits = vision.LookAround();

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.transform == null) continue;
				
				Vector2 hitDir = hit.point - pos2d;

				if (hit.transform.GetComponent<MortalEntity>() == null)
					direction += hitDir * dirFactorWalls(hit.distance);
				else
					direction += hitDir * dirFactorOthers(hit.distance);
			}

			directionMoveInput.direction = direction;

			yield return new WaitForSeconds(
				UnityEngine.Random.Range(rangeTimeToMove.x, rangeTimeToMove.y));

			// directionMoveInput.direction = Vector2.zero;

			// yield return new WaitForSeconds(
			// 	UnityEngine.Random.Range(rangeTimeToStay.x, rangeTimeToStay.y));
		}
	}

	private IEnumerator RunAimCycle()
	{
		yield return new WaitForSeconds(0.1f);

		fighterInput = (StrikeToTarget)fighter.Input;

		while (true)
		{
			RaycastHit2D[] hits = vision.LookAround();
			RaycastHit2D closest = new RaycastHit2D();

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.transform == null) continue; //не хочу длинных скобок
				if (hit.transform.GetComponent<MortalEntity>() == null) continue; // по неживим не бьем
				//if (hit.transform.CompareTag(gameObject.tag)) continue; //по своим не бьем
				
				if (hit.distance > closest.distance)
					closest = hit;
			}

			if (closest.transform != null)
				fighterInput.target = closest.transform;

			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator RunFireCycle()
	{
		fighterInput = (StrikeToTarget)fighter.Input;

		float cooldown = fighter.weapon.Cooldown;

		while (true)
		{
			yield return new WaitForSeconds(0.1f);

			if (fighterInput.target == null) continue;

			yield return new WaitForSeconds(
				cooldown * UnityEngine.Random.Range(factorWaitTime.x, factorWaitTime.y));

			RaycastHit2D[] barriers;

			try
			{
				directionMoveInput.direction = Vector2.zero;

				barriers = Physics2D.RaycastAll(
						(Vector2)transform.position,
						fighterInput.target.position - transform.position,
						Vector2.Distance(fighterInput.target.position, transform.position));

				StopCoroutine(moveCoroutine);
			}
			catch { continue; }

			if (barriers.Length <= 2) // одно - коллайдер снайпера, второе - цели
			{
				yield return new WaitForSeconds(aimTime);
				fighterInput.SetReady();
			}

			StartCoroutine(moveCoroutine);
		}
	}
}