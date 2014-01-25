using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	public Texture2D meteorIcon;
	public Texture2D earthquakeIcon;

	private static Rect meteorRect = new Rect(Screen.width/2 - 50,Screen.height - 120,100,100);

	public static bool mouseInUI() {
		return meteorRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
	}

	void OnGUI () {
		if( GUI.Button (meteorRect , meteorIcon ) ) {
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>().mode = "meteor";
		}
	}
}
