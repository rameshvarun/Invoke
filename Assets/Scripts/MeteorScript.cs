using UnityEngine;
using System.Collections;

public class MeteorScript : MonoBehaviour {
	private Vector3 targetPosition;
	public Vector3 displacement;
	public float speed;

	public float stopEmitTime;
	public float destroyTime;

	private CameraControl cameraControl;

	public bool hit;

	// Use this for initialization
	void Start () {
		targetPosition = transform.position;
		transform.position += displacement;

		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > targetPosition.y) {
			Vector3 direction = (targetPosition - transform.position).normalized*speed*Time.deltaTime;
			transform.position += direction;
		}
		else {

			if(!hit) {
				hit = true;
				transform.position = targetPosition;
				cameraControl.shakeTime = 1.5f;
			}

			GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
			for(int i = 0; i < buildings.Length; ++i) {
				if(Vector3.Distance(buildings[i].transform.position, transform.position) < 0.5) {
					buildings[i].GetComponent<BuildingScript>().DestroyBuilding();
				}
			}



			stopEmitTime -= Time.deltaTime;
			destroyTime -= Time.deltaTime;
			if(stopEmitTime < 0.0f){
				transform.FindChild("Flame/InnerCore").particleEmitter.emit = false;
				transform.FindChild("Flame/OuterCore").particleEmitter.emit = false;
				transform.FindChild("Flame/Smoke").particleEmitter.emit = false;
				transform.FindChild("Flame/Lightsource").light.enabled = false;
				transform.position -= new Vector3(0,0.3f,0)*Time.deltaTime;
			}

			if(destroyTime < 0.0f) {
				Destroy(this.gameObject);
			}
		}
	}
}
