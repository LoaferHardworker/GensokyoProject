using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithTime : MonoBehaviour
{
	public float Delay = 5f;

	private void Start()
	{
		StartCoroutine(WaitAndDestroy(Delay));
	}

	private IEnumerator WaitAndDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}