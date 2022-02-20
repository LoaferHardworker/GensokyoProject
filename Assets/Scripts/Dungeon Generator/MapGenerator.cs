using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//класс который пока на 99% нужен для отладки
public class MapGenerator : MonoBehaviour
{
	public DungeonMap map;
	public GameObject room;

	private IEnumerator Start()
	{
		map = Instantiate(map);

		// На этом сиде будем тестить комнаты 
		// или на другом, но этот подходит, я его сам методом тыка подобрал)
		Random.InitState(3737385);

		map.Generate();

		foreach(KeyValuePair<Vector2Int, MapElement> el in map.PointMap)
		{
			InstantiateNode(el.Key, el.Value.links, el.Value.type);
			yield return 0;
		}
	}

	private void InstantiateNode(Vector2Int pos, HashSet<Vector2Int> links, MapElement.Type type)
	{
		if (type == MapElement.Type.Room)
		{
			GameObject newRoom = Instantiate(room, (Vector2)pos + (Vector2)transform.position, Quaternion.identity, transform);
			newRoom.transform.localScale = new Vector3(1f, 1f, 1f) * map.PointMap[pos].size * 0.8f;
		}

		foreach (Vector2Int dir in links)
		{
			Debug.DrawRay((Vector2)pos + (Vector2)transform.position, (Vector2)dir, Color.green, float.PositiveInfinity);
		}
	}
}