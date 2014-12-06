using UnityEngine;
using System.Collections;

public class PilotScript : MonoBehaviour {
	[SerializeField] GameObject defaultCharacter;
	[SerializeField] RuntimeAnimatorController defaultAnimatorController;
	[SerializeField] GameObject headCamera;

	[SerializeField] bool useIK = true;
	[SerializeField] bool useVR = true;
	[SerializeField] bool useLeap = false;
	[SerializeField] bool useSixense = false;

	GameObject character;
	Animator animator;

	Transform leftHandTarget;
	Transform rightHandTarget;

	IKLimb ikLimbTorso;
	IKLimb ikLimbLeftHand;
	IKLimb ikLimbRightHand;

	GameObject hmd;

	[SerializeField] Vector3 avatarHeadlookOffset = Vector3.zero;
	[SerializeField] Vector3 handRotationOffset = Vector3.zero; //This value should be tweaked on a per-model basis.

	// Use this for initialization
	void Start () {
		CreateCharacter (defaultCharacter);
		if (useVR) {
			InitializeVR();
		}
		if (useIK) {
			InitIKFromRig(character.GetComponent<IKRigConfig>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeVR(){
		Debug.Log ("Initializing VR");
		Transform head = animator.GetBoneTransform (HumanBodyBones.Head);
		hmd = (GameObject) GameObject.Instantiate (headCamera, head.position, Quaternion.identity);
		hmd.transform.parent = head;
		hmd.transform.localPosition = Vector3.zero + (Vector3.down * .25f); //Move the hmd down a little bit from where the head is, do this better.

		//hmd.transform.localRotation = Quaternion.identity;
		hmd.transform.parent = transform;

		Transform hmdTarget = hmd.transform.Find ("CenterEyeAnchor/HeadIKTarget");
		hmdTarget.localRotation = head.localRotation;
	}

	public void CreateCharacter(GameObject prefab){
		Debug.Log ("Creating Mecanim character!"); 
		GameObject _character = (GameObject) Instantiate (prefab, transform.position, transform.rotation); 
		animator = _character.GetComponent<Animator> (); 
		animator.runtimeAnimatorController = defaultAnimatorController; 
		_character.transform.parent = transform; 
		character = _character; 
		Debug.Log ("Mecanim character created.");
	}

	void InitIKFromRig(IKRigConfig rig){
		if (rig == null) {
			Debug.LogError ("*** Cannot initialize from rig, missing rig. ***");
			//InitializeIKJoints(); //Use the old fashioned one as a fallback for now.
		}else{
			Debug.Log ("- Creating Torso from Rig"); // I'll be honest this part is kind of fucked.
			Transform hmdTarget = hmd.transform.Find ("CenterEyeAnchor/HeadIKTarget"); //Get the HeadIKTarget that we added to the OVRCameraRig.
			hmdTarget.transform.Rotate(rig.headIKOffset);
			ikLimbTorso = CreateIKLimb(
				"Torso", 
				animator.GetBoneTransform(rig.torsoBaseBone), 
				animator.GetBoneTransform(rig.torsoJointBone), 
				animator.GetBoneTransform(rig.torsoTipBone), 
				rig.torsoRotationPolicy, 
				rig.torsoFalseBaseParent,
				rig.torsoJointParent,
				rig.torsoJointOffset
			);
			ikLimbTorso.target.parent = hmdTarget;
			ikLimbTorso.elbowTarget.parent = animator.GetBoneTransform(rig.torsoJointParent);
			ikLimbTorso.IsEnabled = true;
			Debug.Log ("- Created Torso from Rig");
		}
	}

	IKLimb CreateIKLimb(string name, 
	                  Transform baseT, 
	                  Transform jointT, 
	                  Transform tipT, 
	                  IKLimb.HandRotations rotationPolicy = IKLimb.HandRotations.KeepLocalRotation, 
	                  HumanBodyBones falseBaseParent = HumanBodyBones.LastBone,
	                  HumanBodyBones elbowParent = HumanBodyBones.LastBone, 
	                  Vector3 elbowOffset = default(Vector3)
	    			){

		//Create the IKLimb GameObject
		Debug.Log ("Creating " + name + " IKLimb GameObject with false base: " + falseBaseParent);
		GameObject go = new GameObject ();
		go.name = "IKLimb" + name;
		go.transform.parent = transform;
		IKLimb limb = go.AddComponent<IKLimb> ();

		//Create the IKLimb Joint Target GameObject
		GameObject jointTarget = new GameObject (name + "JointTarget");
		if(elbowParent != HumanBodyBones.LastBone){
			Debug.Log ("Adding elbow to a different parent.");
			jointTarget.transform.parent = animator.GetBoneTransform(elbowParent);
			Debug.Log ("Before: " + jointTarget.transform.position);
			//jointTarget.transform.position = animator.GetBoneTransform(elbowParent).position;
			//jointTarget.transform.localPosition = Vector3.zero;
			//Quaternion eulers = Quaternion.Euler(Vector3.zero);
			//jointTarget.transform.localRotation = jointTarget.transform.parent.localRotation;
		}else{
			jointTarget.transform.parent = transform;
		}
		if(elbowOffset != Vector3.zero){
			Debug.Log ("Adding an offset of " + elbowOffset);
			jointTarget.transform.localPosition += elbowOffset;
		}

		//Create the IKLimb Tip Target GameObject
		GameObject tipTarget = new GameObject (name + "TipTarget");
		tipTarget.transform.position = tipT.position;
		tipTarget.transform.rotation = tipT.rotation;
		
		//Assign all the values to the IKLimb
		if (falseBaseParent != HumanBodyBones.LastBone) {
			Debug.Log ("WTF FALSE BASE PARENT: " + falseBaseParent);
			GameObject fbp = new GameObject("FalseBaseParent");
			fbp.transform.parent = animator.GetBoneTransform(falseBaseParent);
			limb.upperArm = fbp.transform;
		}else{
			limb.upperArm = baseT;
		}
		limb.forearm = jointT;
		limb.hand = tipT;
		limb.elbowTarget = jointTarget.transform;
		limb.target = tipTarget.transform;
		limb.handRotationPolicy = rotationPolicy;
		return limb;
	}

	void InitializeIKJoints(){
		IKRigConfig rig = character.GetComponent<IKRigConfig> ();
		if ( rig != null ){
			return;
		}

		Debug.Log ("Creating IK Joints"); 
		Debug.Log ("- Creating Torso"); // I'll be honest this part is kind of fucked.
		GameObject torsoGO = new GameObject (); //Create the GameObject to hold the torso's IKLimb script.
		torsoGO.name = "IKLimbTorso"; //Doesn't work?
		torsoGO.transform.parent = transform; //Give the GO the correct parent.
		ikLimbTorso = torsoGO.AddComponent<IKLimb> (); //Add the IKLimb Script.
		ikLimbTorso.IsEnabled = false; //Don't turn it on yet.
		
		GameObject torsoElbowTarget = new GameObject (); //Create the target for the joint target.
		torsoElbowTarget.transform.parent = animator.GetBoneTransform (HumanBodyBones.Spine).parent.parent; //Pretty much the bottom-most Transform should be the parent of the elbow.
		torsoElbowTarget.transform.localPosition = torsoElbowTarget.transform.forward * 5; //For some reason putting it way in front of the avatar helps get rid of the twisty turny chest problem.

		Transform hmdTarget = hmd.transform.Find ("CenterEyeAnchor/HeadIKTarget"); //Get the HeadIKTarget that we added to the OVRCameraRig.
		hmdTarget.transform.rotation = animator.GetBoneTransform (HumanBodyBones.Head).rotation; //Offset the rotation.
		ikLimbTorso.elbowTarget = torsoElbowTarget.transform; //Tell the actual IKLimb what's what.
		ikLimbTorso.target = hmdTarget; //Tell it again.

		ikLimbTorso.handRotationPolicy = IKLimb.HandRotations.UseTargetRotation; //The head should match the rotation of the HMD.
		ikLimbTorso.IsEnabled = true; //Turn it on.

		ikLimbTorso.upperArm = animator.GetBoneTransform (HumanBodyBones.Spine); //This should be like the "hip" bone.
		ikLimbTorso.forearm = animator.GetBoneTransform (HumanBodyBones.Chest); //The W shaped bone in the chest if you have one.
		ikLimbTorso.hand = animator.GetBoneTransform (HumanBodyBones.Head); //The actual base Head bone. Neck is an optional bone so we're not using it.

		if (rig != null) {
			ikLimbTorso.upperArm = animator.GetBoneTransform(rig.torsoBaseBone);
			ikLimbTorso.forearm = animator.GetBoneTransform(rig.torsoJointBone);
			ikLimbTorso.hand = animator.GetBoneTransform(rig.torsoTipBone);
		}

		Debug.Log ("- Created Torso");

		/* *************************************************** */
		Debug.Log ("- Creating Both Arms");
		Debug.Log ("-- Creating Left Arm");
		GameObject lhGO = new GameObject ("IKLimbLeftHand"); //Create the GameObject to hold the left hand's IKLimb script.
		lhGO.transform.parent = transform;
		ikLimbLeftHand = lhGO.AddComponent<IKLimb>();
		ikLimbLeftHand.IsEnabled = false;
		ikLimbLeftHand.upperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
		ikLimbLeftHand.forearm = animator.GetBoneTransform (HumanBodyBones.LeftLowerArm);
		ikLimbLeftHand.hand = animator.GetBoneTransform (HumanBodyBones.LeftHand);
		GameObject leftElbowTarget = new GameObject("Left Elbow Target"); // IK Target for left elbow.
		GameObject leftHandTarget = new GameObject("Left Hand Target"); // IK Target for left hand.
		/*
		Debug.Log ("-- Creating Right Arm");
		GameObject rhGO = new GameObject ("IKLimbRightHand"); //Create the GameObject to hold the right hand's IKLimb script.
		rhGO.transform.parent = transform;
		ikLimbRightHand = rhGO.AddComponent<IKLimb>();
		ikLimbRightHand.IsEnabled = false;
		ikLimbRightHand.upperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm); //Upper Arm or Shoulder depending.
		ikLimbRightHand.forearm = animator.GetBoneTransform (HumanBodyBones.RightLowerArm); //Lower Arm.
		ikLimbRightHand.hand = animator.GetBoneTransform (HumanBodyBones.RightHand); // Hand/Wrist.
		GameObject rightElbowTarget = new GameObject ("Right Elbow Target"); // IK Target for right elbow.
		GameObject rightHandTarget = new GameObject ("Right Hand Target"); // IK Target for right hand.
		*/
		if (useLeap) { //Initialize LeapMotion elbow/hand targets

		} else if (useSixense) { //Initialize Sixense/Hydra elbow/hand targets

		}else{ //Create free-floating elbow/hand targets.
			leftElbowTarget.transform.position = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).transform.position + transform.forward * .1f + transform.right * -.125f + transform.up * -.1f;
			leftHandTarget.transform.position = leftElbowTarget.transform.position + transform.forward * .2f;
			leftHandTarget.transform.rotation = Quaternion.Euler(handRotationOffset);
			ikLimbLeftHand.handRotationPolicy = IKLimb.HandRotations.UseTargetRotation;

		}

		ikLimbLeftHand.elbowTarget = leftElbowTarget.transform;
		ikLimbLeftHand.target = leftHandTarget.transform;
		/*
		ikLimbRightHand.elbowTarget = rightElbowTarget.transform;
		ikLimbRightHand.target = rightHandTarget.transform;*/

		ikLimbLeftHand.IsEnabled = true;

		//Let's avoid fingers for as long as possible.
	}
}
