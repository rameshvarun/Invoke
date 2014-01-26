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

	public Transform[] trees;

	int[,] MapCopy(int[,] array) {
		int[,] newArray = new int[array.GetLength(0),array.GetLength(1)];

		for(int i = 0; i < array.GetLength(0); ++i) {
			for(int j = 0; j < array.GetLength(1); ++j) {
				newArray[i,j] = array[i,j];
			}
		}

		return newArray;
	}

	int CountNeigbors(int x, int y, int steps) {
		int count = 0;

		for(int i = x - steps; i < x + steps + 1; ++i) {
			for(int j = y - steps; j < y + steps + 1; ++j) {
				if(i >= 0 && j >= 0 && i < mapWidth && j < mapHeight) {
					if( !(i == 0 && j == 0)) {
						if( map[i,j] > 0) {
								++count;
						}
					}
				}
			}
		}
		return count;
	}

	// Use this for initialization
	void Start () {
		map = new int[mapWidth, mapHeight];

		//Seed in one island in center
		map[mapWidth/2, mapHeight/2] = 1;

		//Evolve land over passes by growing islands
		for(int i = 0; i < evolutionPasses; ++i) {
			int[,] newMap = MapCopy(map);
			for(int x = waterMargin; x < mapWidth - waterMargin; ++x) {
				for(int y = waterMargin; y < mapHeight - waterMargin; ++y) {
					if( map[x,y] == 0) {
						if( Random.value < CountNeigbors(x, y, 1)*0.1f  ) {
							newMap[x,y] = 1;
						}
					}
				}
			}
			map = newMap;
		}


		//Plant Grass
		for(int x = 0; x < mapWidth; ++x) {
			for(int y = 0; y < mapHeight; ++y) {
				if( CountNeigbors(x, y, 2) > 24 && Random.value > 0.1  ) {
					map[x,y] = 2;
				}
			}
		}

		Vector3 center = new Vector3((mapWidth/2)*tileSize,0,(mapHeight/2)*tileSize);

		//Instantiate base water
		Object waterObject = Instantiate(tiles[0], center + new Vector3(0,-0.1f,0), Quaternion.identity);
		((Transform)waterObject).localScale = Vector3.one * mapWidth * 2;

		//Instantiate tiles from prefabs list
		for(int x = 0; x < map.GetLength(0); ++x) {
			for(int y = 0; y < map.GetLength(1); ++y) {
				int type = map[x,y];
				if(type > 0) {
					Instantiate(tiles[type], new Vector3(x*tileSize,0,y*tileSize), Quaternion.identity);
				}
			}
		}

		//Place trees
		for(int x = 0; x < map.GetLength(0); ++x) {
			for(int y = 0; y < map.GetLength(1); ++y) {
				int type = map[x,y];
				if(type > 0 && Random.value > 0.8f) {

					Transform tree = trees[Random.Range(0,trees.Length)];
					Instantiate(tree, new Vector3(x*tileSize + Random.value - 0.5f,0,y*tileSize + Random.value - 0.5f), Quaternion.identity);
				}
			}
		}

		//Center camera
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().targetPos = center;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
