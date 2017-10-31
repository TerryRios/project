using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) {
			if (GetComponent<Light> ().enabled == false) {
				GetComponent<Light> ().enabled = true;
			} else {
				GetComponent<Light> ().enabled = false;
			}
		}


		
	}
}
