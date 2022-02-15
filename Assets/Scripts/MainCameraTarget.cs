using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraTarget : MonoBehaviour
{
	public float smooth = 5.0f;

	private Transform cmr;
	private Vector3 z_offset;

	private static MainCameraTarget instance;

	private void Start()
	{
		if (instance != null) Destroy(instance);
		instance = this;

		cmr = Camera.main.transform;
		z_offset = new Vector3(0, 0, cmr.position.z - transform.position.z);
	}

	private void Update()
	{
		cmr.position = Vector3.Lerp(cmr.position, transform.position + z_offset, Time.deltaTime * smooth);
	}
}