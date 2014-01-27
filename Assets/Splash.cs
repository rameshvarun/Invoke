using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	private float splashTime = 4.0f;
	// Use this for initialization
	void Start () {
		splashTime = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
		splashTime -= Time.deltaTime;
		if(splashTime < 0)
			Application.LoadLevel(1);
	}
}
