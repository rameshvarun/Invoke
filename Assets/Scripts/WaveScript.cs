using UnityEngine;
using System.Collections;

public class WaveScript : MonoBehaviour {

	public float speed;
	public Vector3 direction;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(speed*Time.deltaTime*direction);

		GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
		for(int i = 0; i < buildings.Length; ++i) {
			if(Vector3.Distance(buildings[i].transform.position, transform.position) < 0.5) {
				if(buildings[i].GetComponent<BuildingScript>().buildingType != "wall") {
					buildings[i].GetComponent<BuildingScript>().DestroyBuilding();
				}
				Destroy(this.gameObject);
			}
		}
	}
}
