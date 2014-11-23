using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PilotController : MonoBehaviour {
	GameObject debugOverlay;
	Canvas debugOverlayCanvas;
	[SerializeField] Text fpsField;
	string preText = "FPS: ";
	string postText = "!";
	float deltaTime;

	// Use this for initialization
	void Start () {
		debugOverlay = GameObject.Find ("DebugOverlay");
		debugOverlayCanvas = debugOverlay.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		CalculateFPS ();
		DisplayFPS ();

		if (debugOverlayCanvas.enabled) {
			DisplayFPS (); 
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}

		if(Input.GetKeyDown(KeyCode.Tab)){
			OVRManager.display.RecenterPose();
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			debugOverlayCanvas.enabled = !debugOverlayCanvas.enabled;
		}
	}

	void CalculateFPS(){
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void DisplayFPS(){
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{1:0} fps ({0:0} ms)", msec, fps);
		fpsField.text = preText + text + postText;
	}
}