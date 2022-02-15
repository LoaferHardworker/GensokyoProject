using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player player { get; private set; }
	public static MortalEntity mortal  { get; private set; }
	public static Fighter fighter { get; private set; }
	public static Movable movable { get; private set; }

	private void Start()
	{
		if (player == null) player = this;
		else if (player != this) Destroy(this);

		mortal = GetComponent<MortalEntity>();
		fighter = GetComponent<Fighter>();
		movable = GetComponent<Movable>();
	}
}