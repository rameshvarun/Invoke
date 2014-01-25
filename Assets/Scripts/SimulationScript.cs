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

	private AIBuilder aiBuilder;

	// Use this for initialization
	void Start () {
		populationTimer = populationCycleTime;
		population = 20.0f;

		food = 50.0f;

		happiness = 50.0f;
		happinessTimer = happinessCycleTime;

		aiBuilder = GetComponent<AIBuilder>();
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
			//Food cannot be less than zero
			if(food < 0.0f)
				food = 0.0f;
		}

		happinessTimer -= Time.deltaTime;
		if(happinessTimer < 0) {
			//Reset happiness update timer
			happinessTimer = happinessCycleTime;

			//Update happiness based off of population and food
			happiness += ((food/population) - 1.0f)*3.0f;

			//Clamp happiness between 1 and 100
			happiness = Mathf.Clamp(happiness, 0.0f, 100.0f);
		}
	}
}
