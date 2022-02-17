using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Generator Settings")]
public class DungeonMap : ScriptableObject
{
	public int mainBranchLen = 10;
	public int otherBranches = 3;
	public Vector2Int otherBranchesLenRange = new Vector2Int(2, 4);
	[Range(0f, 0.8f)] public float longCorridorChance = 0.2f;

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

		for (int generated = 0, j = 0; generated < otherBranches && j < 100; j++)
		{
			n1 = mainBranch[Random.Range(1, mainBranch.Count)];
			int countOfRooms = Random.Range(otherBranchesLenRange.x, otherBranchesLenRange.y);

			if (GenerateBranch(n1, countOfRooms))
				generated++;
		}
	}

	private bool GenerateBranch(Vector2Int n1, int count)
	{
		List<Vector2Int> aveliableDirs = new List<Vector2Int>(passDirs);

		for (int generated = 0; generated < count;)
		{
			//где будет комната
			Vector2Int n2 = n1 + aveliableDirs[Random.Range(0, aveliableDirs.Count)];

			if (PointMap.ContainsKey(n2) || // а вдруг мы не можем в этом месте сделать комнату
				(PointMap.ContainsKey(new Vector2Int(n2.x, n1.y)) && (n2 - n1).x != 0 && (n2 - n1).y != 0) ||
				(PointMap.ContainsKey(new Vector2Int(n1.x, n2.y)) && (n2 - n1).x != 0 && (n2 - n1).y != 0)) //чтоб диагонали не скрещивались
			{
				aveliableDirs.Remove(n2 - n1);

				if (aveliableDirs.Count == 0)
					return false; 

				continue;
			}

			
			//добавляем комнаты в список
			PointMap.Add(n2, new MapElement(DetermineRoomType(n1, n2)));

			PointMap[n1].links.Add(n2 - n1);
			PointMap[n2].links.Add(n1 - n2);

			if (PointMap[n2].type != MapElement.Type.Corridor)
				generated++;

			n1 = n2;

			//обновляем доступные направления спавна комнат
			//такая странная процедуранужна, чтоб ветви были +- прямые
			//некоторое время и не заходили в тупики слишком часто
			if (aveliableDirs.Count == 1 && Random.value < 0.5) //ПОФИКСИТЬ ХАРДКОд
				aveliableDirs = new List<Vector2Int>(passDirs);
		}

		return true;
	}

	private MapElement.Type DetermineRoomType(Vector2Int n1, Vector2Int n2)
	{
		if (PointMap[n1].type != MapElement.Type.Corridor)
			return MapElement.Type.Corridor;

		else if (Random.value < longCorridorChance)
			return MapElement.Type.Corridor;

		return MapElement.Type.Room;
	}
}