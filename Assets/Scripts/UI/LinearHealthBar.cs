using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearHealthBar : MonoBehaviour
{
	[SerializeField] private MortalEntity mortal;
	private RectTransform rectTransform;

	private void Start()
	{
		mortal.OnHealthChange.AddListener(UpdateHealthBar);
		rectTransform = transform as RectTransform;

		UpdateHealthBar(0);
	}

	private void OnDestroy()
	{
		mortal.OnHealthChange.RemoveListener(UpdateHealthBar);
	}

	private void UpdateHealthBar(float difference)
	{
		rectTransform.localScale = new Vector3(mortal.CurrentHealth / mortal.MaxHealth, 1f, 1f);
	}
}