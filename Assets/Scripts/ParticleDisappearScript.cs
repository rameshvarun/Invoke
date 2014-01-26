using UnityEngine;
using System.Collections;

public class ParticleDisappearScript : MonoBehaviour {

	public float stopTime;
	public float destroyTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		stopTime -= Time.deltaTime;
		destroyTime -= Time.deltaTime;

		if(stopTime < 0) {
			this.particleEmitter.emit = false;
		}
		if(destroyTime < 0) {
			Destroy(this.gameObject);
		}
	}
}
