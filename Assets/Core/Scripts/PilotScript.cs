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
		GetInput ();
	}

	void GetInput(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			OVRManager.display.RecenterPose();
		}
	}

	void InitializeVR(){
		Debug.Log ("Initializing VR");
		Transform head = animator.GetBoneTransform (HumanBodyBones.Head);
		hmd = (GameObject) GameObject.Instantiate (headCamera, head.position, Quaternion.identity);
		hmd.transform.parent = head;
		hmd.transform.localPosition = Vector3.zero + (Vector3.down * .25f)+avatarHeadlookOffset; //Move the hmd down a little bit from where the head is, do this better.

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
			return;
		}

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
		if(ikLimbTorso.elbowTarget.parent==null)
			ikLimbTorso.elbowTarget.parent = this.transform;
		ikLimbTorso.IsEnabled = true;
		Debug.Log ("- Created Torso from Rig");

		ikLimbLeftHand = CreateIKLimb (
			"LeftArm",
			animator.GetBoneTransform(rig.leftArmBaseBone),
			animator.GetBoneTransform(rig.leftArmJointBone),
			animator.GetBoneTransform(rig.leftArmTipBone),
			rig.leftArmRotationPolicy,
			rig.leftArmFalseBaseParent,
			rig.leftArmJointParent,
			rig.leftArmJointOffset
		);
		ikLimbLeftHand.target.transform.localPosition = Vector3.zero;
		ikLimbLeftHand.target.transform.position += ikLimbTorso.target.transform.right * -.2f;
		ikLimbLeftHand.IsEnabled = true;
		Debug.Log ("- Created LeftArm from Rig.");

		ikLimbRightHand = CreateIKLimb (
			"RightArm",
			animator.GetBoneTransform(rig.rightArmBaseBone),
			animator.GetBoneTransform(rig.rightArmJointBone),
			animator.GetBoneTransform(rig.rightArmTipBone),
			rig.rightArmRotationPolicy,
			rig.rightArmFalseBaseParent,
			rig.rightArmJointParent,
			rig.rightArmJointOffset
		);
		ikLimbRightHand.target.transform.localPosition = Vector3.zero;
		ikLimbRightHand.target.transform.position += ikLimbTorso.target.transform.right * .2f;
		ikLimbRightHand.IsEnabled = true;
		Debug.Log ("- Created RightArm from Rig.");
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
		GameObject go = new GameObject ();
		go.name = "IKLimb" + name;
		go.transform.parent = transform;
		IKLimb limb = go.AddComponent<IKLimb> ();

		//Create the IKLimb Joint Target GameObject
		GameObject jointTarget = new GameObject (name + "JointTarget");
		if(elbowParent != HumanBodyBones.LastBone){
			jointTarget.transform.parent = animator.GetBoneTransform(elbowParent);
			jointTarget.transform.localPosition = Vector3.zero;
		}else{
			jointTarget.transform.parent = this.transform;
			jointTarget.transform.localPosition = Vector3.zero;
		}
		if(elbowOffset != Vector3.zero){
			jointTarget.transform.localPosition += elbowOffset;
		}

		//Create the IKLimb Tip Target GameObject
		GameObject tipTarget = new GameObject (name + "TipTarget");
		tipTarget.transform.parent = this.transform;
		tipTarget.transform.position = tipT.position;
		tipTarget.transform.rotation = tipT.rotation;
		
		//Assign all the values to the IKLimb
		if (falseBaseParent != HumanBodyBones.LastBone) {
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
}
