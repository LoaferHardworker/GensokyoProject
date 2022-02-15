using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Manager { get; private set; }

	private void Awake()
	{
		if (Manager != null)
		{
			Manager = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}

		
	}
}