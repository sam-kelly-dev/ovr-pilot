using UnityEngine;
using System.Collections;

public class IKRigConfig : MonoBehaviour {

	public string name = "IKRigConfig_BASE";

	public HumanBodyBones torsoBaseBone = HumanBodyBones.Spine;
	public HumanBodyBones torsoJointBone = HumanBodyBones.Neck; // The neck gets all twisty.
	public HumanBodyBones torsoTipBone = HumanBodyBones.Head;
	public HumanBodyBones torsoFalseBaseParent = HumanBodyBones.Spine;
	public HumanBodyBones torsoJointParent = HumanBodyBones.Neck;
	public IKLimb.HandRotations torsoRotationPolicy = IKLimb.HandRotations.UseTargetRotation;
	public Vector3 torsoJointOffset = Vector3.zero;

	public HumanBodyBones leftArmBaseBone = HumanBodyBones.LeftShoulder;
	public HumanBodyBones leftArmJointBone = HumanBodyBones.LeftLowerArm;
	public HumanBodyBones leftArmTipBone = HumanBodyBones.LeftHand;
	public HumanBodyBones leftArmJointParent = HumanBodyBones.LastBone;
	public IKLimb.HandRotations leftArmRotationPolicy = IKLimb.HandRotations.UseTargetRotation;

	public HumanBodyBones rightArmBaseBone = HumanBodyBones.RightShoulder;
	public HumanBodyBones rightArmJointBone = HumanBodyBones.RightLowerArm;
	public HumanBodyBones rightArmTipBone = HumanBodyBones.RightHand;
	public HumanBodyBones rightArmJointParent = HumanBodyBones.LastBone;
	public IKLimb.HandRotations rightArmRotationPolicy = IKLimb.HandRotations.UseTargetRotation;

	public Vector3 headIKOffset = new Vector3(0,90,270);

	// Use this for initialization
	void Start () {

	}

}
