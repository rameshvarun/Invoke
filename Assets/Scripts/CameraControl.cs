using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Vector3 targetPos;
	public Vector3 displacement;

	public float moveSpeed;

	public float mouseMargin = 100;

	private Vector3 right;
	private Vector3 upward;

	public float scrollIncrement;
	public float targetScroll;
	public float scrollSpeed;

	public Vector2 scrollBoundaries;

	private Camera cameraComponent;

	private GridLoader gridLoader;

	private Plane groundPlane;

	public string mode;

	public Transform meteorSelect;

	public float shakeTime;

	public Transform rainSystem;

	public float rainTime;
	public float bloomTime;

	private GUIScript guiScript;
	private SimulationScript simulationScript;

	// Use this for initialization
	void Start () {
		gridLoader = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridLoader>();
		guiScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GUIScript>();
		cameraComponent = this.GetComponent<Camera>();
		simulationScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<SimulationScript>();

		//This determines how the mouse direction is mapped to movements in the 3d space
		right = new Vector3(1,0,-1).normalized;
		upward = new Vector3(1,0,1).normalized;

		groundPlane = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));

		mode = "panning";
		meteorSelect.renderer.enabled = false;

		shakeTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		//Touch scrolling
		//Multi-touch scroll
		if(Input.touchCount == 2 ) {
			Vector2 touchDifference = Input.GetTouch(0).position - Input.GetTouch(1).position;
			Vector2 previousTouchDifference = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
			float scale = touchDifference.magnitude / previousTouchDifference.magnitude;
			
			targetScroll /= scale*scale*scale;
		}
		#else
		//Mouse scrolling
		targetScroll -= scrollIncrement*Input.GetAxis("Mouse ScrollWheel");
		#endif

		//Clamp scroll to max/min bounds
		targetScroll = Mathf.Clamp(targetScroll, scrollBoundaries.x, scrollBoundaries.y);

		cameraComponent.orthographicSize = Mathf.Lerp(cameraComponent.orthographicSize, targetScroll, scrollSpeed*Time.deltaTime);

		//Camera shaking
		if(shakeTime > 0) {
			shakeTime -= Time.deltaTime;
			transform.position = targetPos + displacement + Random.onUnitSphere*shakeTime*0.1f;
			transform.LookAt(targetPos + Random.onUnitSphere*shakeTime*0.1f );

			GetComponent<Vignetting>().chromaticAberration = shakeTime*5.0f;

			if(!quakeSound.audio.isPlaying)
				quakeSound.audio.Play();
		}
		else {
			transform.position = targetPos + displacement;
			transform.LookAt(targetPos);
			GetComponent<Vignetting>().chromaticAberration = 0;

			if(quakeSound.audio.isPlaying)
				quakeSound.audio.Stop();
		}

		//Drought bloom
		if(bloomTime > 0) {
			bloomTime -= Time.deltaTime;
			GetComponent<Bloom>().enabled = true;

			if(!sizzleSound.audio.isPlaying)
				sizzleSound.audio.Play();
		}
		else {
			GetComponent<Bloom>().enabled = false;

			if(sizzleSound.audio.isPlaying)
				sizzleSound.audio.Stop();
		}

		Vector2 move = new Vector3();

		#if UNITY_ANDROID
		//Touch based panning
		if(Input.touchCount == 1) {
			move = -Input.GetTouch(0).deltaPosition;
		}
		#else
		if(Input.mousePresent) {
			if( Input.mousePosition.x < mouseMargin || Input.mousePosition.x > Screen.width - mouseMargin ||
			   Input.mousePosition.y < mouseMargin || Input.mousePosition.y > Screen.height - mouseMargin ) {
				move = new Vector3( Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
				move.Normalize();
			}
		}
		#endif

		targetPos += targetScroll*right*move.x*moveSpeed*Time.deltaTime;
		targetPos += targetScroll*upward*move.y*moveSpeed*Time.deltaTime;

		targetPos.x = Mathf.Clamp(targetPos.x, 0, gridLoader.mapWidth*gridLoader.tileSize);
		targetPos.z = Mathf.Clamp(targetPos.z, 0, gridLoader.mapHeight*gridLoader.tileSize);

		if(mode == "meteor") {
			meteorSelect.renderer.enabled = true;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter;
			groundPlane.Raycast(ray, out enter);

			Vector3 selectionPosition = ray.GetPoint(enter);
			meteorSelect.position = selectionPosition;

			//Cancel action
			if(Input.GetMouseButtonUp(1))
				mode = "panning";

			if(Input.GetMouseButtonUp(0) && !guiScript.mouseInUI() ) {
				mode = "panning";
				simulationScript.Meteor(selectionPosition, 1);
			}
		}
		else {
			meteorSelect.renderer.enabled = false;
		}

		//Move Rain System
		rainSystem.transform.position = transform.position + new Vector3(0,10,0);
		if(rainTime > 0.0f) {
			rainTime -= Time.deltaTime;
			rainSystem.particleSystem.Play();

			if(!rainSound.audio.isPlaying)
				rainSound.audio.Play();
		}
		else {
			rainSystem.particleSystem.Stop();

			if(rainSound.audio.isPlaying)
				rainSound.audio.Stop();
		}
	}

	public Transform rainSound;
	public Transform quakeSound;
	public Transform sizzleSound;
}
