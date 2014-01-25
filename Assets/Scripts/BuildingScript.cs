using UnityEngine;
using System.Collections;

public class BuildingScript : MonoBehaviour {

	private Vector3 targetPosition;

	private GridLoader gridLoader;
	public int x;
	public int y;

	public string buildingType;

	// Use this for initialization
	void Start () {
		gridLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridLoader>();

		x = (int)(transform.position.x / gridLoader.tileSize);
		y = (int)(transform.position.z / gridLoader.tileSize);

		targetPosition = transform.position;
		transform.position -= new Vector3(0,2,0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, targetPosition, 2*Time.deltaTime);
	}
}
