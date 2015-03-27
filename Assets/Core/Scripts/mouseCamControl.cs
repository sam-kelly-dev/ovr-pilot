using UnityEngine;
using System.Collections;

public class mouseCamControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.localEulerAngles += new Vector3(Input.GetAxis("Mouse Y"),Input.GetAxis ("Mouse X"));
	}
}
