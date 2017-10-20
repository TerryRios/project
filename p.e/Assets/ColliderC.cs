using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderC : MonoBehaviour {

	public character _characterScript;

	void OnTriggerEnter(Collider col){
		if (col.gameObject) {
			_characterScript._noObstacle = false;
			Debug.Log ("obs");
		}
	}

	void OnTriggerExit(Collider col){
		if (_characterScript._noObstacle == false) {
			_characterScript._noObstacle = true;
		}
	}
}

