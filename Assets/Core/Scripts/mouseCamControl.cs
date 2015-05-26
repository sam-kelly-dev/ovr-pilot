using UnityEngine;
using System.Collections;

public class mouseCamControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localRotation=SimpleQuaternion.rotateOnLocalAxis (this.transform.localRotation, Input.GetAxis ("Mouse X"), Vector3.up);
		this.transform.localRotation=SimpleQuaternion.rotateOnAxis (this.transform.localRotation, Input.GetAxis ("Mouse Y"), Vector3.right);
		//this.transform.localEulerAngles.y += Input.GetAxis ("Mouse Y"); //+= new Vector3(Input.GetAxis("Mouse Y"),Input.GetAxis ("Mouse X"),0);
	}
}
