using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

	private Vector3 targetPosition;

	//private GridLoader gridLoader;
	public int x;
	public int y;

	public string buildingType;

	private bool destroying;
	private float smokingTime;

	public Transform destructionSmoke;

	// Use this for initialization
	void Start () {

		targetPosition = transform.position;
		transform.position -= new Vector3(0,2,0);

		destroying = false;

		GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
		for(int i = 0; i < trees.Length; ++i) {
			if(Vector3.Distance(targetPosition, trees[i].transform.position) < 0.8f) {
				Destroy( trees[i] );
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(destroying) {
			smokingTime -= Time.deltaTime;
			if(smokingTime < 0.0f) {
				if(transform.position.y > targetPosition.y) {
					transform.position += Vector3.down * 0.1f*Time.deltaTime;
				}
				else{
					Destroy(this.gameObject);
				}
			}
		}
		else {
			transform.position = Vector3.Lerp(transform.position, targetPosition, 2*Time.deltaTime);
		}
	}

	public void DestroyBuilding() {
		if(!destroying) {
			targetPosition = transform.position - new Vector3(0,0.5f,0);
			destroying = true;
			Instantiate(destructionSmoke, transform.position + new Vector3(0,0,0), transform.rotation);
			smokingTime = 1.0f;
		}
	}

	public bool isDestroyed() {
		return destroying;
	}
}
