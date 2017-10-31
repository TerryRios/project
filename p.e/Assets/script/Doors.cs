using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

	Animator animator;
	bool doorOpen;
	public character _character;
	private BoxCollider _box;

	void Start(){
		doorOpen = false;
		animator = GetComponent<Animator> ();
		_box = GetComponent<BoxCollider> ();
	}

	void Update () {
		if (_character.toOpenDoor1 == true&&_character.toOpenDoor2==true) {
			_box.enabled = true;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Player") {
			AudioManager.instance.PlaySound("door_open",transform.position);
			doorOpen = true;
			DoorsControl ("Open");
		}

	}

	void OnTriggerExit(Collider col){
		if (doorOpen) {
			AudioManager.instance.PlaySound("door_close",transform.position);
			doorOpen = false;
			DoorsControl ("Close");
		}
	}

	void DoorsControl(string direction)
	{
		animator.SetTrigger(direction);
	}
}
