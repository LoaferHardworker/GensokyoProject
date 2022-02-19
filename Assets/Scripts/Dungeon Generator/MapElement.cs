using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapElement
{
	public MapElement(Type type, int size = 1)
	{
		this.type = type;
		this.size = size;

		links = new HashSet<Vector2Int>();
	}

	public enum Type
	{
		Room,
		Corridor,
		Booked
	}

	public Type type;
	public int size;
	
	/// <summary>
	///	Все переходы в другие комнаты
	/// </summary>
	public HashSet<Vector2Int> links;
}