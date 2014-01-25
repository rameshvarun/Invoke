using UnityEngine;
using System.Collections;

public class GridLoader : MonoBehaviour {
	public Transform[] tiles;

	public int[,] map;

	public int mapWidth;
	public int mapHeight;

	public float tileSize;

	public int evolutionPasses;
	public int waterMargin;

	int CountNeigbors(int x, int y) {
		int count = 0;

		for(int i = Mathf.Max(x - 1, 0); i < Mathf.Min(x + 2, mapWidth); ++i) {
			for(int j = Mathf.Max(y - 1, 0); j < Mathf.Min(y + 2, mapHeight); ++j) {
				if(i != j && map[i,j] > 0) {
						++count;
				}
			}
		}
		return count;
	}

	// Use this for initialization
	void Start () {
		map = new int[mapWidth, mapHeight];

		/*for(int x = waterMargin; x < map.GetLength(0) - waterMargin; ++x) {
			for(int y = waterMargin; y < map.GetLength(1) - waterMargin; ++y) {
				if(Random.value > 0.9)
					map[x,y] = 1;
			}
		}*/

		//Seed in one island in center
		map[Random.Range(waterMargin, mapWidth - waterMargin), Random.Range(waterMargin, mapHeight - waterMargin)] = 1;
		//map[mapWidth/2, mapHeight/2] = 1;

		//Evolve land over passes by growing islands
		for(int i = 0; i < evolutionPasses; ++i) {
			for(int x = waterMargin; x < mapWidth - waterMargin; ++x) {
				for(int y = waterMargin; y < mapHeight - waterMargin; ++y) {
					if( map[x,y] == 0) {
						if( Random.value < CountNeigbors(x, y)*0.25  ) {
							map[x,y] = 1;
						}
					}
				}
			}
		}

		for(int x = 0; x < map.GetLength(0); ++x) {
			for(int y = 0; y < map.GetLength(1); ++y) {
				int type = map[x,y];
				Instantiate(tiles[type], new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
