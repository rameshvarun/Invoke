using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBuilder : MonoBehaviour {

	public float buildInterval;
	private float currentTime;

	public enum Action {
		Wall,
		Hut,
		WaterTower,
		EngineeringStation,
		Clinic,
		Granary
	}

	private Dictionary<Action, int> defaultAffinities;
	public Dictionary<Action, int> affinities;

	private GridLoader gridLoader;

	public Transform[] huts;
	public Transform waterTower;
	public Transform wallTower;

	public Transform granary;
	public Transform clinic;
	public Transform engineeringStation;

	public GameObject getBuildingAt(int x, int y) {

		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

		for(int i = 0; i < buildings.Length; ++i) {
			if(buildings[i].GetComponent<BuildingScript>().x == x && buildings[i].GetComponent<BuildingScript>().y == y) {
				return buildings[i];
			}
		}

		return null;
	}

	public void Shuffle<BuildingScript>(List<BuildingScript> list)  
	{  

		System.Random rng = new System.Random();  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			BuildingScript value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	public List<BuildingScript> randomBuildings() {
		List<BuildingScript> list = new List<BuildingScript>();
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
		for(int i = 0; i < buildings.Length; ++i) {
			list.Add(buildings[i].GetComponent<BuildingScript>());
		}
		Shuffle(list);
		return list;
	}

	public ArrayList getBuildingsByType(string buildingType) {
		ArrayList buildingsList = new ArrayList();

		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
		for(int i = 0; i < buildings.Length; ++i) {
			if(buildings[i].GetComponent<BuildingScript>().buildingType == buildingType) {
				buildingsList.Add(buildings[i]);
			}
		}

		return buildingsList;
	}
	
	/*Vector3 getCenter() {
		Vector3 sum = Vector3.zero;
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
		for(int i = 0; i < buildings.Length; ++i) {
			sum += buildings[i].transform.position;
		}
		sum /= (float)buildings.Length;
		return sum;
	}
	float getRadius() {
		Vector3 center = getCenter();

		float radius = 0.0f;
		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
		for(int i = 0; i < buildings.Length; ++i) {
			radius = Mathf.Max(radius, Vector3.Distance(center, buildings[i].transform.position) );
		}

		return radius;
	}*/

	void BuildWall() {
		List<int> xpositions = new List<int>();
		List<int> ypositions = new List<int>();
		for(int x = 0; x < gridLoader.mapWidth; ++x) {
			for(int y = 0; y < gridLoader.mapHeight; ++y) {
				if(gridLoader.map[x,y] > 0 && getBuildingAt(x, y) == null) {
					//Check if it borders water
					if(gridLoader.map[x + 1,y] == 0 || gridLoader.map[x - 1,y] == 0 || gridLoader.map[x,y - 1] == 0 || gridLoader.map[x,y + 1] == 0) {
						xpositions.Add(x);
						ypositions.Add(y);
						//Vector3 position = ;
						//positions.Add();
						//
						//
						//
						//return;
					}
				}
			}
		}

		if(xpositions.Count > 0) {
			int index = Random.Range(0, xpositions.Count);
			int xPos = xpositions[index];
			int yPos = ypositions[index];

			Transform gameObject = (Transform)Instantiate(wallTower, new Vector3(xPos*gridLoader.tileSize,0,yPos*gridLoader.tileSize), Quaternion.identity);
			gameObject.GetComponent<BuildingScript>().x = xPos;
			gameObject.GetComponent<BuildingScript>().y = yPos;
		}
	}

	void Build(Transform building) {

		foreach(BuildingScript randomBuilding in randomBuildings()) {
			int randomX = Random.Range(-1,2) + randomBuilding.x;
			int randomY = Random.Range(-1,2) + randomBuilding.y;

			if( randomBuilding.buildingType != "wall" && getBuildingAt(randomX, randomY) == null) {

				Vector3 position = new Vector3(randomX*gridLoader.tileSize,0,randomY*gridLoader.tileSize);

				Transform gameObject = (Transform)Instantiate(building, position, Quaternion.identity);
				gameObject.GetComponent<BuildingScript>().x = randomX;
				gameObject.GetComponent<BuildingScript>().y = randomY;

				if(gameObject.GetComponent<BuildingScript>().buildingType == "hut")
					gameObject.rotation = Quaternion.Euler(0, Random.value*360, 0);
				return;
			}
		}
	}

	// Use this for initialization
	void Start () {
		//Get refence to grid loading script
		gridLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridLoader>();

		currentTime = 0;

		//Set up default affinities
		defaultAffinities = new Dictionary<Action, int>();
		defaultAffinities[Action.Wall] = 0;
		defaultAffinities[Action.Hut] = 2;
		defaultAffinities[Action.WaterTower] = 0;
		defaultAffinities[Action.EngineeringStation] = 0;
		defaultAffinities[Action.Clinic] = 0;
		defaultAffinities[Action.Granary] = 1;

		//Copy into store for current affinities
		affinities = new Dictionary<Action, int>();
		foreach(KeyValuePair<Action,int> pair in defaultAffinities ) {
			affinities[pair.Key] = pair.Value;
		}

		//Instantiate first village
		Transform firstHut = (Transform)Instantiate(huts[Random.Range(0, huts.Length)], new Vector3((gridLoader.mapWidth/2)*gridLoader.tileSize,0,(gridLoader.mapHeight/2)*gridLoader.tileSize), Quaternion.identity);
		firstHut.GetComponent<BuildingScript>().x = gridLoader.mapWidth/2;
		firstHut.GetComponent<BuildingScript>().y = gridLoader.mapHeight/2;
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

		Debug.Log(action);

		if(action == Action.Hut) {
			Build ( huts[Random.Range(0, huts.Length)] );
		}
		if(action == Action.WaterTower) {
			Build ( waterTower );
		}
		if(action == Action.Wall) {
			BuildWall();
		}

		if(action == Action.Granary) {
			Build ( granary );
		}

		if(action == Action.Clinic) {
			Build ( clinic );
		}

		if(action == Action.EngineeringStation) {
			Build (engineeringStation );
		}

		if(affinities[action] > defaultAffinities[action]) {
			--affinities[action];
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
