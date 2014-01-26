using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	
	public Transform meteorFrame;
	public Transform rainFrame;
	public Transform quakeFrame;
	public Transform diseaseFrame;
	public Transform fireFrame;

	public Transform populationText;
	public Transform foodText;
	public Transform manaText;
	public Transform happinessText;

	public Transform[] buildingCounts;


	private SimulationScript simulationScript;
	private AIBuilder aiBuilder;

	public GUISkin guiSkin;

	void Start() {

		simulationScript = GetComponent<SimulationScript>();
		aiBuilder = GetComponent<AIBuilder>();
	
	}

	float GUIx( int x ) {
		return ((float)(x))/Screen.width;
	}

	float GUIy( int y ) {
		return ((float)(y))/Screen.height;
	}


	public bool mouseInUI() {
		GameObject[] guiObjects = GameObject.FindGameObjectsWithTag("GUI");
		for(int i = 0; i < guiObjects.Length; ++i) {
			if(guiObjects[i].GetComponent<ActionScript>().getRect().Contains(Input.mousePosition))
				return true;
		}
		return false;
	}

	void Update () {
		meteorFrame.guiTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100),GUIy( 20 ),1);
		rainFrame.guiTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100 - 200 - 20),GUIy( 20 ),1);
		quakeFrame.guiTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100 + 200 + 20),GUIy( 20 ),1);
		diseaseFrame.guiTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100 + 400 + 40),GUIy( 20 ),1);
		fireFrame.guiTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100 - 400 - 40),GUIy( 20 ),1);

		//Update GUI Text
		populationText.guiText.text = "Population: " + (int)simulationScript.population + "/" + (int)aiBuilder.getBuildingsByType("hut").Count*20;
		foodText.guiText.text = "Food: " + (int)simulationScript.food + "/" + (int)(50 + aiBuilder.getBuildingsByType("granary").Count*50);
		manaText.guiText.text = "Mana: " + (int)simulationScript.mana;
		happinessText.guiText.text = "Suffering: " + (100 -(int)simulationScript.happiness) + "%";

		//Update building counts
		buildingCounts[0].guiText.text = "Huts: " + aiBuilder.getBuildingsByType("hut").Count;
		buildingCounts[1].guiText.text = "Granaries: " + aiBuilder.getBuildingsByType("granary").Count;
		buildingCounts[2].guiText.text = "Wells: " + aiBuilder.getBuildingsByType("well").Count;
		buildingCounts[3].guiText.text = "Engineering Yards: " + aiBuilder.getBuildingsByType("engineering").Count;
		buildingCounts[4].guiText.text = "Clinics: " + aiBuilder.getBuildingsByType("clinic").Count;
		buildingCounts[5].guiText.text = "Walls: " + aiBuilder.getBuildingsByType("wall").Count;
	}

	void OnGUI() {

		GUI.skin = guiSkin;

		GameObject[] guiObjects = GameObject.FindGameObjectsWithTag("GUI");
		for(int i = 0; i < guiObjects.Length; ++i) {
			if(guiObjects[i].GetComponent<ActionScript>().getRect().Contains(Input.mousePosition)) {
				GUI.Box(new Rect (Input.mousePosition.x,Screen.height - Input.mousePosition.y,300,200), guiObjects[i].GetComponent<ActionScript>().tooltip);
			}
		}
	}
}
