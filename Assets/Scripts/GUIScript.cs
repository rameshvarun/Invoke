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


	private SimulationScript simulationScript;

	public GUISkin guiSkin;

	void Start() {

		simulationScript = GetComponent<SimulationScript>();
	
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
		populationText.guiText.text = "Population: " + (int)simulationScript.population;
		foodText.guiText.text = "Food: " + (int)simulationScript.food;
		manaText.guiText.text = "Mana: " + (int)simulationScript.mana;
		happinessText.guiText.text = "Suffering: " + (100 -(int)simulationScript.happiness) + "%";
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
