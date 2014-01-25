using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBuilder : MonoBehaviour {

	public float buildInterval;
	private float currentTime;

	enum Action {
		Wall,
		Huts
	}

	private Dictionary<Action, int> affinities;

	private GridLoader gridLoader;

	public Transform[] huts;

	GameObject getBuildingAt(int x, int y) {
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

		for(int i = 0; i < buildings.Length; ++i) {
			if(buildings[i].GetComponent<BuildingScript>().x == x && buildings[i].GetComponent<BuildingScript>().y == y) {
				return buildings[i];
			}
		}

		return null;
	}

	// Use this for initialization
	void Start () {
		//Get refence to grid loading script
		gridLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridLoader>();

		currentTime = 0;

		//Set up initial affinities
		affinities = new Dictionary<Action, int>();
		affinities[Action.Wall] = 0;
		affinities[Action.Huts] = 1;

		//Instantiate first village
		Instantiate(huts[Random.Range(0, huts.Length)], new Vector3((gridLoader.mapWidth/2)*gridLoader.tileSize,0,(gridLoader.mapHeight/2)*gridLoader.tileSize), Quaternion.identity);
	}

	void Build() {
		int affinitySum = 0;
		foreach(KeyValuePair<Action,int> pair in affinities ) {
			affinitySum += pair.Value;
		}

		int random = Random.Range(0, affinitySum);

		Action action = Action.Wall;
		foreach(KeyValuePair<Action,int> pair in affinities ) {
			random -= pair.Value;
			if(random < 0) {
				action = pair.Key;
				break;
			}
		}

		if(action == Action.Huts) {
			GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
			while(true) {
				BuildingScript randomBuilding = buildings[Random.Range(0, buildings.Length)].GetComponent<BuildingScript>();
				int randomX = Random.Range(-1,2) + randomBuilding.x;
				int randomY = Random.Range(-1,2) + randomBuilding.y;

				Debug.Log(randomX + " " + randomY);

				if( getBuildingAt(randomX, randomY) == null) {
					Instantiate(huts[Random.Range(0, huts.Length)], new Vector3(randomX*gridLoader.tileSize,0,randomY*gridLoader.tileSize), Quaternion.identity);
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		if(currentTime > buildInterval) {
			currentTime = 0;
			Build ();
		}
	}
}
