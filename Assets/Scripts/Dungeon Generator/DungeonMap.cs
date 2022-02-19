using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Generator Settings")]
public class DungeonMap : ScriptableObject
{
	[SerializeField] protected int mainBranchLen = 10;
	[SerializeField] protected int otherBranches = 3;
	[SerializeField] protected Vector2Int otherBranchesLenRange = new Vector2Int(2, 4);
	[SerializeField] [Range(0f, 0.8f)] protected float longCorridorChance = 0.2f;
	[SerializeField] [Range(0f, 1f)] protected float turnChance = 0.5f;
	[SerializeField] [Range(0f, 1f)] protected float room3x3Chance = 0.5f;

	public Dictionary<Vector2Int, MapElement> PointMap { get; private set; }

	private readonly List<Vector2Int> passDirs = new List<Vector2Int>()
	{
		Vector2Int.up,
		Vector2Int.right,
		Vector2Int.down,
		Vector2Int.left,
		new Vector2Int(1, 1),
		new Vector2Int(-1, 1),
		new Vector2Int(1, -1),
		new Vector2Int(-1, -1)
	};

	public DungeonMap()
	{
		PointMap = new Dictionary<Vector2Int, MapElement>();
	}

	public void Generate()
	{
		PointMap.Clear();

		Vector2Int n1 = Vector2Int.zero;

		PointMap.Add(n1, new MapElement(MapElement.Type.Room));

		GenerateBranch(n1, mainBranchLen);

		var mainBranch = new List<Vector2Int>(PointMap.Keys);

		for (int generated = 0, j = 0; generated < otherBranches && j < 500; j++)
		{
			n1 = mainBranch[Random.Range(1, mainBranch.Count)];
			if (PointMap[n1].type == MapElement.Type.Booked) continue;
			int countOfRooms = Random.Range(otherBranchesLenRange.x, otherBranchesLenRange.y);

			if (GenerateBranch(n1, countOfRooms))
				generated++;
		}

		 var toRemove = new List<KeyValuePair<Vector2Int, MapElement>> ();

		foreach(KeyValuePair<Vector2Int, MapElement> el in PointMap)
			if (el.Value.type == MapElement.Type.Booked)
				toRemove.Add(el);
		
		foreach(KeyValuePair<Vector2Int, MapElement> el in toRemove)
			PointMap.Remove(el.Key);
	}

	private bool GenerateBranch(Vector2Int n1, int count)
	{
		List<Vector2Int> aveliableDirs = new List<Vector2Int>(passDirs);

		for (int generated = 0; generated < count;)
		{
			MapElement.Type type = DetermineRoomType(n1);
			int size = 1;

			if (type == MapElement.Type.Room && Random.value < room3x3Chance)
				size = 3;

			Vector2Int dir = aveliableDirs[Random.Range(0, aveliableDirs.Count)];
			Vector2Int n2 = n1+ dir * (PointMap[n1].size / 2) + dir * (1 + size / 2);

			if (!IsAveliablePass(n1, dir, size))
			{
				if (size > 1)
					continue;

				aveliableDirs.Remove(dir);
				if (aveliableDirs.Count == 0) return false; 
				continue;
			}
			
			PointMap.Add(n2, new MapElement(type, size));

			PointMap[n1].links.Add(dir);
			PointMap[n2].links.Add(-dir);

			for (int i = -size / 2; i <= size / 2; i++)
				for (int j = -size / 2; j <= size / 2; j++)
				{
					if (i == 0 && j == 0) continue;
					PointMap.Add(n2 + new Vector2Int(i, j), new MapElement(MapElement.Type.Booked));
				}

			if (PointMap[n2].type != MapElement.Type.Corridor)
				generated++;

			n1 = n2;

			if (aveliableDirs.Count == 1 && Random.value < turnChance)
				aveliableDirs = new List<Vector2Int>(passDirs);
		}

		return true;
	}

	private MapElement.Type DetermineRoomType(Vector2Int from)
	{
		if (PointMap[from].type != MapElement.Type.Corridor)
			return MapElement.Type.Corridor;

		else if (Random.value < longCorridorChance)
			return MapElement.Type.Corridor;

		return MapElement.Type.Room;
	}

	private bool IsAveliablePass(Vector2Int n1, Vector2Int dir, int size)
	{
		Vector2Int n2 = n1+ dir * (PointMap[n1].size / 2) + dir * (1 + size / 2);
		if (PointMap.ContainsKey(n2) || // а вдруг мы не можем в этом месте сделать комнату
			(PointMap.ContainsKey(new Vector2Int(n2.x, n1.y)) && dir.x != 0 && dir.y != 0) ||
			(PointMap.ContainsKey(new Vector2Int(n1.x, n2.y)) && dir.x != 0 && dir.y != 0)) //чтоб диагонали не скрещивались
			return false;

		for (int i = -size / 2; i <= size / 2; i++)
			for (int j = -size / 2; j <= size / 2; j++)
				if (PointMap.ContainsKey(n2 + new Vector2Int(i, j)))
					return false;

		return true;
	}
}