using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	
	public Transform meteorFrame;
	public Transform rainFrame;
	public Transform quakeFrame;

	private GUITexture meteorTexture;
	private GUITexture rainTexture;
	private GUITexture quakeTexture;

	private CameraControl cameraControl;

	void Start() {
		meteorTexture = meteorFrame.guiTexture;
		rainTexture = rainFrame.guiTexture;
		quakeTexture = quakeFrame.guiTexture;

		cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
	}

	float GUIx( int x ) {
		return ((float)(x))/Screen.width;
	}

	float GUIy( int y ) {
		return ((float)(y))/Screen.height;
	}


	public bool mouseInUI() {
		return meteorTexture.GetScreenRect().Contains(Input.mousePosition);
	}

	void Update () {
		meteorTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100),GUIy( 20 ),1);
		rainTexture.transform.position = new Vector3(GUIx(Screen.width/2 - 100 - 200 - 20),GUIy( 20 ),1);

		if( Input.GetMouseButtonUp(0) && meteorTexture.GetScreenRect().Contains(Input.mousePosition) ) {
			cameraControl.mode = "meteor";
		}

		if( Input.GetMouseButtonUp(0) && rainTexture.GetScreenRect().Contains(Input.mousePosition) ) {
			cameraControl.rainTime = 5.0f;
		}
	}
}
