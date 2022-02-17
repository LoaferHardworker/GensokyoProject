using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapElement
{
	public MapElement(Type type)
	{
		this.type = type;

		links = new HashSet<Vector2Int>();
	}

	public enum Type
	{
		Room,
		Corridor,
		Booked
	}

	public Type type;
	
	/// <summary>
	///	Все переходы в другие комнаты
	/// </summary>
	public HashSet<Vector2Int> links;
}