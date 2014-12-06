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

	SkinnedMeshRenderer smr;
	Animator animator;

	GameObject character;

	// Use this for initialization
	void Start () {
		debugOverlay = GameObject.Find ("DebugOverlay");
		debugOverlayCanvas = debugOverlay.GetComponent<Canvas> ();

		animator = GetComponent<Animator> ();
		SetIKLimbTargets (controllerPose);
		animator.Play("Hands Layer.HoldingController");
		animator.SetFloat("Fistiness", .5f);
		controllerRenderer.enabled = true;

		//InitializeCharacter ();
	}
	void InitializeCharacter(GameObject prefab, Avatar avatar){
		/*
		character = GameObject.Instantiate (prefab, transform.position, transform.rotation);
		Animator _anim = character.AddComponent<Animator> ();
		_anim.avatar = avatar;*/
		//leftHand = character.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.

	}
	// Update is called once per frame
	void Update () {
		CalculateFPS ();
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
		if (Input.GetKeyDown (KeyCode.C)) {
			if(currentPose == controllerPose){
				SetIKLimbTargets(relaxedPose);
				controllerRenderer.enabled = false;
				animator.Play("Hands Layer.Idle");
				Debug.Log ("Controller renderer is now enabled");
			}else if(currentPose == relaxedPose){
				SetIKLimbTargets(controllerPose);
				animator.Play("Hands Layer.HoldingController");
				animator.SetFloat("Fistiness", .5f);
				controllerRenderer.enabled = true;
				Debug.Log ("Controller renderer is now NOT enabled");
			}
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

	[SerializeField] IKPose currentPose;
	[SerializeField] IKPose controllerPose;
	[SerializeField] IKPose relaxedPose;
	[SerializeField] MeshRenderer controllerRenderer;

	void GripController(){
		Debug.Log ("Gripping controller.");
		currentPose = controllerPose;
	}

	void Relaxed(){
		Debug.Log ("Relaxing hands.");
		currentPose = relaxedPose;
	}

	[SerializeField] IKLimb leftHand;
	[SerializeField] IKLimb leftThumb;
	[SerializeField] IKLimb leftIndex;
	[SerializeField] IKLimb leftMiddle;
	[SerializeField] IKLimb leftRing;
	[SerializeField] IKLimb leftPinky;
	
	[SerializeField] IKLimb rightHand;
	[SerializeField] IKLimb rightThumb;
	[SerializeField] IKLimb rightIndex;
	[SerializeField] IKLimb rightMiddle;
	[SerializeField] IKLimb rightRing;
	[SerializeField] IKLimb rightPinky;

	void SetIKLimbTargets(IKPose pose){
		currentPose = pose;

		if (pose == controllerPose) {

		}

		leftHand.target = pose.leftHand.target;
		leftHand.elbowTarget = pose.leftHand.elbow;

		/*
		if (pose.leftHand.fingers != null) {
			leftThumb.IsEnabled = true;
			leftIndex.IsEnabled = true;
			leftMiddle.IsEnabled = true;
			leftRing.IsEnabled = true;
			//leftPinky.IsEnabled = true;

			leftThumb.target = pose.leftHand.fingers.thumb.target;
			leftThumb.elbowTarget = pose.leftHand.fingers.thumb.elbowTarget;
			leftIndex.target = pose.leftHand.fingers.index.target;
			leftIndex.elbowTarget = pose.leftHand.fingers.index.elbowTarget;
			leftMiddle.target = pose.leftHand.fingers.middle.target;
			leftMiddle.elbowTarget = pose.leftHand.fingers.middle.elbowTarget;
			leftRing.target = pose.leftHand.fingers.ring.target;
			leftRing.elbowTarget = pose.leftHand.fingers.ring.elbowTarget;
			//leftPinky.target = pose.leftHand.fingers.pinky.target;
			//leftPinky.elbowTarget = pose.leftHand.fingers.pinky.elbowTarget;			 
		}else
		{
			leftThumb.IsEnabled = false;
			leftIndex.IsEnabled = false;
			leftMiddle.IsEnabled = false;
			leftRing.IsEnabled = false;
			leftPinky.IsEnabled = false;
		}
		*/
		rightHand.target = pose.rightHand.target;
		rightHand.elbowTarget = pose.rightHand.elbow;
		/*
		if (pose.rightHand.fingers != null) {
			rightThumb.IsEnabled = true;
			rightIndex.IsEnabled = true;
			rightMiddle.IsEnabled = true;
			rightRing.IsEnabled = true;
			rightPinky.IsEnabled = true;

			rightThumb.target = pose.rightHand.fingers.thumb.target;
			rightThumb.elbowTarget = pose.rightHand.fingers.thumb.elbowTarget;
			rightIndex.target = pose.rightHand.fingers.index.target;
			rightIndex.elbowTarget = pose.rightHand.fingers.index.elbowTarget;
			rightMiddle.target = pose.rightHand.fingers.middle.target;
			rightMiddle.elbowTarget = pose.rightHand.fingers.middle.elbowTarget;
			rightRing.target = pose.rightHand.fingers.ring.target;
			rightRing.elbowTarget = pose.rightHand.fingers.ring.elbowTarget;
			rightPinky.target = pose.rightHand.fingers.pinky.target;
			rightPinky.elbowTarget = pose.rightHand.fingers.pinky.elbowTarget;
		}else{
			rightThumb.IsEnabled = false;
			rightIndex.IsEnabled = false;
			rightMiddle.IsEnabled = false;
			rightRing.IsEnabled = false;
			rightPinky.IsEnabled = false;
		}
*/

	}
}