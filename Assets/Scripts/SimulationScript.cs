using UnityEngine;
using System.Collections;

public class SimulationScript : MonoBehaviour {

	public float population;
	public float populationCycleTime;
	public float populationCycleChance;
	private float populationTimer;

	public float food;

	public float happiness;
	public float happinessCycleTime;
	private float happinessTimer;

	public float mana;

	private AIBuilder aiBuilder;
	private CameraControl cameraControl;
	private GridLoader gridLoader;

	public Transform meteor;

	public Transform wave;

	// Use this for initialization
	void Start () {
		populationTimer = populationCycleTime;
		population = 20.0f;

		food = 50.0f;

		happiness = 50.0f;
		happinessTimer = happinessCycleTime;

		aiBuilder = GetComponent<AIBuilder>();
		gridLoader = GetComponent<GridLoader>();
		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
	}

	public void Water(int level) {
		if(level == 1) {
			cameraControl.rainTime = 5.0f;
			food += 20;
		}
		if(level == 2) {
			aiBuilder.affinities[AIBuilder.Action.Wall] += 5;
			food += 100;
			happiness -= 20;

			//Target 2 random buildings
			GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
			int count = 0;
			while(count < Mathf.Min (2, buildings.Length)) {
				GameObject randomBuilding = buildings[Random.Range(0, buildings.Length)];

				if(randomBuilding.GetComponent<BuildingScript>().buildingType != "wall") {

					Vector3 direction = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
					direction.Normalize();

					Vector3 wavePosition = randomBuilding.transform.position - direction*25.0f;

					Transform gameObject = (Transform)Instantiate(wave, wavePosition, Quaternion.identity);
					gameObject.GetComponent<WaveScript>().direction = direction;
					++count;
				}
			}
		}
		if(level == 3) {

			//Target 8 random buildings
			GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
			int count = 0;
			while(count < Mathf.Min (8, buildings.Length)) {
				GameObject randomBuilding = buildings[Random.Range(0, buildings.Length)];
				
				if(randomBuilding.GetComponent<BuildingScript>().buildingType != "wall") {
					
					Vector3 direction = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);
					direction.Normalize();
					
					Vector3 wavePosition = randomBuilding.transform.position - direction*25.0f;
					
					Transform gameObject = (Transform)Instantiate(wave, wavePosition, Quaternion.identity);
					gameObject.GetComponent<WaveScript>().direction = direction;
					++count;
				}
			}

			aiBuilder.affinities[AIBuilder.Action.Wall] += 10;
			food += 500;
			happiness -= 100;
		}
	}

	public void Meteor(Vector3 position, int level) {
		Instantiate(meteor, position, Quaternion.identity);
	}

	public void Disease(int level) {
		if(level == 1) {
			population -= population*0.1f;
			happiness -= 20;
		}
		if(level == 2) {
			population -= population*0.15f;
			happiness -= 50;
		}
		if(level == 3) {
			population -= population*0.20f;
			happiness -= 100;
		}
	}
	
	public void Fire(int level) {
		if(level == 1) {
			food -= 5;
			happiness -= 10;
		}
		if(level == 2) {
			food -= 20;
			happiness -= 25;
			aiBuilder.affinities[AIBuilder.Action.WaterTower] += 2;
		}
		if(level == 3) {
			food -= 100;
			happiness -= 100;

			int count = 30;
			while(count > 0 ) {
				int x = Random.Range(0,gridLoader.mapWidth);
				int y = Random.Range(0,gridLoader.mapHeight);

				if(gridLoader.map[x, y] > 0) {
					Meteor(new Vector3(x*gridLoader.tileSize,0,y*gridLoader.tileSize), 1);
					--count;
				}
			}

			aiBuilder.affinities[AIBuilder.Action.WaterTower] += 4;
		}
	}

	public void Quake(int level) {

		int count = 0;

		if(level == 1) {
			cameraControl.shakeTime = 3.0f;
			count = 2;

			aiBuilder.affinities[AIBuilder.Action.EngineeringStation] += 1;
			happiness -= 15.0f;
		}
		if(level == 2) {
			cameraControl.shakeTime = 5.0f;
			count = 8;

			aiBuilder.affinities[AIBuilder.Action.EngineeringStation] += 2;
			happiness -= 50.0f;
		}
		if(level == 3) {
			cameraControl.shakeTime = 10.0f;
			count = 35;

			aiBuilder.affinities[AIBuilder.Action.EngineeringStation] += 4;
			happiness -= 100.0f;
		}

		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");

		count -= aiBuilder.getBuildingsByType("engineering").Count;
		count = Mathf.Clamp(count, 0, buildings.Length);

		while(count > 0) {
			BuildingScript randomBuilding = buildings[Random.Range(0, buildings.Length)].GetComponent<BuildingScript>();
			if(!randomBuilding.isDestroyed()){
				randomBuilding.DestroyBuilding();
				--count;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		populationTimer -= Time.deltaTime;
		if(populationTimer < 0) {
			//Reset population update timer
			populationTimer = populationCycleTime;

			//Chance that population formula will re-update
			if(Random.value < populationCycleChance) {
				population *= 1 +((food/population - 1)*0.5f);

				//Population is limited to 20*number of huts
				float populationMax = aiBuilder.getBuildingsByType("hut").Count*20;
				if( population >  populationMax)
					population = populationMax;
			}

			//Food reduces by 5% of population
			food -= population*0.05f;

			//Chance of sacrifice
			if(Random.value < (100.0f - happiness)/100.0f ) {
				mana += 1.0f;
			}
		}
		//Food cannot be less than zero, and is limited by the amount of granaries are owned
		float maxFood = 50 + aiBuilder.getBuildingsByType("granary").Count*50;
		food = Mathf.Clamp(food, 0, maxFood);

		happinessTimer -= Time.deltaTime;
		if(happinessTimer < 0) {
			//Reset happiness update timer
			happinessTimer = happinessCycleTime;

			//Update happiness based off of population and food
			happiness += ((food/population) - 1.0f)*3.0f;

			//Clamp happiness between 1 and 100
			happiness = Mathf.Clamp(happiness, 0.0f, 100.0f);
		}

		//Clamp mana
		mana = Mathf.Clamp(mana, 0, 10 + Mathf.Ceil( population / 50.0f ) );
	}
}
