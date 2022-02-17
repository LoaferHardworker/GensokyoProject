using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public DungeonMap map;
	public GameObject room;

	private IEnumerator Start()
	{
		map.Generate();

		foreach(KeyValuePair<Vector2Int, MapElement> el in map.PointMap)
		{
			InstantiateNode(el.Key, el.Value.links, el.Value.type);
			yield return new WaitForSeconds(0f);
		}

		Debug.Log("Finished");
	}

	private void InstantiateNode(Vector2Int pos, HashSet<Vector2Int> links, MapElement.Type type)
	{
		if (type == MapElement.Type.Room)
		{
			Instantiate(room, (Vector2)pos + (Vector2)transform.position, Quaternion.identity, transform);
		}

		foreach (Vector2Int dir in links)
		{
			Debug.DrawRay((Vector2)pos + (Vector2)transform.position, (Vector2)dir, Color.green, float.PositiveInfinity);
		}
	}
}