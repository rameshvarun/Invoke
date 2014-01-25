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

	private Camera camera;

	// Use this for initialization
	void Start () {
		//This determines how the mouse direction is mapped to movements in the 3d space
		right = new Vector3(1,0,-1).normalized;
		upward = new Vector3(1,0,1).normalized;

		camera = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		//Mouse scroll
		targetScroll -= scrollIncrement*Input.GetAxis("Mouse ScrollWheel");
		//Clamp scroll to max/min bounds
		targetScroll = Mathf.Clamp(targetScroll, scrollBoundaries.x, scrollBoundaries.y);

		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetScroll, scrollSpeed*Time.deltaTime);

		transform.position = targetPos + displacement;
		transform.LookAt(targetPos);

		Vector2 move = new Vector3();

		#if UNITY_ANDROID
		//Touch based panning
		if(Input.touchCount == 1) {
			move = -Input.GetTouch(0).deltaPosition;
		}
		#else
		if( Input.mousePosition.x < mouseMargin || Input.mousePosition.x > Screen.width - mouseMargin ||
		   Input.mousePosition.y < mouseMargin || Input.mousePosition.y > Screen.height - mouseMargin ) {
			move = new Vector3( Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
		}
		#endif

		move.Normalize();
		targetPos += targetScroll*right*move.x*moveSpeed*Time.deltaTime;
		targetPos += targetScroll*upward*move.y*moveSpeed*Time.deltaTime;
	}
}
