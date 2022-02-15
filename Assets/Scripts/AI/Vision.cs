using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
	[SerializeField][Range(1, 300)] private int countOfRays = 96;
	[SerializeField][Range(0f, 20f)] private float visiblity = 15f;

	private Vector2[] rayCastDirecions;
	private Vector2[] rayCastStartPoints;

	public float Visiblity { get => visiblity; }

	private void Start()
	{
		rayCastDirecions = new Vector2[countOfRays];
		rayCastStartPoints = new Vector2[countOfRays];

		Vector2 pos2d = (Vector2)transform.position;

		for (int i = 0; i < countOfRays; i++)
		{
			double angle = 2 * Math.PI * i / countOfRays;
			Vector2 direction = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));

			RaycastHit2D[] reverseHits =
				Physics2D.RaycastAll(pos2d + direction * visiblity, -direction, visiblity);

			rayCastStartPoints[i] = reverseHits[reverseHits.Length-1].point - pos2d + direction * 0.1f;
			rayCastDirecions[i] = direction;
		}
	}

	public RaycastHit2D[] LookAround()
	{
		RaycastHit2D[] hits = new RaycastHit2D[countOfRays];

		for (int i = 0; i < countOfRays; i++)
		{
			Vector2 pos2d = (Vector2)transform.position + rayCastStartPoints[i];
			RaycastHit2D hit = Physics2D.Raycast(pos2d, rayCastDirecions[i], visiblity);

			hits[i] = hit;

			if (hit.transform != null) Debug.DrawRay(pos2d, rayCastDirecions[i] * Vector2.Distance(hit.point, pos2d), Color.red);
			else Debug.DrawRay(pos2d, rayCastDirecions[i] * visiblity, Color.white);
		}

		return hits;
	}
}