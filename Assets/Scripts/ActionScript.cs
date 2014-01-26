using UnityEngine;
using System.Collections;

public class ActionScript : MonoBehaviour {

	public string action;
	public int level;
	public int mana;

	private SimulationScript simulationScript;
	private CameraControl cameraControl;

	public string tooltip;

	public Rect getRect() {
		return guiTexture.GetScreenRect();
	}

	// Use this for initialization
	void Start () {
		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();

		this.guiTexture.enabled = false;
		simulationScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<SimulationScript>();
	}
	
	// Update is called once per frame
	void Update () {

		if(guiTexture.GetScreenRect().Contains(Input.mousePosition)) {
			if(Input.GetMouseButtonUp(0)) {
				if(simulationScript.mana >= mana) {
					simulationScript.mana -= mana;

					if(action == "quake") {
						simulationScript.Quake(level);
					}
					if(action == "water") {
						simulationScript.Water(level);
					}
					if(action == "meteor") {
						cameraControl.mode = "meteor";
					}
					if(action == "fire") {
						simulationScript.Fire(level);
					}
					if(action == "disease") {
						simulationScript.Disease(level);
					}
				}
				else {
					GameObject.Find("InsufficientMana").guiText.color = Color.white;
				}
			}
		}
	}
}
