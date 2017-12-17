using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engineScript : MonoBehaviour {

	public movement _character;
	public GameObject _reader1;
	public GameObject _reader2;

	void Update () {
		if (_character.engineisOn == true) {
			_reader1.GetComponent<BoxCollider> ().enabled = true;
			_reader2.GetComponent<BoxCollider> ().enabled = true;
		}
	}
}
